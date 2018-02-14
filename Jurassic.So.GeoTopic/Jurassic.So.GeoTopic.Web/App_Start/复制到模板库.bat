@ECHO OFF
SET CurrentDir=%~dp0
SET SourceDir=%CurrentDir%..\bin
SET TargetDir=%CurrentDir%..\..\..\..\ÏîÄ¿Ä£°å\Jurassic.So.GeoTemplate\lib

robocopy "%SourceDir%" "%TargetDir%"

pause
