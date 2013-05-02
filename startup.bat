timeout 20

cd C:\Users\OpenVirtualWorlds\Documents\John\Chimera

git add .
git commit -m "Startup log push - %DATE% %TIME%"
git push
git pull

cd Bin
git add .
git commit -m "Startup log push - %DATE% %TIME%"
git pull

cd C:\Users\OpenVirtualWorlds\Desktop\Opensim-Timespan\
start "OpenSim" /MAX OpenSim.exe
cd C:\Users\OpenVirtualWorlds\Documents\John\Chimera\

timeout 60

launch.bat
