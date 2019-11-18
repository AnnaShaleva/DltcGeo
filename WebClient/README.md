# Chainbox ledger

# Установка блокчейна

```sh
cd network
./setHostname.sh 172.x.x.x
./raiseNetwork.sh kafka
```

# Проверка установки блокчейна
```sh
cp connection_profile_kafka.json ../
cd ../
python3 test_fabric.py
```

# Установка сервера
```sh
cd deployment
./rebuild_image.sh
./hard_restart.sh <path_to_chainbox-ledger_folder>
```
