cd Bin

Chimera.exe

IF %errorlevel% EQU -1073741819 (
GOTO exit
) ELSE ( 
IF %errorlevel% EQU 0 GOTO exit ELSE GOTO restart
)

GOTO :EOF

:restart
cd ..
launch.bat
GOTO :EOF

:exit
cd ..

cscript shutdown1.vbs
timeout 1
cscript shutdown2.vbs

git pull
git add Logs/*
git commit -m "Shutdown log push - %DATE% %TIME% "
git push

shutdown.exe /s /t 00

GOTO :EOF
