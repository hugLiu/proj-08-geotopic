<template>
    <div v-if="!hidden">
        <div class="embed-responsive embed-responsive-16by9" v-if="url!=''" v-bind:style="stylestr">
            <iframe class="embed-responsive-item" v-bind:src="url"></iframe>
        </div>
        <div v-else>
            <div v-html="htmltext" v-bind:style="stylestr"></div>
        </div>
    </div>
</template>
<script>
    export default {
        props: {
            id: {
                type: String,
                default: null
            },
            htmltext: String,
            url: {
                type: String,
                default: ""
            },
            stylestr: String,
            class: String,
            hidden: {
                type: Boolean,
                default: false
            }
        }
    };
</script>