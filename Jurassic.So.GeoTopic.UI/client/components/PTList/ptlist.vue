<template>
    <div id=".searchList">
        <searchlist :filters="filterlist" :data="datalist" :highlight="highlight" :showfilter="showfilter" :allowpaging="allowpaging"
            :size="size" :count="count" :onselectpage="Pager" :onitemclick="TitleClick" :oncollect="collectClick" :onfilterclear="FilterClean"
            :onfilterclick="FilterClick">
        </searchlist>
    </div>
</template>
<script>
    import operation from "./../../Utils/MetadataDefOperation";
    import jsonPath from '../../utils/jsonpath';
    import date from "../../utils/dateHelper";
    import array from '../../utils/arrayAndObjectOperation';
    import soUI from '../../SoUI/soUI';
    var defaultListModel = {
        id: "",
        title: "",
        imgStr: "",
        format: "",
        items: []
    };

    var filterdata = {
        "title": "",
        "filter": "",
        "checkbox": false,
        "droplist": []
    };

    var ftitleModel = {
        "s:datasourcename": "来源",
        "s:format": "格式",
        "format": "文件类型",
        "ep:o:organization": "单位",
        "b:contributor": "作者"
    };
    export default {
        props: {
            param: Object,
            refresh:Boolean
        },
        data: function data() {
            return {
                allowpaging: false,
                showfilter: false,
                titletemplate: "",
                filterlist: [],
                filtertitle: "筛选条件",
                tip: "",
                pagenum: 10,
                size: 10,
                count: 1,
                strTemplate: "",
                datalist: [],
                highlight: [],
                collections: [],
            }
        },
        mounted: function () {
            var pagefilter = { "from": 0, "size": this.size };
            this.SetParms(pagefilter);
            this.getData();
        },
        methods: {
            //石亚茹Match接口
            getData: function () {
                let pmdata = this.param;
                delete pmdata["viewtype"];
                let _this = this;
                $.ajax({
                    url: _this.$store.state.siteUrl + "/Document/GetMatchData",
                    data: { param: JSON.stringify(pmdata) },
                    type: "get",
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        let data = JSON.parse(result);
                        _this.count = data.count;
                        _this.GetCollection();
                        _this.GetListData(data.metadatas);
                        if (data.count > _this.size) _this.allowpaging = true;
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            },

            Pager: function (pageIndex) {
                var startIndex = (pageIndex - 1) * this.size;
                var pagefilter = { "from": startIndex, "size": this.size };
                this.SetParms(pagefilter);
                this.getData();
            },

            TitleClick: function (value) {
                window.open("/Render/PTDetail?iiid=" + value.id)
            },

            //点击收藏
            collectClick: function (item) {
              var success = function(){
                item.collectImg = (item.collectId > 0 ? 'collect' : 'save');
              }
              this.$store.dispatch('checkCollecting', {item: item, success: success, error: null});
            },

            FilterClick: function () { },
            FilterClean: function () { },

            GetCollection() {
                let _this = this;
                $.ajax({
                    url: _this.$store.state.siteUrl + "/UserBehavior/GetAllFavoriteMessByUserId",
                    data: {},
                    type: "get",
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        _this.collections = result;
                    },
                    error: function (result) {
                        console.log(result);
                    }
                });
            },

            //检查收藏
            checkCollection: function (item) {
              var matches = this.collections.filter(function(e){
                if(e.JContent == null){
                  e.JContent = JSON.parse(e.Content);
                }
                return e.JContent.id === item.id;
              });
              if(matches.length > 0) {
                item.collectImg = "collect";
                item.collectId = matches[0].Id;
              } else {
                item.collectImg = "save";
                item.collectId = 0;
              }
            },

            GetListData: function (data) {
                this.datalist = [];
                for (var i = 0; i < data.length; i++) {
                    let listModel = array.deepClone(defaultListModel); //调用深度克隆
                    var item = data[i];
                    let _this = this;
                    let showItems = ["SourceName", "CreatedDate", "Author", "Abstract"];
                    showItems.forEach(function (i) {
                        let title = operation.getTitle(i);
                        let type = operation.getDataType(i);
                        let value = _this.getValue(item, i);
                        if (type === "DateString" && value!="") {
                            value = date.formatCSTDate(value,"yyyy-MM-dd hh:mm:ss");
                        }
                        listModel.items.push({ "title": title, "value": value });
                    })
                    listModel.id = this.getValue(item, "IIId");
                    listModel.title = this.getValue(item, "FormalTitle");
                    //检查是否收藏
                    this.checkCollection(listModel);
                    listModel.format = this.getValue(item, "SourceFormat");
                    if (listModel.format != "")
                        listModel.format = listModel.format.toLocaleLowerCase();
                    listModel.imgStr = this.getValue(item, "Thumbnail");
                    if (listModel.imgStr != "" && listModel.imgStr.indexOf('data:image') != 0)
                        listModel.imgStr = "data:image/gif;base64," + listModel.imgStr;

                    this.datalist.push(listModel);
                }
            },
            getValue: function (document, name) {
                let path = "$." + operation.getJsonPath(name);
                let value = jsonPath(document, path);
                return value[0] || "";
            },

            GetFilterData: function (data) {
                this.filterlist = [];
                this.filterlist.push(filterdata);
                for (var title in ftitleModel) {
                    if (data[title]) {
                        let filterModel = [];
                        filterModel.droplist = [];
                        filterModel.checkbox = true;
                        filterModel.filter = title;
                        filterModel.title = ftitleModel[title];
                        for (var name in data[title]) {
                            filterModel.droplist.push({ text: name, value: "", count: "" });
                        }
                        this.filterlist.push(filterModel);
                    }
                }
            },
            SetParms: function (pagefilter) {
                this.param.pager = pagefilter;
                this.highlight = this.$store.state.SearchService.highlightWords;
            }
        },
        watch:{
            'refresh':function(){
                var pagefilter = { "from": 0, "size": this.size };
                this.SetParms(pagefilter);
                this.getData();
            }
        },
        components: {
            searchlist: soUI.searchlist
        }
    };
</script>

<style scoped>
    .searchList {
        font-family: '微软雅黑', arial, verdana, sans-serif;
        font-size: 14px;
    }
</style>
