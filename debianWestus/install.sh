sudo apt install git

sudo echo "" > /etc/motd
sudo echo " _ _ _         _   _____ _____    _____ _____ _____    _____                     " >> /etc/motd
sudo echo "| | | |___ ___| |_|  |  |   __|  |  |  |  _  |   | |  |   __|___ ___ _ _ ___ ___ " >> /etc/motd
sudo echo "| | | | -_|_ -|  _|  |  |__   |  |  |  |   __| | | |  |__   | -_|  _| | | -_|  _|" >> /etc/motd
sudo echo "|_____|___|___|_| |_____|_____|   \___/|__|  |_|___|  |_____|___|_|  \_/|___|_|  " >> /etc/motd

sudo apt update
sudo apt install openvpn

sudo -u mueha0 wget -P ~/ https://github.com/OpenVPN/easy-rsa/releases/download/v3.0.8/EasyRSA-3.0.8.tgz

sudo -u mueha0 tar xvf ~/EasyRSA-3.0.8.tgz
sudo -u mueha0 rm ~/EasyRSA-3.0.8.tgz

sudo -u mueha0 cd ~/EasyRSA-v3.0.8/
sudo -u mueha0 cp vars.example vars

sudo -u mueha0 sed -i 's/#set_var EASYRSA_REQ_COUNTRY\t"US"/set_var EASYRSA_REQ_COUNTRY\t"CH"/' vars
sudo -u mueha0 sed -i 's/#set_var EASYRSA_REQ_PROVINCE\t"California"/set_var EASYRSA_REQ_PROVINCE\t"SomePlace"/' vars
sudo -u mueha0 sed -i 's/#set_var EASYRSA_REQ_CITY\t"San Francisco"/set_var EASYRSA_REQ_CITY\t"SomeTown"/' vars
sudo -u mueha0 sed -i 's/#set_var EASYRSA_REQ_ORG\t"Copyleft Certificate Co"/set_var EASYRSA_REQ_ORG\t\t"Muehan Fun Company"/' vars
sudo -u mueha0 sed -i 's/#set_var EASYRSA_REQ_EMAIL\t"me@example.net"/set_var EASYRSA_REQ_EMAIL\t"asdf@asdf.net"/' vars
sudo -u mueha0 sed -i 's/#set_var EASYRSA_REQ_OU\t\t"My Organizational Unit"/set_var EASYRSA_REQ_OU\t\t"Muehan Org Unit"/' vars

sudo -u mueha0 export EASYRSA_BATCH=1
sudo -u mueha0 ./easyrsa init-pki
sudo -u mueha0 ./easyrsa build-ca nopass