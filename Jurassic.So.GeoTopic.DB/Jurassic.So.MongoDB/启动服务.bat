@ECHO OFF
cd /d "D:\Program Files\MongoDB\Server\3.2\bin"
REM mongod --dbpath=E:\Databases\MongoDB --logpath=E:\Databases\MongoDB\logs\mongodb.log --install
net start mongodb
pause

