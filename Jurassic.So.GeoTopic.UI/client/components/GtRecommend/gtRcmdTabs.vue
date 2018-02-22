<template>
  <div class="tabs">
    <el-tabs type="border-card">
      <el-tab-pane v-for="tab in tabs" v-bind:label="tab.name">
        <div v-for="item in getData(tab.filter)">
            <a v-if="item.title!==null" v-bind:id="item.iiid" href="javascript:void(0);" @click="titleclick(item.iiid)">{{item.title}}</a>
            <br v-if="item.title===null" />
        </div>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
  import {Tabs,TabPane} from 'element-ui'
  import ajax from "../../utils/pollAjax"
  import jsonPath from "../../utils/jsonpath"
  import modelmapping from "./../../Utils/MetadataDefOperation"
  export default{
    props: {
        id: String,
        showtab: {
            type: Boolean,
            default:false
        },
        itemsize: {
            type: Number,
            default:5
        },
        displayfield: Array,
        tabs: Array,
        onitemclick: Function
    },
    data: function(){
      return {

      };
    },
    methods: {
        getData: function (filter) {
          let searlist = [];
          for(let i = 0; i < this.itemsize; i++){
            searlist.push({title:null, iiid:"#"});
          }
          let _this = this; //use for ajax
           let defaultUrl = "/SearchService/Match";
          ajax({
            type: "post",
            url: defaultUrl,
            data: JSON.stringify(filter),
            contentType: "application/json",
            cache: false,
            async: false,
            dataType: 'json',
            success: function (result) {
              if (result.count > 0) {
                let size = result.metadatas.length;
                if(size > _this.itemsize){
                  size = _this.itemsize;
                }

                for (let i = 0; i < size; i++) {
                    let iiid = _this.getMetadataByName(result.metadatas[i], _this.displayfield[0]);
                    let title = _this.getMetadataByName(result.metadatas[i], _this.displayfield[1]);
                    let format = _this.getMetadataByName(result.metadatas[i], _this.displayfield[2]);
                    searlist[i] = {iiid: iiid, title: title, format: format};
                }
              }
            },
            error: function (err) {
              console.log(err);
            }
          });

          return searlist;
        },
        getMetadataByName: function (metadata, name) {
          let defPath = modelmapping.getJsonPath(name);
          let values = jsonPath(metadata, "$." + defPath);
          if(values!==null&&values.length>0){
              return values[0];
          }else {
              return null;
          }
        },
        titleclick: function (value) {
            this.onitemclick(value);
      },

    },
    components: {
        "el-tabs": Tabs,
        "el-tab-pane": TabPane
    }
  }

</script>

<style scoped>
  a {
    line-height:normal;
  }
</style>
