#!/bin/bash
set -e

# Generate password
PASSWORD=$(openssl rand -base64 16)

# Replace placeholder
sed "s/{{PASSWORD}}/$PASSWORD/" cache.yml > docker-compose.yml

# Prompt deploy
echo "✅ Setup complete. Use command: sudo docker compose up -d with password: $PASSWORD"