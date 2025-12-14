#!/bin/bash
set -e

# Generate password
PASSWORD=$(openssl rand -base64 32 | tr -dc "A-Za-z0-9" | head -c 16)

# Replace placeholder
sed "s/{{PASSWORD}}/$PASSWORD/" search.yml > docker-compose.yml

# Generate certificates
certbot certonly --nginx -d search.tramtimes.net \
  --email philvessey@outlook.com \
  --non-interactive \
  --no-eff-email \
  --agree-tos

# Modify certificates
chmod 644 /etc/letsencrypt/live/search.tramtimes.net/fullchain.pem
chown 33:33 /etc/letsencrypt/live/search.tramtimes.net/fullchain.pem
chmod 600 /etc/letsencrypt/live/search.tramtimes.net/privkey.pem
chown 33:33 /etc/letsencrypt/live/search.tramtimes.net/privkey.pem

# Modify paths
chmod 755 /etc/letsencrypt
chmod 755 /etc/letsencrypt/live
chmod 755 /etc/letsencrypt/archive

# Create directory
mkdir -p /etc/elasticsearch

# Create instances
tee /etc/elasticsearch/instances.yml > /dev/null <<'EOF'
instances:
  - name: search1
    dns:
      - localhost
      - search1
    ip:
      - 127.0.0.1
      - 172.28.0.2

  - name: search2
    dns:
      - localhost
      - search2
    ip:
      - 127.0.0.1
      - 172.28.0.3

  - name: search3
    dns:
      - localhost
      - search3
    ip:
      - 127.0.0.1
      - 172.28.0.4
EOF

# Modify instances
chmod 644 /etc/elasticsearch/instances.yml
chown 1000:1000 /etc/elasticsearch/instances.yml

# Generate authority
docker run --rm -v "$PWD:/working" docker.elastic.co/elasticsearch/elasticsearch:8.17.3 \
  bin/elasticsearch-certutil ca -out /working/ca.zip \
  --pem > /dev/null 2>&1

# Unzip authority
unzip -oq ./ca.zip -d /etc/elasticsearch

# Modify authority
chmod 644 /etc/elasticsearch/ca/ca.crt
chown 1000:1000 /etc/elasticsearch/ca/ca.crt
chmod 600 /etc/elasticsearch/ca/ca.key
chown 1000:1000 /etc/elasticsearch/ca/ca.key

docker run --rm -v "$PWD:/working" -v "/etc/elasticsearch:/elasticsearch" docker.elastic.co/elasticsearch/elasticsearch:8.17.3 \
  bin/elasticsearch-certutil cert -out /working/certs.zip \
  --in /elasticsearch/instances.yml \
  --ca-cert /elasticsearch/ca/ca.crt \
  --ca-key /elasticsearch/ca/ca.key \
  --pem > /dev/null 2>&1

# Unzip certificates
unzip -oq ./certs.zip -d /etc/elasticsearch/

# Modify certificates
chmod 644 /etc/elasticsearch/search1/search1.crt
chown 1000:1000 /etc/elasticsearch/search1/search1.crt
chmod 600 /etc/elasticsearch/search1/search1.key
chown 1000:1000 /etc/elasticsearch/search1/search1.key
chmod 644 /etc/elasticsearch/search2/search2.crt
chown 1000:1000 /etc/elasticsearch/search2/search2.crt
chmod 600 /etc/elasticsearch/search2/search2.key
chown 1000:1000 /etc/elasticsearch/search2/search2.key
chmod 644 /etc/elasticsearch/search3/search3.crt
chown 1000:1000 /etc/elasticsearch/search3/search3.crt
chmod 600 /etc/elasticsearch/search3/search3.key
chown 1000:1000 /etc/elasticsearch/search3/search3.key

# Create directory
mkdir -p /etc/nginx/certs

# Copy certificates
cp /etc/elasticsearch/ca/ca.crt /etc/nginx/certs/elasticsearch.crt
cp /etc/elasticsearch/ca/ca.key /etc/nginx/certs/elasticsearch.key

# Modify certificates
chmod 644 /etc/nginx/certs/elasticsearch.crt
chown 33:33 /etc/nginx/certs/elasticsearch.crt
chmod 600 /etc/nginx/certs/elasticsearch.key
chown 33:33 /etc/nginx/certs/elasticsearch.key

