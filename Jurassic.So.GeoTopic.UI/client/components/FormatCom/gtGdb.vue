<template>
    <div v-if="isready">
        <gdbshow :url="url" :style="param.style">
        </gdbshow>
    </div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    //import ajax from '../../utils/coment'
//    function getpath() {
//        //获取当前网址
//        var curWwwPath=window.document.location.href;
//        //获取主机地址之后的目录
//        var pathName=window.document.location.pathname;
//        var pos=curWwwPath.indexOf(pathName);
//        //获取主机地址
//        var localhostPaht=curWwwPath.substring(0,pos);
//        //获取带"/"的项目名
//        var projectName=pathName.substring(0,pathName.substr(1).indexOf('/')+1);
//
//        //获取项目路径
//        var realPath=localhostPaht+projectName;
//        return realPath;
//    }

    export default {
        props: {
            param:Object
        },
        data:function(){
            return{
                //url:this.param.url,
                url:"http://"+window.location.host+this.param.url,
                isready:false

            }
        },
        watch:{
            param: function () {
                this.url="http://"+window.location.host+this.param.url;
            }
        },
        mounted:function(){
             this.isready=true;
        },
    components: {
            gdbshow: soUI.gdb
        }
    }
</script>
