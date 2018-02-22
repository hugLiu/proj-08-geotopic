<template>
    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-heading">
            <h5 style="position:relative;">
            {{pram.title}}
            </h5>
        </div>
        <div class="panel-body" style="height:100%">
            <div class="embed-responsive embed-responsive-16by9">
                <iframe class="embed-responsive-item" :height="heights+'px'" :src="iframeUrl"></iframe>
            </div>
        </div>

    </div>
</template>

<script>
    //一般默认的都不在做下级分发，所以这里直接当成外部链接处理，不在调用模块。
    export default {
        props: {
            pram: {
                type: Object,
                default: {}
            },
            global: {
                type: Object,
                default: null
            },
            heights: {
                type: Number
            }
        },
        data: function(){
            return {}
        },
        computed:{
            iframeUrl:function(){
               return this.pram.url+"?param="+ JSON.stringify(this.pram.param)+"&global="+JSON.stringify(this.global)
            }
        }
    }
</script>
