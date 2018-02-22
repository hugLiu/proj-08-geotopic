@ECHO OFF
REM ECHO %~dp0
cd /d "%~dp0"
rd Content
mklink /d "Content" "..\..\..\Jurassic.So.GeoTopic.Services\GTAPI\Content"
rd Scripts
mklink /d "Scripts" "..\..\..\Jurassic.So.GeoTopic.Services\GTAPI\Scripts"
pause
