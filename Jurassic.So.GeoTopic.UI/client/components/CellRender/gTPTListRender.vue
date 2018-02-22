<template>
    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-body" style="height:100%">
            <template v-if="isready">
                <!--<gt_list :param="paramdata" :refresh="refresh"></gt_list>-->
                <component v-bind:is="currentptlist" :param="paramdata" :refresh="refresh"></component>
            </template>
        </div>
    </div>
</template>

<script>
    import models from '../../utils/getCom'
    import ao from "../../utils/arrayandobjectoperation"
    import matchfilter from "../../utils/matchfilter"

    export default {
        props: {
            id: String,
            pram: {
                type: Object,
                default: {}
            },
            global: {
                type: Object,
                default: null
            },
            useessearch: {
                type: Boolean,
                default: false
            },
            refresh: {
                type: Boolean,
                default: false
            }
        },
        data: function () {
            let ptlist= this.useessearch ?"gt_listes":"gt_list";
            return {
                paramdata:this.pram.param,
                isready:false,
                currentptlist:ptlist
            }
        },
        mounted: function () {
            this.paramdata.filter = this.goParam().filter;
            this.isready = true;
        },

        methods: {
            //转化参数，转换成match方法需要的kmd参数，并且替换全局变量。返回一个替换后的备份，不修改原始参数
            goParam: function () {
                return matchfilter.getFilter(this.pram.param.filter, this.global);
            }
        },
        computed: {
            paramdata: function () {
                return matchfilter.getFilter(this.pram.param.filter, this.global);
            }
        },
        watch: {
            // 'pram.param.filter.$and':function(){
            //  this.paramdata=this.goParam();
            // }
            'refresh': function () {
                this.paramdata.filter = this.goParam().filter;
            }
        },
        components: {
            gt_listes: models.listcomES ,
            gt_list: models.listcom
        }
    }

</script>
<style scoped>
    .text-overflow {
        width: 40%;
        /*    height:40%;
    border:1px solid red;
    border-top:4px solid red;
    padding:3px;*/
        overflow: hidden;
        ;
        /* 内容超出宽度时隐藏超出部分的内容 */
        text-overflow: ellipsis;
        /* 当对象内文本溢出时显示省略标记(...) ；需与overflow:hidden;一起使用。*/
        white-space: nowrap;
        /* 不换行 */
    }
</style>
