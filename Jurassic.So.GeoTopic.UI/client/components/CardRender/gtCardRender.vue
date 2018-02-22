<template>
<div>
<!--     <ul class="nav nav-tabs" role="tablist">
        <li role="presentation" v-bind:class="index ==0?'active':''"  v-for="(card ,index) in cardData">
            <a v-bind:href="'#'+card.Id" role="tab" data-toggle="tab">{{card.Title}}</a>
        </li>
    </ul> -->

    <!-- ps:这里效率又问题，这里的Tab不是异步加载，会一次加载所有Tab -->
<!--     <div class="tab-content">
        <div v-for="(card, index) in cardData" role="tabpanel" v-bind:class="index ==0?'tab-pane active':'tab-pane'" v-bind:id="card.Id">
            <tablerender  :data="card.Definition" :global="global"></tablerender>
        </div>
    </div> -->

    <template v-if="cardData.length>1">
        <el-tabs type="card" style="width:100%; height:100%;">
            <el-tab-pane :label="card.Title" v-for="(card, index) in cardData">
                <tablerender  :data="card.Definition" :global="global"></tablerender>
            </el-tab-pane>
        </el-tabs>
    </template>
    <template v-else>
        <div style="width:100%; height:100%;">
            <tablerender v-for="(card, index) in cardData" :data="card.Definition" :global="global"></tablerender>
        </div>
    </template>

</div>
</template>

<script>
    //var tablerender = require('../cardRender/GtTableRender.vue')
    import tablerender from '../cardRender/GtTableRender.vue'
    import {Tabs,TabPane} from 'element-ui'
    export default {
        //class,style,hidden几个属性并未使用，这里需求不明，因为PtRender并不是单个组件。
        props: {
            id: String,
            class:String,
            style:String,
            hidden:Boolean,
            global:{
                type: Object,
                default: null
            },
            topicid:String,
            //为了解决加载Loading的问题，这个只能解决当前卡片的布局加载load。里面的PtRender的loading还不行。
            onload:{
                type: Function,
                default: function(){}
            },
            refresh:Boolean
        },
        data: function(){
            if(this.topicid && this.topicid != "")
                this.getData();
            return {
                cardData: []
            }
        },
        updated:function(){
            this.onload();
        },
        methods:{
            getData: function(){
                 this.$http.get("/GeoTopic/GetCards?id="+this.topicid+"&ran="+Math.random()).then((result)=>{
                    for(let i=0;i < result.data.length; i++){
                      let dataDef = JSON.parse(result.data[i].Definition);
                        result.data[i].Definition = dataDef
                        for(let j=0;j < dataDef.cells.length; j++){
                            if(!dataDef.cells[j].param) continue
                            let cell = JSON.parse(dataDef.cells[j].param);
                                dataDef.cells[j].param = cell;
                        }
                    }
                    this.cardData = result.data;
                });
                // $.ajax({
                //     url: "/GeoTopic/GetCards?id="+this.topicid,
                //     type: "get",
                //     //async: false,
                //     datatype:"json",
                //     success: function (data) {
                //         for(let i=0;i < data.length; i++){
                //             data[i].Definition = JSON.parse(data[i].Definition);
                //             for(let j=0;j < data[i].Definition.cells.length; j++){
                //                 if(data[i].Definition.cells[j].param != "")
                //                     data[i].Definition.cells[j].param = JSON.parse(data[i].Definition.cells[j].param);
                //             }
                //         }
                //         this.cardData = data;
                //     }
                // });
            }
        },
        watch:{
            'topicid':function(){
                this.getData();
            },
            'refresh':function(){
                this.getData();
            }
        },
        // updated: function(){
        //     if(this.cardData.length > 0)
        //         // this.activeName = this.cardData[0].Id.toString();
        //     this.activeName ="23";
        //     alert(this.activeName);
        // },
        components: {
            tablerender:tablerender,
            "el-tabs":Tabs,
            "el-tab-pane":TabPane
        }
    }
</script>
