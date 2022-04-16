apt install git

echo "" > /etc/motd
echo " _ _ _         _   _____ _____    _____ _____ _____    _____                     " >> /etc/motd
echo "| | | |___ ___| |_|  |  |   __|  |  |  |  _  |   | |  |   __|___ ___ _ _ ___ ___ " >> /etc/motd
echo "| | | | -_|_ -|  _|  |  |__   |  |  |  |   __| | | |  |__   | -_|  _| | | -_|  _|" >> /etc/motd
echo "|_____|___|___|_| |_____|_____|   \___/|__|  |_|___|  |_____|___|_|  \_/|___|_|  " >> /etc/motd

apt update
apt install openvpn

wget -P ~/ https://github.com/OpenVPN/easy-rsa/releases/download/v3.0.8/EasyRSA-3.0.8.tgz

tar xvf ~/EasyRSA-3.0.8.tgz
rm ~/EasyRSA-3.0.8.tgz

cd ~/EasyRSA-v3.0.8/
cp vars.example vars

sed -i 's/#set_var EASYRSA_REQ_COUNTRY\t"US"/set_var EASYRSA_REQ_COUNTRY\t"CH"/' vars
sed -i 's/#set_var EASYRSA_REQ_PROVINCE\t"California"/set_var EASYRSA_REQ_PROVINCE\t"SomePlace"/' vars
sed -i 's/#set_var EASYRSA_REQ_CITY\t"San Francisco"/set_var EASYRSA_REQ_CITY\t"SomeTown"/' vars
sed -i 's/#set_var EASYRSA_REQ_ORG\t"Copyleft Certificate Co"/set_var EASYRSA_REQ_ORG\t\t"Muehan Fun Company"/' vars
sed -i 's/#set_var EASYRSA_REQ_EMAIL\t"me@example.net"/set_var EASYRSA_REQ_EMAIL\t"asdf@asdf.net"/' vars
sed -i 's/#set_var EASYRSA_REQ_OU\t\t"My Organizational Unit"/set_var EASYRSA_REQ_OU\t\t"Muehan Org Unit"/' vars

export EASYRSA_BATCH=1
./easyrsa init-pki
./easyrsa build-ca nopass