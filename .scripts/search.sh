#!/bin/bash
set -e

# Generate password
PASSWORD=$(openssl rand -base64 16)

# Create directory
mkdir ./search

# Change permissions
chmod -R 777 ./search

#  Change directory
cd ./search

# Create instances
cat > ./instances.yml <<EOF
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

# Generate authority
docker run --rm -v "$PWD:/working" docker.elastic.co/elasticsearch/elasticsearch:8.17.3 \
  bin/elasticsearch-certutil ca \
  --out /working/authority.p12 --pass $PASSWORD

# Generate certificates
docker run --rm -v "$PWD:/working" docker.elastic.co/elasticsearch/elasticsearch:8.17.3 \
  bin/elasticsearch-certutil cert \
  --ca /working/authority.p12 --ca-pass $PASSWORD \
  --in /working/instances.yml --out /working/certificates.zip --pass $PASSWORD

# Unzip certificates
unzip ./certificates.zip

# Copy certificates
cp ./search1/search1.p12 ../search1.p12
cp ./search2/search2.p12 ../search2.p12
cp ./search3/search3.p12 ../search3.p12

#  Change directory
cd ..

# Delete directory
rm -rf ./search

# Replace placeholder
sed "s/{{PASSWORD}}/$PASSWORD/" search.yml > docker-compose.yml

# Prompt deploy
echo "✅ Setup complete. Use command: sudo docker compose up -d with password: $PASSWORD"