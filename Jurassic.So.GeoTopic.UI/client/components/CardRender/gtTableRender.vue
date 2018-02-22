<template>
  <div>
      <rowrender v-for="row in data.layout.Rows" :prams="data.cells"  :data="row"
        :heights="heights" :global="global"></rowrender>
  </div>
</template>

<script>
    import  rowrender from '../cardRender/gtRowRender.vue'
    export default {
        props: {
            data: Object,
            global: {
                type: Object,
                default: null
            }
        },
        data:function(){
            //计算外层格子高度= 页面正文总高度-70px   /行数
               var heights=(window.screen.availHeight-300)/this.data.layout.Rows.length;
               return {
                   heights:heights
               }
        },
        components:
        {
            rowrender:rowrender
        }
    }
</script>
