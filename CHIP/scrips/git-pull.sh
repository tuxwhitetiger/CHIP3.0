if ping -c 1 1.1.1.1 &> /dev/null
then
  git -C /home/tux/CHIP pull https://github.com/tuxwhitetiger/CHIP.git
  git -C /home/tux/CHIP3 pull https://github.com/tuxwhitetiger/CHIP3.0.git
else
  echo "no internet"
