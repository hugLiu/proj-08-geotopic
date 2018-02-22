<template>
    <div class="top" v-bind:style="style">
        <div class="top_title">
            <h5 style="font-size: 16px;">{{title}}</h5>
            <!-- 刷新按钮 -->
            <!-- <span ><img @click="imgClick" :src="refresh | getIcon" alt="#"/></span> -->
        </div>
        <div class="top_content" v-if="!hidden" >
            <table class="table table-hover">
                <thead>
                <tr>
                    <th style="width: 13%;">排名</th>
                    <th v-for="f in topdata.fields" v-if="f.title=='名称'" style="width:45%">{{f.title}}</th>
                    <th v-for="f in topdata.fields" v-if="f.title!='名称'">{{f.title}}</th>
                </tr>
                </thead>
                <tbody>
                <tr v-if="data==null">
                    <td>没有任何数据</td>
                </tr>
                <tr v-for="(r,rindex) in topdata.records">
                    <td>
                        <span v-if="rindex<=2" class="ibx-hotTop-rank first">{{rindex+1}}</span>
                        <span v-else class="ibx-hotTop-rank bottom">{{rindex+1}}</span>
                    </td>
                    <td v-for="(f,findex) in topdata.fields">
                        <a @click="tdClick(urlContainer[findex][rindex])" v-if="f.url&&f.url!=''">{{r[f["name"]]}}</a>
                        <span v-else>{{r[f["name"]]}}</span>
                    </td>
                </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style scoped>
    body{
        font-family:"Microsoft YaHei";
        font-weight: normal;
        font-size: 10px;
    }
    .top .top_title{
        background-color: white;
        border-color: #e7eaec;
        border-image: none;
        border-style: solid solid none;
        border-width: 4px 0px 0;
        color: inherit;
        margin-bottom: 0;
        padding: 14px 15px 7px;
        min-height: 52px;
        border-bottom: solid 1px #E7EAEC;
    }
    .top .top_title h5{
        font-weight: 600;
        display: inline-block;
        font-size: 16px;
        margin: 0 0 11px;
        padding: 0;
        text-overflow: ellipsis;
        float: left;
    }
    .top .top_title a{
        display: block;
        margin-bottom: 5px;
    }
    .top .top_title img{
        display: block;
        float: right;
        text-align: center;
        margin: 0 auto;
        margin-right: 50px;
        margin-top:-10px;
    }
    .top .top_content{
        padding: 10px;
        clear: both;
        background-color: white;
    }
    thead{
        vertical-align: middle;
    }
    table thead tr th{
        line-height: 1.42857;
        padding: 10px;
        vertical-align: middle;
        border-bottom: 2px solid #ddd;
        font-weight: bold;
    }
    table tbody{
        vertical-align: middle;
    }
    table tbody tr td{
        border-top: 1px solid #e7eaec;
        line-height: 1.42857;
        padding: 10px;
        vertical-align: middle;
    }
    .ibx-hotTop-rank {
        float: left;
        width: 16px;
        height: 16px;
        line-height: 16px;
        color: #fff;
        text-align: center;
        margin: 10px 10px;
        margin-left: 0;
    }
    tbody td .first {
        background-color: #f44528;
    }
    tbody td .bottom{
        background-color: #ff943e;
    }
    a :hover{
        cursor: pointer;
    }
</style>
<script>
    import   analysis from "../../ComonJs/formatAnalysis"
    import   getIcon from  "../../ComonJs/formatIcon"
    export default {
        props: {
            id:{
                type:String,
                default:""
            },
            data: Object,
            title: String,
            class:{
                type:String,
                default:""
            },
            style:{
                type:String,
                default:""
            },
            hidden:{
                type:Boolean,
                default:false
            },
            ischange:Number,
            ontdclick:Function,
            onrefresh:Function
        },
        data:function () {
            return{
                urlContainer:[],    //放置所有格式化之后的url
                refresh:"refresh",
                topdata:''
            }
        },
//        mounted:function () {
//            //如何只有一行数据可以跳转的时候
//            //this.setData();
//        },
        methods: {
            setData:function () {
                this.topdata=this.data;
                if(this.data&&this.data!=null) {
                    if(this.data.fields&&this.data.fields!=[]){
                        var field=this.data.fields;
                        for(var i=0;i<field.length;i++){
                            if(field[i].url&&field[i].url!="") {
                                var record = [];
                                for (var j = 0; j < this.data.records.length; j++) {
                                    var key = [{"field": "id"}]
                                    var value = this.data.records[j]
                                    var test = analysis(key, value, field[i].url);
                                    record[j] = test;
                                    this.urlContainer.push(record);
                                }
                                record = [];
                            }
                        }
                    }
                }
            },
            tdClick:function (url) {
                this.ontdclick&&this.ontdclick(url);
            },
            imgClick: function () {
                this.onrefresh&&this.onrefresh();
            }
        },
        filters:{
            getIcon: function (str) {
                return getIcon(str);
            }
        },
        watch:{
            ischange:function () {
                this.setData();
                this.ischange=0;
            }
        }
    }
</script>
