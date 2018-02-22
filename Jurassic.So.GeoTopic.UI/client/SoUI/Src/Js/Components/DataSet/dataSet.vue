<template>
    <div class="dataset">
        <ul class="nav nav-tabs" id="tab">
            <li :class="$index==0?'active':''" v-for="list in datasource">
                <a data-toggle="tab" :href='("#tab_"+$index)'>{{list.name}}</a>
            </li>
        </ul>
        <br />
        <div class="tab-content">
            <div :id='("tab_"+$index)' :class='$index==0?"tab-pane active":"tab-pane"' v-for="list in datasource">
                <div :id='("datatable_"+$index)'>
                    <grid :totalcount="list.totalcount" :columns="list.column" :datasource="list.data" :ongetdata="onGetData"></grid>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import grid from '../../components/Grid/grid.vue';
    export default{
        props:{
            datasource:Array,
            ongetdata:Function
        },
        components:{
            'grid':grid
        },
        created:function(){
            if(this.ongetdata){
                this.datasource=this.ongetdata(1)
            }
        }
    }
</script>

<style scoped>
</style>
