#!/bin/bash
set -e

# Generate password
PASSWORD=$(openssl rand -base64 32 | tr -dc "A-Za-z0-9" | head -c 16)

# Replace placeholder
sed "s/{{PASSWORD}}/$PASSWORD/" cache.yml > docker-compose.yml

# Generate certificates
certbot certonly --nginx -d cache.tramtimes.net \
  --email philvessey@outlook.com \
  --non-interactive \
  --no-eff-email \
  --agree-tos

# Create stream
tee "/etc/nginx/stream.conf" > /dev/null <<'EOF'
server {
    listen 6380 ssl;
    proxy_pass 172.28.0.2:6379;

    ssl_certificate     /etc/letsencrypt/live/cache.tramtimes.net/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/cache.tramtimes.net/privkey.pem;

    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_ciphers         HIGH:!aNULL:!MD5;
}
EOF

# Update nginx
if ! grep -q "stream {" "/etc/nginx/nginx.conf"; then
    sed -i '/^http {/i\
stream {\
    include /etc/nginx/stream.conf;\
}\
' /etc/nginx/nginx.conf
fi

# Restart nginx
systemctl restart nginx

# Prompt complete
echo ""
echo "✅ Setup complete. Use command: sudo docker compose up -d"
echo "✅ Setup complete. Use connection: cache.tramtimes.net:6380,password=$PASSWORD,ssl=true,abortConnect=false"
echo ""