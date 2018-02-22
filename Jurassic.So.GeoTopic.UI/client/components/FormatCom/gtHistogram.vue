<template>
    <div>
        <so_histogram :data="param.data"
                      :maintitle="param.maintitle"
                      :subhead="param.subhead"
                      :showtool="param.showtool"
                      :showlabel="param.showlabel"
                      :style="param.style"
        >
        </so_histogram>
    </div>
</template>
<script>
    import soUI from '../../SoUI/soUI';
    export default{
        props: {
            param: Object
        }
        ,
        components: {
            so_histogram: soUI.histogram
        }
    }
</script>