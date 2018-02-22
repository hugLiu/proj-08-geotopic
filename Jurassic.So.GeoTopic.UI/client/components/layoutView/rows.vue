<template>
    <div>
        <div style="display:inline-block" v-for="col in data.Cols">
            <rowrec v-if="col.Rows.length>0" v-for="row in col.Rows" :data="row" :size="size" :hc="hc">
            </rowrec>
            <template v-if="col.Rows.length==0">
                <div style="width:{{ (size-(data.Cols.length-1)*20)/data.Cols.length-6 }}px;height: {{ (size-(data.Cols.length-1)*20)/hc-6 }}px;line-height: {{ size/hc-12 }}px;"  class="layoutviewdiv">
                    {{col.CellId}}
                </div>
            </template>
        </div>
    </div>

</template>

<style scoped>
    .layoutviewdiv {
        /*padding:10px; width:50px; height:50px;*/
        /*border: 1px solid #899199;*/
        /*-moz-border-radius: 15px;*/
        /*-webkit-border-radius: 15px;*/
        /*border-radius:15px;*/

        /*height: 50px;*/
        /*width: 50px;*/
        /*background-color: #5b5d5f;*/

        text-align:center;
        /*line-height: 80px;*/
        margin:3px 3px 3px 3px;
        /*width:100px;*/
        /*height: 100px;*/
        display: inline-block;
        padding: 5px 10px 6px;
        text-decoration: none;

        border-radius: 5px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;

        -moz-box-shadow: 0 1px 2px rgba(0,0,0,0.5);
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,0.5);
        text-shadow: 0 -1px 1px rgba(0,0,0,0.25);
        border-bottom: 1px solid rgba(0,0,0,0.25);
        position: relative;
        cursor: pointer;
    }
</style>

<script>
    export default{
        props: {
            data: Object,
            size: Number,
            hc: Number
        },
        data:function(){
            return {
//                w: this.size,
//                h: this.size,
//                hc: 4,
//                lc:4,
//                c:3
            };
        },
        name: "rowrec"
    }
</script>
