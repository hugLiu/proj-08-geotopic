<template>
  <div class="semantics" style="width: 300px">
    <el-popover ref="popoverset" placement="top-end" trigger="click">
      <div class="popovercontent">
        <div>目标叙词：
          <el-radio-group v-model="selectTerm" v-for="term in terms" size="small">
            <el-radio :label="term">{{term}}</el-radio>
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
        <el-tree :data="semanticstree" :props="defaultProps" :node-key="id" accordion @node-click="handleNodeClick"></el-tree>
      </div>
    </el-card>

  </div>
</template>

<script>
  import ajax from '../../utils/pollAjax'
  import listToTree from "../../utils/listToTree"
  import { Popover, Button, Tree, RadioGroup, Radio, Card } from 'element-ui'
  export default {
    props: {
      id: String,
      name: String,
      terms: { type: Array, default: function () { return [] } },
      relation: Object,
      semanticstree: { type: Array, default: function () { return [] } },
      defaultProps: {
        type: Object,
        default: function () {
          return {
            children: 'children',
            label: 'text'
          }
        }
      },
      onclick: Function
    },
    data: function () {
      let select = "";
      let isshow=false;
      if (this.terms.length) {
          select = this.terms[0];
          isshow=true;
        }
      return {
        selectTerm: select,
        show:isshow,
        parentid: null
      };
    },
    mounted: function () {
      this.getSematicsTree();
    },
    methods: {
      handleNodeClick: function (term) {
        this.onclick(term.text);
      },
      getSematicsTree: function () {
        this.semanticstree = [];
        let pathterm = this.selectTerm;
        //获得当前选中叙词信息（支持pathterm、term过滤）
        let current = this.getCurrent(pathterm);
        let term = current.term;
        let nodes = [];
        for (let r in this.relation) {
          let sr = this.relation[r].sr;
          let direction = this.relation[r].direction;
          let param = "?term=" + term + "&sr=" + sr + "&direction=" + direction;
          let pid = "";
          switch (r) {
            case "parent":
              pid = null;
              break;
            case "brother"://定义 brother的参数是获得父亲的
              let parent = this.getParent(param);
              //改变direction方向获得兄弟
              direction = (direction) == "forward" ? "backward" : "forward";
              param = "?term=" + parent.pterm + "&sr=" + sr + "&direction=" + direction;
              pid = parent.pid;
              break;
            case "child":
              pid = current.id; break;
            default: break;
          }
          this.getNodes(param, pid).forEach(function (node) { nodes.push(node); });
        }
        this.semanticstree = listToTree(nodes);
      },
      getParent: function (pparam) {
        let result = this.getSemanticsResult(pparam);
        let pid = result[0].termclassid;
        let pterm = result[0].term;
        return { "pid": pid, "pterm": pterm };//[id, pterm];
      },
      getNodes: function (param, pid) {
        let result = this.getSemanticsResult(param);
        let nodes = [];
        for (let i = 0; i < result.length; i++) {
          let bo = { "parentId": pid, "id": result[i].termclassid, "name": result[i].term }
          nodes.push(bo);
        }
        return nodes;
      },
      getCurrent: function (term) {
        let defaultUrl = "/SemanticsService/GetTermInfo";
        let param = { "term": [term], "cc": "" };
        let currentid = "";
        let currentterm = "";
        ajax({
          url: defaultUrl,
          data: JSON.stringify(param),
          contentType: "application/json",
          type: "post",
          async: false,
          cache: false,
          dataType: 'json',
          success: function (result) {
            currentid = result[0].termclassid;
            currentterm = result[0].term;
          },
          error: function (err) {
            console.log(err);
          }
        });
        return { "id": currentid, "term": currentterm };
      },
      getSemanticsResult: function (param) {
        let defaultUrl = "/SemanticsService/Semantics";
        let result = [];
        ajax({
          url: defaultUrl + param,
          type: "get",
          async: false,
          cache: false,
          dataType: 'json',
          success: function (data) {
            result = data;
          },
          error: function (err) {
            console.log(err);
          }
        });
        return result;
      }
    },
    watch: {
      selectTerm: function () {
        this.getSematicsTree();
      }
    },
    components: {
      "el-radio": Radio,
      "el-radio-group": RadioGroup,
      "el-card": Card,
      "el-tree": Tree,
      "el-button": Button,
      "el-popover": Popover
    }
  }

</script>

<style scoped>
  a {
    padding: 10px;
  }

  .header {
    padding: 5px;
    background-color: #d3dce6;
    height: 30px;
    border-bottom: 1px solid lightgrey
  }
</style>
