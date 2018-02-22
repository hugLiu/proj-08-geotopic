<template>
    <div class="row">
        <!--只有1-4可以被整除-->
        <div v-bind:class="'col-md-' + 12/data.Cols.length" v-for="col in data.Cols">
            <!--if判断rows的长度，》0就递归，=0就显示模块。-->
            <rowrec v-if="col.Rows.length>0" v-for="row in col.Rows" :data="row" :prams="prams"
              :heights="getHeight(col.Rows.length)" :global="global">
            </rowrec>
            <!--ptrender就在这里调用，pram就是参数。div只是占位，可以直接替换成模块。-->
            <!--<div v-if="col.Rows.length==0">{{col.CellId}}</div>-->
            <!--style="width:480px; height:330px;"-->
            <div v-if="col.Rows.length==0">
            <div></div>
                <!-- <ptrender  :pram="col.CellId | getPram" :heights="heights"></ptrender> -->
                <render :prams="getPram(col.CellId)" :heights="heights" :global="global"></render>
            </div>
        </div>
    </div>
</template>

<script>
    //切换render的加载模块，一个是按模块加载，一个是按url加载。
    import render from "../cardRender/GtRenderMod.vue"
    // import render from "../cardRender/GtRenderUrl.vue"

    export default{
        props: {
            data: Object,
            prams: Array,
            global:{
                type: Object,
                default: null
            },
            heights:Number
        },
        methods:{
            getPram: function(id){
                for(let i=0;i<this.prams.length;i++){
                    if(id==this.prams[i].id){
                        return this.prams[i];
                    }
                }
            },

            getHeight: function(rownum){
                return this.heights/rownum;
            }
        },
        name:"rowrec",
        components: {"render":render}
    }
</script>