# Create elasticsearch
tee "/etc/nginx/conf.d/elasticsearch.conf" > /dev/null <<'EOF'
server {
    listen 9201 ssl;
    server_name search.tramtimes.net;

    ssl_certificate     /etc/letsencrypt/live/search.tramtimes.net/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/search.tramtimes.net/privkey.pem;

    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_ciphers         HIGH:!aNULL:!MD5;

    location / {
        proxy_http_version 1.1;
        proxy_pass https://172.28.0.2:9200;

        proxy_ssl_name search1;
        proxy_ssl_trusted_certificate /etc/nginx/certs/elasticsearch.crt;
        proxy_ssl_verify on;
        proxy_ssl_verify_depth 2;

        proxy_set_header Connection "keep-alive";
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
EOF

# Restart nginx
systemctl restart nginx

# Setup renewal
tee /etc/letsencrypt/renewal-hooks/deploy/run.sh > /dev/null <<'EOF'
#!/bin/bash
set -e

chmod 644 /etc/letsencrypt/live/search.tramtimes.net/fullchain.pem
chown 33:33 /etc/letsencrypt/live/search.tramtimes.net/fullchain.pem
chmod 600 /etc/letsencrypt/live/search.tramtimes.net/privkey.pem
chown 33:33 /etc/letsencrypt/live/search.tramtimes.net/privkey.pem

chmod 755 /etc/letsencrypt
chmod 755 /etc/letsencrypt/live
chmod 755 /etc/letsencrypt/archive

mkdir -p /tmp/certbot

EXPIRY=$(openssl x509 -in "/etc/elasticsearch/ca/ca.crt" -noout -enddate | cut -d= -f2)

if [ $(date -d "$EXPIRY" +%s) -lt $(date -d "+365 days" +%s) ]; then
  docker run --rm -v "/tmp/certbot:/working" docker.elastic.co/elasticsearch/elasticsearch:8.17.3 \
    bin/elasticsearch-certutil ca -out /working/ca.zip \
    --pem > /dev/null 2>&1

  unzip -oq /tmp/certbot/ca.zip -d /etc/elasticsearch

  chmod 644 /etc/elasticsearch/ca/ca.crt
  chown 1000:1000 /etc/elasticsearch/ca/ca.crt
  chmod 600 /etc/elasticsearch/ca/ca.key
  chown 1000:1000 /etc/elasticsearch/ca/ca.key
fi

docker run --rm -v "/tmp/certbot:/working" -v "/etc/elasticsearch:/elasticsearch" docker.elastic.co/elasticsearch/elasticsearch:8.17.3 \
  bin/elasticsearch-certutil cert -out /working/certs.zip \
  --in /elasticsearch/instances.yml \
  --ca-cert /elasticsearch/ca/ca.crt \
  --ca-key /elasticsearch/ca/ca.key \
  --pem > /dev/null 2>&1

unzip -oq /tmp/certbot/certs.zip -d /etc/elasticsearch/

chmod 644 /etc/elasticsearch/search1/search1.crt
chown 1000:1000 /etc/elasticsearch/search1/search1.crt
chmod 600 /etc/elasticsearch/search1/search1.key
chown 1000:1000 /etc/elasticsearch/search1/search1.key
chmod 644 /etc/elasticsearch/search2/search2.crt
chown 1000:1000 /etc/elasticsearch/search2/search2.crt
chmod 600 /etc/elasticsearch/search2/search2.key
chown 1000:1000 /etc/elasticsearch/search2/search2.key
chmod 644 /etc/elasticsearch/search3/search3.crt
chown 1000:1000 /etc/elasticsearch/search3/search3.crt
chmod 600 /etc/elasticsearch/search3/search3.key
chown 1000:1000 /etc/elasticsearch/search3/search3.key

mkdir -p /etc/nginx/certs

cp /etc/elasticsearch/ca/ca.crt /etc/nginx/certs/elasticsearch.crt
cp /etc/elasticsearch/ca/ca.key /etc/nginx/certs/elasticsearch.key

chmod 644 /etc/nginx/certs/elasticsearch.crt
chown 33:33 /etc/nginx/certs/elasticsearch.crt
chmod 600 /etc/nginx/certs/elasticsearch.key
chown 33:33 /etc/nginx/certs/elasticsearch.key

trap 'rm -rf /tmp/certbot' EXIT

docker restart search1 search2 search3 > /dev/null 2>&1
systemctl restart nginx > /dev/null 2>&1
EOF

# Modify renewal
chmod +x /etc/letsencrypt/renewal-hooks/deploy/run.sh

# Prompt complete
echo ""
echo "✅ Setup complete. Use command: sudo docker compose up -d"
echo "✅ Setup complete. Use connection: https://elastic:$PASSWORD@search.tramtimes.net:9201"
echo ""