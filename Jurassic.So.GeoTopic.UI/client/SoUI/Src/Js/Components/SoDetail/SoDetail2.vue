<template>
    <div v-if="data &&data!=[]">
        <div class="detail" v-bind:style="style">
            <div class="basicInfo" style="clear: both;">
                <div  class="basicInfo_Pic" style="float: left; width: 75%;height: 100%;"> 
                    <slot name="mySlot"></slot>
                </div> 
                <div style="float: right; width: 24%;height: 100%; margin-left:1%; margin-top: 2px;">
                    <div class="basicInfo_data">
                        <table class="table table-bordered" style="border-radius: 5px;">
                            <thead></thead>
                            <tbody>
                                <tr v-for="basicInfo in data.basicInfo" v-if="basicInfo.value&&basicInfo.key!='标题'&&basicInfo.key!='数据源名称'&&basicInfo.key!='摘要'">
                                    <th style="width: 35%;">{{basicInfo.key}}</th>
                                    <td style="width: 65%;">{{basicInfo.value}}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div>
                        <div class="groups" v-for="group in data.groups">
                            <table class="table table-bordered" style="width: 100%;">
                                <thead></thead>
                                <tbody>
                                    <tr  v-for="n in group.items.length" v-if="group.items[n-1].value&&group.items[n-1].key!='标题'&&group.items[n-1].key!='类型'&&group.items[n-1].key!='作者'&&group.items[n-1].key!='摘要'">
                                        <th style="width: 35%;">{{group.items[n-1].key}}</th>
                                        <td style="width: 65%;">{{group.items[n-1].value}}</td> 
                                    </tr>
                                </tbody>
                            </table> 
                        </div>
                    </div>
                </div>
            </div> 
        </div>
    </div>
</template>
<style scoped>
    th{
        background-color: #f5f5f5;
    }
    .groups th{width: 10%;}
    .groups td{width:40%;}
    .table { 
        margin-bottom: 0;
    }
</style>
<script>
    import GetIcon from  "../../ComonJs/formatIcon"
    export default {
        props: {
            id:String,
            data: Object,
            style:{
                type:String,
                default:""
            },
            ischange:Number,
            showpreview: {
                type:Boolean,
                default:true
            },
            showdownload:{
                type:Boolean,
                default:true
            },
            onshowtopic:Function
        },
        data:function () {
          return{
              defaultPhoto:"",
              topdata:null
          }
        },
        methods: {
            getIcon: function (str) {
                return GetIcon(str);
            }
        },
    }
</script>