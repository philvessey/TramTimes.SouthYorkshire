#!/bin/bash
set -e

# Generate password
PASSWORD=$(openssl rand -base64 32 | tr -dc "A-Za-z0-9" | head -c 16)

# Replace placeholder
sed "s/{{PASSWORD}}/$PASSWORD/" server.yml > docker-compose.yml

# Generate certificates
certbot certonly --nginx -d server.tramtimes.net \
  --email philvessey@outlook.com \
  --non-interactive \
  --no-eff-email \
  --agree-tos

# Modify certificates
chmod 644 /etc/letsencrypt/live/server.tramtimes.net/fullchain.pem
chown 999:999 /etc/letsencrypt/live/server.tramtimes.net/fullchain.pem
chmod 600 /etc/letsencrypt/live/server.tramtimes.net/privkey.pem
chown 999:999 /etc/letsencrypt/live/server.tramtimes.net/privkey.pem

# Create directory
mkdir -p /etc/postgresql

# Create postgresql
tee /etc/postgresql/postgresql.conf > /dev/null <<'EOF'
listen_addresses = '*'
port = 5432

ssl = on
ssl_cert_file = '/certs/fullchain.pem'
ssl_key_file = '/certs/privkey.pem'
EOF

# Modify postgresql
chmod 644 /etc/postgresql/postgresql.conf
chown 999:999 /etc/postgresql/postgresql.conf

# Create stream
tee "/etc/nginx/stream.conf" > /dev/null <<'EOF'
server {
    listen 5433;
    proxy_pass 172.28.0.2:5432;
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

# Setup renewal
tee /etc/letsencrypt/renewal-hooks/deploy/run.sh > /dev/null <<'EOF'
#!/bin/bash
set -e

chmod 644 /etc/letsencrypt/live/server.tramtimes.net/fullchain.pem
chown 999:999 /etc/letsencrypt/live/server.tramtimes.net/fullchain.pem
chmod 600 /etc/letsencrypt/live/server.tramtimes.net/privkey.pem
chown 999:999 /etc/letsencrypt/live/server.tramtimes.net/privkey.pem

docker restart server > /dev/null 2>&1
EOF

# Modify renewal
chmod +x /etc/letsencrypt/renewal-hooks/deploy/run.sh

# Prompt complete
echo ""
echo "✅ Setup complete. Use command: sudo docker compose up -d"
echo "✅ Setup complete. Use command: sudo docker exec -it server psql -U postgres -d postgres"
echo "✅ Setup complete. Use connection: Host=server.tramtimes.net;Port=5433;SslMode=Require;Username=postgres;Password=$PASSWORD;Database=southyorkshire"
echo ""