sudo docker stop chainbox_gui_rvk

sudo docker rm chainbox_gui_rvk

echo $1

sudo docker run --name chainbox_gui_rvk \
  --restart always \
  -p 8005:8000 \
  -v /home/dselivanov/chainbox-gui-rvk:/usr/src/app \
  -v /home/dselivanov:/home/dselivanov \
  -d chainbox_gui_rvk
