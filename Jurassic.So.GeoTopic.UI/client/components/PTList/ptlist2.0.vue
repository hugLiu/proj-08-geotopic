<template>
    <div id=".searchList">
        <searchlist :filters="filterlist" :data="datalist" :highlight="highlight" :showfilter="showfilter" :allowpaging="allowpaging"
            :size="size" :count="count" :onselectpage="Pager" :onitemclick="TitleClick" :oncollect="collectClick" :onfilterclear="FilterClean"
            :onfilterclick="FilterClick">
            </searchlist>
    </div>
</template>
<script>
    import ajax from '../../utils/pollAjax'
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
        "title": "时间",
        "filter": "CreatedDate",
        "checkbox": false,
        "droplist": [{ text: "最近一天", value: date.getYesterday(new Date()), count: 0 },
        { text: "最近一月", value: date.getLastMonth(new Date()), count: 0 },
        { text: "最近一年", value: date.getLastYear(new Date()), count: 0 }]
    };
    let filterItems = ["SourceName", "SourceFormat", "Author"];

    export default {
        props: {
            param: Object,
            refresh: Boolean
        },
        data: function data() {
            return {
                allowpaging: false,
                showfilter: true,
                titletemplate: "",
                filterlist: [],
                filtertitle: "筛选条件",
                tip: "",
                pagenum: 10,
                size: 10,
                count: 1,
                strTemplate: "",
                datalist: [],
                collections: [],
                research: false,
                currentParam: {}
            }
        },
        computed: {
            highlight: function () {
                if (typeof (this.param.sentence) != "undefined")
                    return this.param.sentence.split("");
                else return [];
            }
        },
        mounted: function () {
            var pagefilter = { "from": 0, "size": this.size };
            let gfield = [];
            for (let item of filterItems) {
                gfield.push(operation.removeSubscript(operation.getJsonPath(item)));
            }
            var grouprule = {
                "Top": -1,
                "GFields": gfield
            }
            this.param.grouprule = grouprule;
            this.SetParms(pagefilter);
            this.getData();
        },
        methods: {
            getData: function () {
                let pmdata = this.currentParam;
                delete pmdata["viewtype"];
                let _this = this;
                let defaultUrl = "/SearchService/Search";
                ajax({
                    url: defaultUrl,
                    data: JSON.stringify(pmdata),
                    contentType: "application/json",
                    type: "post",
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        // let data = JSON.parse(result);
                        _this.count = result.count;
                        _this.GetCollection();
                        _this.GetListData(result.metadatas);
                        if (!_this.research) {
                            _this.GetFilterData(result.groups);
                        }
                        if (result.count > _this.size) _this.allowpaging = true;
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
                this.research = true;
                this.getData();
            },

            TitleClick: function (value) {
                window.open("/Render/PTDetail?iiid=" + value.id)
            },

            //点击收藏
            collectClick: function (item) {
                var success = function () {
                    item.collectImg = (item.collectId > 0 ? 'collect' : 'save');
                }
                this.$store.dispatch('checkCollecting', { item: item, success: success, error: null });
            },
            FilterClick: function (checkedfilters) {
                let filters = []
                for (let j in checkedfilters) {
                    let checkedfilter = checkedfilters[j];
                    let field = checkedfilter.filter;
                    let filter = [];
                    for (let i in checkedfilter.data) {
                        let value = checkedfilter.data[i].value;
                        let filterItems = operation.buildFilter(field, value);
                        let filterAnd =this.getFilter(filterItems, "$and");
                        filter.push(filterAnd);
                    }
                    //一个dropdown过滤器 filter参数
                    if (filter.length > 0) {
                        let filterOr = this.getFilter(filter, "$or");// filter.length > 1 ? { "$or": filter } : filter[0];
                        filters.push(filterOr);
                    }
                }
                //多个dropdown过滤器 filter参数
                let dropdownFilter = this.getFilter(filters, "$and");// filters.length > 1 ? { "$and": filters } : filters[0];
                //搜索 filter参数
                let searchFilter = this.param.filter;
                //合并过滤条件
                let filterCombine = [];
                filterCombine.push(dropdownFilter);
                if (searchFilter != null && JSON.stringify(searchFilter) != "{}") {
                    filterCombine.push(searchFilter);
                }
                this.currentParam.filter = this.getFilter(filterCombine, "$and");// { "$and": filterCombine };

                console.log(JSON.stringify(this.currentParam.filter));
                this.research = true;
                this.getData();
            },
            getFilter: function (filters, op) {
                let resultFilter = {};
                if (filters.length > 1) {
                    resultFilter[op] = filters
                }
                else if (filters.length == 1) {
                    resultFilter = filters[0];
                }
                return resultFilter;
            },
            FilterClean: function () {
                this.research = true;
                this.currentParam.filter = array.deepClone(this.param.filter);
                this.getData();
            },

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
                var matches = this.collections.filter(function (e) {
                    if (e.JContent == null) {
                        e.JContent = JSON.parse(e.Content);
                    }
                    return e.JContent.id === item.id;
                });
                if (matches.length > 0) {
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
                        if (type === "DateString" && value != "") {
                            value = date.formatCSTDate(value, "yyyy-MM-dd hh:mm:ss");
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
                for (var item of filterItems) {
                    let title = operation.removeSubscript(operation.getJsonPath(item));//获得实际的字段名称
                    if (data[title]) {
                        let filterModel = [];
                        filterModel.droplist = [];
                        filterModel.checkbox = true;
                        filterModel.filter = item;
                        filterModel.title = operation.getTitle(item);
                        for (var name in data[title]) {
                            filterModel.droplist.push({ text: name, value: name, count: data[title][name] });
                        }
                        this.filterlist.push(filterModel);
                    }
                }
            },
            SetParms: function (pagefilter) {
                this.param.pager = pagefilter;
                this.currentParam = array.deepClone(this.param);
            }
        },
        watch: {
            'refresh': function () {
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
