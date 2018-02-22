@ECHO OFF
SET CurrentDir=%~dp0
ECHO 复制到GtWeb
robocopy %CurrentDir%..\%1 %CurrentDir%..\..\Jurassic.So.GeoTopic\Jurassic.So.GeoTopic.Web\Content\gtui *.* /S /XF *.json

ECHO 复制到项目_协同
robocopy %CurrentDir%..\%1 %CurrentDir%..\..\..\..\..\..\..\03项目\01深圳协同研究环境\开发库\04编码\源代码\项目源码\Jurassic.Share\Jurassic.Share.Web\Content\gtui *.* /S /XF *.json
exit 0
