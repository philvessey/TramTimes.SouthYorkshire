#!/bin/bash
set -e

# Create admin
adduser --disabled-password --gecos "" admin
usermod -aG users,admin admin
usermod -s /bin/bash admin

# Grant permissions
echo "admin ALL=(ALL) NOPASSWD:ALL" > /etc/sudoers.d/admin
chmod 440 /etc/sudoers.d/admin

# Set SSH
mkdir -p /home/admin/.ssh
echo "ssh-rsa {{KEY}}" > /home/admin/.ssh/authorized_keys
chmod 700 /home/admin/.ssh
chmod 600 /home/admin/.ssh/authorized_keys
chown -R admin:admin /home/admin/.ssh

# Update packages
apt update && apt upgrade -y
apt install certbot -y
apt install docker-compose -y
apt install nginx libnginx-mod-stream python3-certbot-nginx -y
apt install unzip -y

# Harden SSH
cat <<EOF > /etc/ssh/sshd_config.d/ssh-hardening.conf
PermitRootLogin no
PasswordAuthentication no
KbdInteractiveAuthentication no
ChallengeResponseAuthentication no
MaxAuthTries 3
AllowTcpForwarding no
X11Forwarding no
AllowAgentForwarding no
AuthorizedKeysFile %h/.ssh/authorized_keys
AllowUsers admin
EOF

# Prompt reboot
echo ""
echo "✅ Setup complete. Use command: sudo reboot"
echo ""