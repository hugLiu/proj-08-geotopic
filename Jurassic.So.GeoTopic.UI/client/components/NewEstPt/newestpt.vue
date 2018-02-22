<template>
    <div class="newEstPt">
        <top :data="data" :title="title"
        :style="style" :hidden="hidden" :class="" :ischange="ischange"
        :onrefresh="refresh" :ontdclick="tdClick">
    </top>
</div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    import ajax from '../../utils/pollAjax'
    // import _ajax from '../../utils/coment'

    // var pram= {
    //     "filter": {},
    //     "fields": {},
    //     "sortrules": {
    //         "dc.date.value": {
    //             "direction": -1,
    //             "promotion": []
    //         }
    //     },
    //     "pager": {
    //         "from": 0,
    //         "size": 10
    //     }
    // };
    var count=1;
    //按照indexedDate排序
    var _param=
    {
        "sortrules": {
            "dc.date.value": {
                "direction": -1
            }
        },
        "pager": {
            "From": 0,
            "Size": 10
        }
    }
    _param = JSON.stringify(_param);
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
        "visible":""
    },
    {
        "name":"createddate",
        "title":"创建日期",
        "type":"",
        "key":false,
        "visible":""
    }
    ];

    export default {
        props: {
            id:{
                type:String,
                default:null
            }
        },
        data: function data() {
            return {
                data:{
                    "fields":fields,
                    "records":[]
                },
                ischange:0,
                hidden:false,
                title:"最新入库成果",
                //style:"width:410px;"
                style:""
            }
        },
        mounted: function () {
            this.getData();
        },
        methods: {
            //石亚茹Match方法
            getData:function () {
                count++;
                let _this = this;
                this.data.records=[];
                $.ajax({
                    url:"/Document/GetMatchData",
                    type:"post",
                    //async:false,
                    data:{param: _param},
                    success:function(result){
                        //var _result=JSON.parse(result).metadatas;
                        var _result=JSON.parse(result).metadatas;   //这是重油项目的
                        for (let i = 0; i < _result.length; i++) {
                            if (!_result[i]) return;
                            _this.data.records.push({
                                "id": _result[i].iiid,
                                "name":_result[i].dc.title[0].text?_result[i].dc.title[0].text:'',
                                "author":_result[i].dc.contributor?_result[i].dc.contributor[0].name:'',
                                "createddate":_result[i].indexeddate?_result[i].indexeddate.slice(0,10).replace(/-/g,'/'):""
                                //eval('new'+eval(_result[i].indexeddate).source).toLocaleDateString()
                            });
                        }
                        _this.ischange = count;   //watch
                    }
                });

           },
            //点击td某一项打开详情页面
            tdClick:function (url,data) {
               //var iiid=data.id; data为null
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
                this.data.records=[];
                this.getData();
            },
        },
        components:{
            top:soUI.top,
        }
    }
</script>
