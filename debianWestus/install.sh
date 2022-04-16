sudo apt install git

sudo echo "" > /etc/motd
sudo echo " _ _ _         _   _____ _____    _____ _____ _____    _____                     " >> /etc/motd
sudo echo "| | | |___ ___| |_|  |  |   __|  |  |  |  _  |   | |  |   __|___ ___ _ _ ___ ___ " >> /etc/motd
sudo echo "| | | | -_|_ -|  _|  |  |__   |  |  |  |   __| | | |  |__   | -_|  _| | | -_|  _|" >> /etc/motd
sudo echo "|_____|___|___|_| |_____|_____|   \___/|__|  |_|___|  |_____|___|_|  \_/|___|_|  " >> /etc/motd

sudo apt update
sudo apt install openvpn

wget -P ~/ https://github.com/OpenVPN/easy-rsa/releases/download/v3.0.6/EasyRSA-unix-v3.0.7.tgz

cd ~
tar xvf EasyRSA-unix-v3.0.7.tgz
rm EasyRSA-unix-v3.0.7.tgz

cd ~/EasyRSA-v3.0.7/
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