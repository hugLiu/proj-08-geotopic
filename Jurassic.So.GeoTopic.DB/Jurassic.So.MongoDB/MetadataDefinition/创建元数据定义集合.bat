@ECHO OFF
SET jsonPath=E:\Projects\����ҵ��\01������\02Դ����\Jurassic.PKS.Service\Jurassic.PKS.Service
SET jsonFile=MetadataDefinition.json
cd /d "D:\Program Files\MongoDB\Server\3.2\bin"
REM mongoimport /host:192.168.1.152 /port:27017 /db:IBaseDemo /collection:MetadataDefinition /drop 

REM mongoimport /host:192.168.1.152 /port:27017 /db:IBaseDemo /collection:MetadataDefinition /upsert /upsertFields:name /file:%jsonPath%%jsonFile% /jsonArray
mongoimport /host:localhost /port:27017 /db:IBaseDemo /collection:MetadataDefinition /upsert /upsertFields:name /file:%jsonPath%\%jsonFile% /jsonArray
REM mongoimport /host:localhost /port:27017 /db:IBaseDemo /collection:MetadataDefinition /drop /file:%jsonPath%%jsonFile% /jsonArray
pause
