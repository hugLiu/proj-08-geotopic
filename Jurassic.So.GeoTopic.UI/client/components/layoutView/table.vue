<template>
        <rowss v-for="row in data.Rows"  :data="row" :size="size" :hc="data.Rows.length"></rowss>
</template>

<script>
    import  rows from '../layoutView/rows.vue'
    export default {
        props: {
            data: Object,
            size: Number
        },
        components: {
            rowss:rows
        }
    }
</script>