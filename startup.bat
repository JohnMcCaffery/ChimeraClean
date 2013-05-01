cd C:\Users\OpenVirtualWorlds\Documents\John\Chimera

git pull
git add Logs\*
git commit -m "Startup log push - %DATE% %TIME%"
git push

cd Bin
git pull

cd C:\Users\OpenVirtualWorlds\Desktop\Opensim-Timespan\
start "OpenSim" /MAX OpenSim.exe
cd C:\Users\OpenVirtualWorlds\Documents\John\Chimera\Bin\

timeout 60

Chimera.exe

cd ..

start cscript shutdown1.vbs
timeout 1
start cscript shutdown2.vbs
