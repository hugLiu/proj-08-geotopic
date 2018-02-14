@ECHO OFF
SET SourcePath=C:\GeoTopic\GTAPI_Publish\SooilDataServiceApi_Debug
SET TargetPath=C:\GeoTopic\Publish\GTAPI
robocopy /E "%SourcePath%" "%TargetPath%"
robocopy /E "%SourcePath%" "%TargetPath%_AdapterA"
robocopy /E "%SourcePath%" "%TargetPath%_AdapterB"
robocopy /E "%SourcePath%" "%TargetPath%_AdapterC"
pause

