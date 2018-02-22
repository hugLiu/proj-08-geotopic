<template>
    <object id="joid" align="left" classid="clsid:D2546426-13FD-4018-85F1-F5BD61147C8F" style="width:98%;"
            codebase="JoGIS4.ocx#version=1,0,0,1" wmode="Opaque" VIEWASTEXT>
        <PARAM NAME="_Version" VALUE="65536"/>
        <PARAM NAME="_ExtentX" VALUE="20000"/>
        <PARAM NAME="_ExtentY" VALUE="15000"/>
        <PARAM NAME="_StockProps" VALUE="0"/>
        <PARAM NAME="WMODE" VALUE="transparent"/>
    </object>
   <!-- <hr>
   <input type="button" @click="df" value="change">-->
</template>
<script>
    export default{
        props:{
            url:String,
            style:String,

        },
        mounted: function () {
            if(this.url=="undefined" || this.url==""){
                return;
            }
            //http://localhost:4097/FileInfo/GetStream?url=ADP://SQL适配器多域测试/SQL适配器域1/SUQ9MTIzODU=&ticket=SQL适配器域1___SUQ9MTIzODU=___12396&format=gdb
             //encodeURI
              try{
                this.$el.LoadMapFile(this.url);
                this.createContextMenu();
              }
              catch(err){
               console.log(err)
              }
           
        },
        watch:{
            url: function () {
                this.$el.LoadMapFile(this.url);
                this.createContextMenu();
            }
        },
        methods:{
            createContextMenu:function () {
                try{
                    this.$el.ResetTools();
                    this.$el.AddTool(13);//放大
                    this.$el.AddTool(14);//缩小
                    this.$el.AddTool(15);//移动/手势
                    this.$el.AddTool(17);//复位
                }
                catch(err){
                     console.log(err)
                }

                // 定制自定义菜单测试
                /*joid.AddCustomTool("数据统计", 0);//菜单开始
                joid.AddCustomTool("年度储量统计", 40);
                joid.AddCustomTool("年度产量统计",41);
                joid.AddCustomTool("油气田产量统计", 42);
                joid.AddCustomTool("", 0);//菜单结束
                joid.AddCustomTool("相关成果", 0);//菜单开始
                joid.AddCustomTool("成果目录", 43);
                joid.AddCustomTool("成果检索", 44);
                joid.AddCustomTool("", 0);//菜单结束
                joid.AddCustomTool("地质特征", 0);//菜单开始
                joid.AddCustomTool("烃源岩特征", 45);
                joid.AddCustomTool("储层特征", 46);
                joid.AddCustomTool("储盖特征", 47);
                joid.AddCustomTool("油气藏特征", 48);
                joid.AddCustomTool("", 0);//菜单结束*/
            },
            /*df:function () {
                joid.ShowEagleEye(1);
            },*/
        }
    }
</script>
