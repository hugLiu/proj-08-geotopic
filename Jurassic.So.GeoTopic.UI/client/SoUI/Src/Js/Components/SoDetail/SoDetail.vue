<template>
    <div v-if="data &&data!=[]">
        <div class="detail" v-bind:style="style">
            <div class="basicInfo" style="clear: both;">
                <div  class="basicInfo_Pic" style="float: left; width: 50%;height: 100%;">
                    <!--<img v-if="data.thumbnailUrl&&data.thumbnailUrl!=''"-->
                         <!--:src="data.thumbnailUrl" alt="" style="border: solid 1px #DDDDDD;" v-on:click="showPic(data.id)">-->
                    <!--<img v-else :src="getIcon('thumbnail')" alt="#">-->
                    <slot name="mySlot"></slot>
                </div>

                <div class="basicInfo_data" style="float: right; width: 45%;height: 100%;">
                    <table class="table table-bordered" style="border-radius: 5px;">
                        <thead></thead>
                        <tbody>
                            <tr v-for="basicInfo in data.basicInfo">
                                <th style="width: 35%;">{{basicInfo.key}}</th>
                                <td style="width: 65%;">{{basicInfo.value}}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="groups" v-for="group in data.groups" style="clear: both" >
                <h4 style="margin-bottom: 10px;">{{group.name}}</h4>
                <table class="table table-bordered" v-if="group.items.length%2 == 0" style="width: 100%;">
                    <thead></thead>
                    <tbody>
                        <tr  v-for="n in Math.floor(group.items.length/2)">
                            <th style="width: 15%;">{{group.items[2*n-2].key}}</th>
                            <td style="width: 35%;">{{group.items[2*n-2].value}}</td>
                            <th style="width: 15%;">{{group.items[2*n-1].key}}</th>
                            <td style="width: 35%;">{{group.items[2*n-1].value}}</td>
                        </tr>
                    </tbody>
                </table>
                <table class="table table-bordered" v-if="group.items.length%2 != 0">
                    <thead></thead>
                    <tbody>
                        <tr v-for="n in Math.floor(group.items.length/2)" style="width: 100%;">
                            <th style="width: 15%;">{{group.items[2*n-2].key}}</th>
                            <td style="width: 35%;">{{group.items[2*n-2].value}}</td>
                            <th style="width: 15%;">{{group.items[2*n-1].key}}</th>
                            <td style="width: 35%;">{{group.items[2*n-1].value}}</td>
                        </tr>
                        <tr style="width: 100%;">
                            <th style="width: 15%;">{{group.items[group.items.length-1].key}}</th>
                            <td style="width: 35%;">{{group.items[group.items.length-1].value}}</td>
                            <th style="width: 15%;"></th>
                            <td style="width: 35%;"></td>
                        </tr>
                    </tbody>
                </table>
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
//            showTopic:function () {
//                alert('点击事件')
//                var iiid=this.data.id;
//                console.log(this.data);
//                alert(iiid);
//                if(this.id){
//                    this.onshowtopic && this.onshowtopic(gtModels.getReturnModel(this.id, iiid));
//                }
//                else
//                    this.onshowtopic && this.onshowtopic(iiid);
//            },
        }
    }
</script>