@ECHO OFF
ECHO ����
SET CompileProgram=D:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe
SET Solution=%~dp0..\..\GeoTopic1.0.sln
SET Project=SubmissionTool.csproj
SET CompileDir=%~dp0bin
SET CompileLog=%~dp0bin\Compile.Log
"%CompileProgram%" "%Solution%" /Build Release /Out "%CompileLog%" /Log

ECHO ���
SET ZipProgram=D:\Program Files\7-Zip\7z.exe
SET ZipFile=%~dp0bin\Excel�����ύ����.7z
SET ZipPath=%~dp0bin\Release
DEL /Q "%ZipFile%"
"%ZipProgram%" a "%ZipFile%" "%ZipPath%\*"
ECHO ���
pause
