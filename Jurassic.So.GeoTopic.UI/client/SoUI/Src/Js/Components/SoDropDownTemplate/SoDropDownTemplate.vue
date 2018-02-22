<template>
    <div class="dropdown">
        <a class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" role="button" aria-expanded="false">
            <span class="icon-reorder"></span>
        </a>
        <ul class="dropdown-menu" role="menu">
            <li role="presentation" v-for="(item, index) in itemlist">
                <a><input type="checkbox" v-model="item.visible" @click.stop /><span class="item-title">{{item.title}}</span></a>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        props: {
            itemlist: Array
        }
    }
</script>

<style scoped>
    a {
        cursor: pointer;
    }
    
    input {
        cursor: pointer;
    }
    
    .dropdown {
        left: 25px;
    }
    
    .dropdown-toggle {
        color: #898989;
    }
    
    .dropdown .icon-reorder {
        color: #898989;
        margin: 0;
    }
    
    .item-title {
        margin: 7px;
    }
</style>