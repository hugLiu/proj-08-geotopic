<template>
    <div :id="defaultId"   :style="style" v-show="!hidden && data != null && data.length" class="tree-default-style"  ></div>
    <!--<div id="{{defaultId}}" :class="class" :style="style" v-show="!hidden && data != null && data.length" class="tree-default-style" selection="{{selection}}"></div> -->
</template>

<script>
    export default {
        props: {
            id: {
                type: String,
                default: null
            },
            class: String,
            style: String,
            hidden: {
                type: Boolean,
                default: false
            },
            data: Array,
            showcheckbox: {
                type: Boolean,
                default: false
            },
            enableedit: {
                type: Boolean,
                default: false
            },
            onloaded: Function,
            onnodeselected: Function,
            onadd: Function,
            onupdate: Function,
            ondelete: Function
        },
        data() {
            return {
                defaultId: "tree-" + Date.now(),
                selection: ""
            }
        },
        watch: {
            data: function() {
                this.loadData()
            }
        },
        methods: {
            loadData: function() {
                if (this.data && this.data.length > 0) {
                    try {
                        this.fixTreeData() //fix jstree data setting bug
                    } catch (e) {}
                    this.treeInit()
                }
            },
            fixTreeData: function() {
                for (var i = 0; i < this.data.length; i++) {
                    if (!(("id" in this.data[i]) && ("parentId" in this.data[i]))) { //rarely case but prevent error result, source data not include id, parentId then delete this
                        this.data.splice(i, 1)
                        i--
                    }
                }
                for (var i = 0; i < this.data.length; i++) {
                    var parentId = this.data[i].parentId
                    if (isNaN(parentId)) {
                        parentId = parentId.replace(/ /g, "")
                    }
                    var id = parentId == "" || parentId == null || parentId == "#" ? "#" : this.checkParentId(parentId) //set root node "#"
                    delete this.data[i].parentId;
                    this.data[i].parent = id //parentId->parent
                }
            },
            checkParentId: function(pId) {
                for (var j = 0; j < this.data.length; j++) {
                    var id = this.data[j].id
                    if (isNaN(id)) {
                        id = id.replace(/ /g, "")
                    }
                    if (pId == id) {
                        return pId
                    }
                }
                return "#"
            },
            treeInit: function() {
                var plugins = ['types', 'dnd']
                if (this.showcheckbox) plugins.push("checkbox")
                $('#' + this.defaultId).jstree({
                    'plugins': plugins,
                    'types': {
                        'default': {
                            'icon': 'fa fa-folder'
                        },
                        'html': {
                            'icon': 'fa fa-file-code-o'
                        },
                        'svg': {
                            'icon': 'fa fa-file-picture-o'
                        },
                        'css': {
                            'icon': 'fa fa-file-code-o'
                        },
                        'img': {
                            'icon': 'fa fa-file-image-o'
                        },
                        'js': {
                            'icon': 'fa fa-file-text-o'
                        }
                    },
                    'core': {
                        "multiple": this.showcheckbox ? true : false, // multiple select
                        "animation": true,
                        'data': this.data
                    }
                });
            },
            loadedEvent: function() {
                if (this.data) {
                    var selectedNodes = []
                    for (var i in this.data) {
                        var d = this.data[i]
                        if (d && d.state) {
                            var isSelected = d.state.selected ? d.state.selected : false
                            if (isSelected) {
                                var selectedNode = {
                                    "id": d.id,
                                    "parentId": d.parentId,
                                    "text": d.text,
                                    "value": d.value
                                }
                                selectedNodes.push(selectedNode)
                            }
                        }
                    }
                    var $detail = {
                        "selectedNodes": selectedNodes
                    }
                    if (this.onloaded) {
                        if (this.id) {
                            this.onloaded(soModels.getReturnModel(this.defaultId, $detail))
                        } else {
                            this.onloaded($detail)
                        }
                    }
                }
            },
            nodeSelectedEvent: function(obj) {
                if (obj.selected) {
                    this.selection = obj.selected // get selected node
                }
                if (obj.action == "select_node" || obj.action == "deselect_node") {
                    var $detail = {}
                    if (!!obj.node) {
                        var $activeNode = obj.node
                        $detail.activeNode = {
                            "id": $activeNode.id,
                            "parentId": $activeNode.parent == "#" ? null : $activeNode.parent,
                            "text": $activeNode.text,
                            "value": $activeNode.original.value,
                            "state": {
                                "opened": $activeNode.state.opened,
                                "selected": $activeNode.state.selected
                            }
                        }
                    }
                    if (obj.selected) {
                        var selectedID = obj.selected,
                            selectedNodes = []
                        for (var i in selectedID) {
                            var id = selectedID[i]
                            for (var j in this.data) {
                                if (id.toString() == this.data[j].id.toString()) {
                                    selectedNodes.push({
                                        "id": this.data[j].id.toString(),
                                        "parentId": this.data[j].parent.toString() == "#" ? null : this.data[j].parent.toString(),
                                        "text": this.data[j].text,
                                        "value": this.data[j].value,
                                        "state": this.data[j].state ? {
                                            "opened": this.data[j].state.opened,
                                            "selected": true
                                        } : null
                                    })
                                }
                            }
                        }
                        $detail.selectedNodes = selectedNodes
                    }
                    if (this.onnodeselected) {
                        if (this.id) {
                            this.onnodeselected(soModels.getReturnModel(this.defaultId, $detail))
                        } else {
                            this.onnodeselected($detail)
                        }
                    }
                }
            }
        },
        created() {
            if (this.id) {
                this.defaultId = this.id
            }
        },
        mounted: function() {
            this.loadData()
            this.$nextTick(function() { //tree dom load complete
                var self = this
                $('#' + this.defaultId).on('loaded.jstree', function(e, data) {
                    self.loadedEvent()
                })
                $('#' + this.defaultId).on('changed.jstree', function(e, data) {
                    self.nodeSelectedEvent(data)
                })
            })
        }
    }
</script>

<style scoped>
    .tree-default-style {
        font-size: 13px;
        color: #676a6c;
    }

    .jstree-open>.jstree-anchor>.fa-folder:before {
        content: "\f07c";
    }

    .jstree-default .jstree-icon.none {
        width: 0;
    }

    .fa {
        display: inline-block;
        font: normal normal normal 14px/1 FontAwesome;
        font-size: inherit;
        text-rendering: auto;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
    }

    .fa-folder:before {
        content: "\f07b";
    }
</style>
