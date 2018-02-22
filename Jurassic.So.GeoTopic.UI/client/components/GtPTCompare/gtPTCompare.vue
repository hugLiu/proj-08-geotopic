<template>
    <div class="compare">
        <el-row v-for="r in rownum">
            <el-col :span="Math.floor(20/colsnum)" v-for="(c, index) in colsnum" :offset="index > 0 ? 1 : 0">
                <el-card :body-style="{ padding: '0px'}" v-if="haveElse(getIndex(r,c))">
                 <div slot="header" class="clearfix" v-if="showheader">
                     <span style="line-height: 36px"> {{ comparefilter[getIndex(r,c)-1]}} </span>
                 </div>
                 <!-- <ptrender :pram="filter"></ptrender>  -->
                 <ptrender :pram="getFilter(getIndex(r,c))" :istool="istool" :isremarks="isremark"
                 :isdown="isdown" :isview="isview" :issubmit="issubmit" :isfoottool="isfoottool"></ptrender>
             </el-card>
         </el-col>
     </el-row>
 </div>
</template>

<script>
    import ptrender from "../CellRender/GtPTRender.vue"
    import {Card,Col,Row} from 'element-ui'
    import helper from "../../utils/ArrayAndObjectOperation"
    export default{
        props: {
            publicfilter:{
                type:Object,
                required: true
            },
            comparekey:String,
            comparefilter:Array,
            colsnum: {
                type:Number,
                default:2
            },
            showheader:{
                type:Boolean,
                default:false
            },
            istool: {
                type: Boolean,
                default: true
            },
            isremarks: {
                type: Boolean,
                default: true
            },
            isdown: {
                type: Boolean,
                default: true
            },
            isview: {
                type: Boolean,
                default: true
            },
            issubmit: {
                type: Boolean,
                default: true
            },
            isfoottool: {
                type: Boolean,
                default: true
            }
        },
        data: function(){
            return {
                commonparam:this.publicfilter.param.filter
            };
        },
        computed:{
            rownum:function(){
               if(!this.comparefilter||this.comparefilter.length==0) {
                return 0;
            }
            return Math.ceil(this.comparefilter.length/this.colsnum);
        }
    },
    methods:{
        haveElse: function(index){
            return index <= this.comparefilter.length;
        },
        getFilter:function(index){
            if(!this.comparefilter||this.comparefilter.length==0) {
                return this.publicfilter;
            };
            let comparevalue=this.comparefilter[index-1];
            let compareparam= {[this.comparekey]:comparevalue};
            let combineparam={"$and":[ this.commonparam, compareparam]};
            let tempfilter=helper.deepClone(this.publicfilter);
            tempfilter.param.filter=combineparam;
            return  tempfilter;
        },
        getIndex:function(rowindex,colindex){
            return (rowindex-1)*this.colsnum+colindex;
        }
    },
    components: {
        "ptrender":ptrender,
        "el-row":Row,
        "el-col":Col,
        "el-card":Card
    }
}
</script>
