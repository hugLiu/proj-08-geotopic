<template>
    <div class="newEstPt">
        <top  :data="data" :title="title"
              :style="style" :hidden="hidden" :class="" :ischange="ischange"
              :onrefresh="refresh" :ontdclick="tdClick">
        </top>
    </div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    var fields= [
            {
                "name":"name",
                "title":"名称",
                "type":"",
                "key":false,
                "visible":"",
                "url":"/Render/PTDetail?iiid=<#= id#>"
            },
            {
                "name":"author",
                "title":"作者",
                "type":"",
                "key":false,
                "visible":"",
            },
            {
                "name":"createddate", 
                "title":"浏览时间",
                "type":"", 
                "key":false, 
                "visible":""
            }
            ];
    var count=1;
    var title="最热浏览成果";
    export default {
        props: {
            id:{
                type:String,
                default:""
            },
            count: {
                type: Number,
                default: 10
            },
            style:String
        },
        data: function data() {
            return {
                data:{
                    "fields":fields,
                    "records":[]
                },
                ischange:0,
                hidden:false,
                title:"最热浏览成果",
                //style:"width:410px;"
            }
        },
        mounted: function () {
            this.getData();
        },
        methods: {
            //雷晶GetDownloadMessByUserId方法
            getData:function () {
                count++;
                let _this = this;
                let _count=this.count;
                let _type="look";
                this.data.records=[];
                $.ajax({
                    url:"/User/GetUserBehaviorGttop",
                    type:"post",
                    data:{count:_count,type:_type},
                    success:function(result){
                        for (let i = 0; i < result.length; i++) {
                            if (!result[i]) return;
                            _this.data.records.push({
                                "id": result[i].iiid,
                                "name": result[i].DC_Title_Text,
                                "author":result[i].DC_Author,
                                "createddate": eval('new ' + eval(result[i].CreatedDate).source).toLocaleDateString()
                            });
                        }
                        _this.ischange=count;   //watch
                    }
                });
            },
            //点击td某一项打开详情页面
            tdClick:function (url,data) {
                //var iiid=data.id;
                window.open(url);
                let i=url.indexOf("iiid="),
                    iiid=""
                if(i>-1){
                     iiid=url.substring(i+5)
                }
                if(iiid){
                $.ajax({
                    url: "/RECP/GetMateData",
                        type: "post",
                        data: { iiid: iiid },
                        success: function (result) {
                            var result = JSON.parse(result);
                            var _iiid=result.iiid;
                            var _name=result.dc?(result.dc.title?result.dc.title[0].text:""):"";
                            var _author=result.dc?(result.dc.contributor?result.dc.contributor[0].name:""):"";
                            var _producttype=result.ep?(result.ep.producttype?result.ep.producttype:""):"";
                            $.ajax({
                                url: "/User/OpenDetailPage",
                                type: "post",
                                data: { iiid: _iiid, name: _name, author: _author, producttype: _producttype },
                                success: function (result) {
                                    console.log(result);
                                },
                                error: function (result) {
                                    console.log(result);
                                }
                            });
                        },
                        error: function (result) {
                            console.log(result);
                        }
                })
                }
            },
            //刷新按钮
            refresh:function () {
                this.getData();
            },
        },
        components:{
            top:soUI.top,
        }
    }
</script>