<template>
    <component :is="mo" :pram="prams" :heights="heights" :global="global"></component>
</template>

<script>
    import model from "../../utils/loadmodel"
    //默认路径还未添加
    var renderModel = {
        ptrender: require('../cellRender/GtPTRender.vue'),
        ptlistrender: require('../cellRender/GTPTListRender.vue'),
        urlrender: require('../cellRender/GtUrlRender.vue')
    };

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
                mo : null
            };
        },
        created:function(){
            //if(!this.$options.components) this.$components = {};
            //if(this.cellId){
                //let re = this.getPram(this.cellId);
                this.init();
            //}
        },
        // watch:{
        //     cellId: function () {
        //         if(this.cellId){
        //             let re = this.getPram(this.cellId);
        //             this.$options.components["render"] = renderModel[model.getConfig(re.url)];
        //         }
        //     }
        // },
        // beforeMount:function(){
        //     //alert(3);
        //     //model.load("mod1");
        //     debugger;
        //     // if(!this.$options.components) this.$components = {};
        //     // this.$options.components["ptrender"] = ptrender111;
        //     //this.$components["ptrender"] = ptrender;
        // },
        methods:{
           init:function(){
                // this.$options.components["render"] = {};
                // this.$options.components["render"] = renderModel[model.getConfig(this.prams.url)];
                this.mo = this.$options.components[model.getConfig(this.prams.url)];
           },
           reload:function(){
            this.mo= null;
            this.mo = this.$options.components[model.getConfig(this.prams.url)];
        }
        },
        watch:{
            prams: function () {
                this.init();
            },
            global: function () {
                this.reload();
            }
        },
        components: renderModel//{}
    }
</script>
