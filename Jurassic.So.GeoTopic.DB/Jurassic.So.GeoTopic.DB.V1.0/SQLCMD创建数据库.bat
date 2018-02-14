@ECHO OFF
SET ScriptPath=%~dp0
REM ECHO ¡°%ScriptPath%"
SQLCMD -U sa -P moss@55.cn -i "%ScriptPath%Jurassic.So.GeoTopic.publish.sql" -o "%ScriptPath%Jurassic.So.GeoTopic.publish.log"
pause
