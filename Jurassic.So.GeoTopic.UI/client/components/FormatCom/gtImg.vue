<template>
   <soimage  :url="param.url"  :styleobj="styleObj"></soimage>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    export default {
        props: {
            param:Object
        },
       data:function () {
          return{
             styleObj:{
                "height":this.param.heights/1.5+"px"
             }
          }
       },
        components: {
            "soimage": soUI.dlimg
        }
    }
</script>
