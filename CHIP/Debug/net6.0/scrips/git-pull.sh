if ping -c 1 1.1.1.1 &> /dev/null
then
  git -C /home/tux/CHIP pull https://github.com/tuxwhitetiger/CHIP.git
  git -C /home/tux/CHIP3/CHIP3.0 pull https://github.com/tuxwhitetiger/CHIP3.0.git
else
  echo "no internet"


cd /home/tux/CHIP3/CHIP
sudo mcs -out:bob.exe *.cs /r:RGBLedMatrix.dll


