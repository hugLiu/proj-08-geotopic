@ECHO OFF
SET CurrentDir=%~dp0
ECHO ���Ƶ�GtWeb
robocopy %CurrentDir%..\%1 %CurrentDir%..\..\Jurassic.So.GeoTopic\Jurassic.So.GeoTopic.Web\Content\gtui *.* /S /XF *.json

ECHO ���Ƶ���Ŀ_Эͬ
robocopy %CurrentDir%..\%1 %CurrentDir%..\..\..\..\..\..\..\03��Ŀ\01����Эͬ�о�����\������\04����\Դ����\��ĿԴ��\Jurassic.Share\Jurassic.Share.Web\Content\gtui *.* /S /XF *.json
exit 0
