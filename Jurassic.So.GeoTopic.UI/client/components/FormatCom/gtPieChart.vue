<template>
    <div>
        <so_piechart :maintitle="param.data.text"
                     :subhead="param.data.subtext"
                     :showtool="param.showtool"
                     :showlabel="param.showlabel"
                     :data="param.data.data"
                     :style="param.style"
        >
        </so_piechart>
    </div>
</template>
<script>
    import soUI from '../../SoUI/soUI';
    export default{

        props: {
            param:Object
        }
        ,
        components: {
            so_piechart: soUI.piechart
        }
    }
</script>