@ECHO OFF
SET CurrentDir=%~dp0
SET SourceDir=%CurrentDir%..\bin
SET TargetDir=%CurrentDir%..\..\..\..\��Ŀģ��\Jurassic.So.GeoTemplate\lib

robocopy "%SourceDir%" "%TargetDir%"

pause
