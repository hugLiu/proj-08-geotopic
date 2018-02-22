<template>
    <div class="newEstPt">
        <top  :data="data" :title="title"
              :style="style" :hidden="hidden" :class="" :ischange="ischange"
              :onRefresh="refresh" :onTdClick="tdClick">
        </top>
    </div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    //import ajax from '../../utils/pollAjax';
    // import _ajax from '../../utils/coment'
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
                "title":"创建日期",
                "type":"",
                "key":false,
                "visible":""
            }
            ];

    var count=1;
    export default {
        props: {
            id:{
                type:String,
                default:""
            },
            count: {
                type: Number,
                default: 10
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
                title:"成果收藏组件",
                style:"width:410px;"
            }
        },
        mounted: function () {
            this.getData();
        },
        methods: {
            //雷晶GetFavoriteMessByUserId方法
            getData:function () {
                count++;
                let _this = this;
                let _count=this.count;
                this.data.records=[];
                $.ajax({
                    url:"/UserBehavior/GetFavoriteMessByUserId",
                    type:"get",
                    async:false,
                    data:{count:_count},
                    success:function(result){
                        for (let i = 0; i < result.length; i++) {
                            if (!result[i]) return;
                            _this.data.records.push({
                                "id": result[i].Type,
                                "name": result[i].Title,
                                "author":result[i].CreatedBy,
                                "createddate":eval('new ' + eval(result[i].CreatedDate).source).toLocaleDateString()
                            });
                        }
                    }
                });
                _this.ischange=count;   //watch
            },
            //点击td某一项打开详情页面
            tdClick:function (url) {
                window.open("/GeoTopic/Detail?iiid=" + url);
            },
            //刷新按钮
            refresh:function () {
                this.data.records = [];
                this.getData();
            },
        },
        components:{
            top:soUI.top,
        }
    }
</script>
