<template>
            <div>
                <richtext :url="param.url"  :stylestr="styleStr">
                </richtext>
            </div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    export default {
        props: {
            param: Object,
        },
        data:function () {
            return{
                styleStr:"height:"+(this.param.heights-15)+"px"
            }
        },
        components: {
            richtext: soUI.richtext
        }
    }
</script>