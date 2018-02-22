<template>
    <div class="associatebo" style="width: 300px">
        <el-popover ref="popoverset" placement="top-end" trigger="click">
            <div class="popovercontent">
                <div>目标对象：
                    <el-radio-group v-model="selectBO" v-for="bo in bos" size="small">
                        <el-radio :label="bo.value">{{bo.value}}</el-radio>
                    </el-radio-group>
                </div>
            </div>
        </el-popover>
        <el-card :body-style="{ padding: '0px'}"  v-if="show">
            <div class="header">
                <span style="font-size: 15px"> {{name}} </span>
                <el-button v-popover:popoverset style="float:right;background-color:#d3dce6" type="primary" size="mini" :plain="true" icon="setting">设置</el-button>
            </div>
            <div class="content">
                <el-tree :data="botree" :props="defaultProps" :node-key="id" accordion @node-click="handleNodeClick"></el-tree>
            </div>
        </el-card>

    </div>
</template>

<script>
    import ajax from '../../utils/pollAjax'
    import listToTree from "../../utils/listToTree"
    export default {
        props: {
            id: String,
            name: String,
            bos:  { type: Array, default: function () { return [] } },
            botreetemplate: Array,
            botree: {
                type: Array,
                default: function () { return [] }
            },
            defaultProps: {
                type: Object,
                default: function () {
                    return {
                        children: 'children',
                        label: 'text'
                    }
                }
            },
            // show: { type: String, default: 'All' }, //P：上级 C：下级 B：同级 All：所有
            onclick: Function
        },
        data: function () {
            let select = "";
            let isshow=false;
            if (this.bos.length) {
                select = this.bos[0].value
                isshow=true;
            }
            return {
                selectBO:select,
                show:isshow
            };
        },
        mounted: function () {
            this.getBOTree();
        },
        methods: {
            getBOType: function () {
                for (var i = 0; i < this.bos.length; i++) {
                    if (this.bos[i].value == this.selectBO)
                        return this.bos[i].type;
                }
            },
            handleNodeClick: function (bo) {
                this.onclick(bo.text);
            },
            getBOTree: function () {
                let _this = this;
                let botype = this.getBOType();
                let param = botreetemplate;
                let defaultUrl = "/BOService/GetBOTree";
                ajax({
                    url: defaultUrl,
                    data: JSON.stringify(param),
                    contentType: "application/json",
                    type: "post",
                    cache: false,
                    dataType: 'json',
                    success: function (result) {
                        let nodes = [];
                        for (let i = 0; i < result.length; i++) {
                            let bo = { "parentId": result[i].pid, "id": result[i].boid, "name": result[i].name }
                            nodes.push(bo);
                        }
                        _this.botree = listToTree(nodes);
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
        },
        watch: {
            selectBO: function () {
                this.getBOTree();
            }
        }
    }
</script>

<style scoped>
    .associatebo a {
        padding: 10px;
    }

    .associatebo .header {
        padding: 5px;
        background-color: #d3dce6;
        height: 30px;
        border-bottom: 1px solid lightgrey
    }
</style>
