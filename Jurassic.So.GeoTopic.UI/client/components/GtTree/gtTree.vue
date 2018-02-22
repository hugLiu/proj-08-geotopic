<template>
    <tree :data="treeData" :onnodeselected="nodeClick" :class="classname" :style="style" :hidden="hidden"></tree>
</template>

<script>
    import soUI from '../../SoUI/soUI'
    export default {
        props: {
            id: String,
            classname: String,
            style: String,
            hidden: Boolean,
            userid: {
                type: String,
                default: null
            },
            topicindex: {
                type: Object,
                default: null
            },
            topiccode: {
                type: Array,
                default: []
            },
            topictitle: {
                type: Array,
                default: []
            },
            onclick: Function
        },
        data: function() {
            let param = {
                topicIndex: this.topicindex,
                topicCode: this.topiccode,
                topicTitle: this.topictitle
            };
            if (this.userid) param[userId] = this.userid;
            let reData = [];
            let _this = this;
            this.$http.post("/GeoTopic/GetTopics", param).then((result)=>{
               _this.treeData = result.data;
               _this.$children[0].data = result.data
            });
            // $.ajax({
            //     url: "/GeoTopic/GetTopics",
            //     data: param,
            //     type: "post",
            //     async: false,
            //     //cache: false,
            //     success: function(data) {
            //         reData = data;
            //     }
            // });
            return {
                treeData: reData
            }
        },
        methods: {
            nodeClick: function(node) {
                this.onclick(node.activeNode.id);
            }
        },
        components: {
            tree: soUI.tree
        }
    }
</script>
