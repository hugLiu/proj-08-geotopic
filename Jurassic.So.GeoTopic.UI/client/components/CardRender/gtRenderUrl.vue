<!-- 直接调用url（即render控制器）的渲染方式，有一个样式问题，弹出窗会在iframe中打开而不是全屏打开 -->

<!-- 其他三个模块对布局和渲染负责，为了将逻辑拆分所以有了这个模块。这个模块将负责所有的组件调用的逻辑。 -->
<template>
<!--     <div class="embed-responsive embed-responsive-16by9">
        <iframe class="embed-responsive-item" style="'width:100%;height:100%;" :src="url"></iframe>
    </div> -->
    <iframe frameborder="no" border="0" marginwidth="0" marginheight="0" scrolling="no" width="100%" :height="heights + 120 +'px'" :src="url"></iframe>
</template>

<script>
    export default{
        props: {
            prams: Object,
            global:{
                type: Object,
                default: null
            },
            heights:Number
        },
        data:function(){
            return {
                url:""
            };
        },
        created:function(){
            this.init();
        },
        methods:{
           init:function(){
                //后台参数配置中现在确实请求类型的配置,Render控制器中的方法全部是get，第三方的访问也一般是get。
                //这么处理后，模块加载的参数配置的第一层其实没什么意义了。并且也不存在这一层的UrlRender类型。
                //并且全局变量没有传递过去。
                if(this.prams.param && this.prams.param.filter){
                    this.url = this.prams.url + "?param="+JSON.stringify(this.prams)+"&global="+JSON.stringify(this.global)+"&heights="+this.heights;
                }
           }
        },
        watch:{
            prams: function () {
                this.init();
            }
        }
    }
</script>
