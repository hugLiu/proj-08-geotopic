<template>
    <div>
        <so_linechart :data="param.data"
                      :maintitle="param.maintitle"
                      :subhead="param.subhead"
                      :showtool="param.showtool"
                      :showlabel="param.showlabel"
                      :style="param.style"
        >
        </so_linechart>
    </div>
</template>
<script>
    import soUI from '../../SoUI/soUI';
    export default{
        data(){
            debugger;
            return {

            }
        }
        ,
        props: {
            param: Object
        }
        ,
        components: {
            so_linechart: soUI.linechart
        }
    }
</script>
