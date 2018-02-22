<template>
    <div class="similarbo" style="width: 300px">
        <el-popover ref="popoverset" placement="top-end" trigger="click">
            <div class="popovercontent">
                <div>
                    目标对象：
                    <el-radio-group v-model="selectBO" v-for="bo in bos" size="small">
                        <el-radio :label="bo.value">{{bo.value}}</el-radio>
                    </el-radio-group>
                </div>
                <div>
                    应用场景：
                    <el-radio-group v-model="selectScene" v-for="s in scene" size="small">
                        <el-radio :label="s">{{s}}</el-radio>
                    </el-radio-group>
                </div>
                <div v-for="(property,index) in propertySchema">
                    {{property.name}}：
                    <template v-if="property.datatype=='String'">
                        <el-select v-model="selectProperty[index]" clearable placeholder="请选择" size="mini" @change="selectPropertyChange()">
                            <el-option v-for="value in property.values" :label="value" :value="value">
                            </el-option>
                        </el-select>
                    </template>
                    <template v-else-if="property.datatype=='Decimal'">
                        <input class="minNum" type="number" @keyup="validMin(index)" v-model="selectProperty[index].min" @change="selectPropertyChange()"></input>
                        <span>——</span>
                        <input class="maxNum" type="number" @keyup="validMax(index)" v-model="selectProperty[index].max" @change="selectPropertyChange()"></input>
                    </template>
                    <template v-else-if="property.datatype=='Date'">
                        <el-date-picker v-model="property.values" type="datetimerange" placeholder="选择时间范围">
                        </el-date-picker>
                    </template>
                </div>
            </div>
        </el-popover>
        <el-card :body-style="{ padding: '0px'}"  v-if="show">
            <div class="header">
                <span style="font-size: 15px"> {{name}} </span>
                <el-button v-popover:popoverset style="float:right;background-color:#d3dce6" type="primary" size="mini" :plain="true" icon="setting">设置</el-button>
            </div>
            <div class="content">
                <el-carousel :autoplay=false arrow="hover" height="100px" indicator-position="none">
                    <el-carousel-item v-for="item in carouselItemCount">
                        <el-row v-for="r in row">
                            <a href="#" @click="itemclick(similarbo)" v-for="similarbo in getPatialSimilarBOs(item,r)">{{similarbo}}</a>
                        </el-row>
                    </el-carousel-item>
                </el-carousel>
            </div>
        </el-card>

    </div>
</template>

<script>
    import ajax from '../../utils/pollAjax'
    import str from "../../utils/stringHelper";
    import { Popover, Button, CarouselItem, Carousel, DatePicker, InputNumber, Row, RadioGroup, Radio, Card } from 'element-ui'
    export default {
        props: {
            id: String,
            name: String,
            bos: { type: Array, default: function () { return [] } },
            scene: { type: Array, default: function () { return [] } },
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
                show:isshow,
                selectScene: "",
                similarBOs: [],
                propertySchema: [],
                selectProperty: [],
                boProperty: [],
                row: 4,
                col: 4,
                isStr: false
            };
        },
        mounted: function () {
            this.getAppdomain();
            this.selectScene = this.scene[0];
            this.get3GXByFilter();
            this.getPropertySchema();
            this.getSimilarBO();
        },
        computed: {
            //用于相似对象展示结果的排列
            total: function () {
                return this.row * this.col;
            },
            carouselItemCount: function () {
                if (this.similarBOs.length > this.total) {
                    return Math.ceil(this.similarBOs.length / this.total);
                }
                return 1;
            }
        },
        methods: {
            //获得过滤条件
            getFilter: function () {
                let filters = [];
                for (let i = 0; i < this.selectProperty.length; i++) {
                    let value = this.selectProperty[i];
                    let name = this.propertySchema[i].name
                    let field = this.selectScene + "." + name;
                    //匹配过滤
                    if (name != "" && typeof (value) === "string") {
                        let strValue = { "$eq": value };
                        filters.push(this.getFilterBase(field, strValue));
                    }
                    //范围过滤
                    else if (name != "" && value != null && value instanceof Object) {
                        let minValue = { "$gte": value.min };
                        let maxValue = { "$lte": value.max };
                        filters.push(this.getFilterBase(field, minValue));
                        filters.push(this.getFilterBase(field, maxValue));
                    }
                }
                return filters.length > 0 ? { "$and": filters } : {};
            },
            getFilterBase: function (field, value) {
                let filter = {};
                filter[field] = value;
                return filter;
            },
            //获得选中的属性
            getSelectProperty: function () {
                let result = [];
                for (let i = 0; i < this.propertySchema.length; i++) {
                    result[i] = null;
                    for (let j = 0; j < this.boProperty.length; j++) {
                        let type = str.trim(this.propertySchema[i].datatype);
                        let schemaName = str.trim(this.propertySchema[i].name);
                        let propertyName = str.trim(this.boProperty[j].name);
                        if (schemaName === propertyName) {
                            if (type === "String") {
                                result[i] = str.trim(this.boProperty[j].value);
                                break;
                            }
                            else if (type === "Decimal") {
                                result[i] = { max: 0, min: 0 }
                                let numValue = parseFloat(str.trim(this.boProperty[j].value)) || 0;
                                result[i] = { max: numValue, min: numValue }
                                break;
                            }
                        }
                    }
                }
                this.selectProperty = result;
            },
            //选中的属性发生变化时刷新
            selectPropertyChange: function () {
                this.getSimilarBO();
            },
            validMin: function (index) {
                if (this.selectProperty[index].min > this.selectProperty[index].max) {
                    gtUI.message({ message: "不能大于最大值！", type: 'warning' });
                    this.selectProperty[index].min = this.selectProperty[index].max;
                }
            },
            validMax: function (index) {
                if (this.selectProperty[index].max < this.selectProperty[index].min) {
                    gtUI.message({ message: "不能小于最小值！", type: 'warning' });
                    this.selectProperty[index].max = this.selectProperty[index].min;
                }
            },
            //获得分页展示结果
            getPatialSimilarBOs: function (item, row) {
                let start = (item - 1) * this.total + (row - 1) * this.col;
                let end = start + this.col;
                let patialBOs = this.similarBOs.slice(start, end);
                return patialBOs;
            },
            //获得当前选中对象的类型
            getBOType: function () {
                for (var i = 0; i < this.bos.length; i++) {
                    if (this.bos[i].value === this.selectBO)
                        return this.bos[i].type;
                }
            },
            itemclick: function (boname) {
                this.onclick(boname);
            },
            getSimilarBO: function () {
                this.similarBOs = [];
                let _this = this;
                let bot = this.getBOType();
                let filter = this.getFilter();
                let param = { "bot": bot, "filter": filter };
                console.log("GetBOListByFilter的参数：" + JSON.stringify(param));
                let defaultUrl = "/BOService/GetBOListByFilter";
                ajax({
                    url: defaultUrl,
                    data: JSON.stringify(param),
                    contentType: "application/json",
                    type: "post",
                    cache: false,
                    dataType: 'json',
                    success: function (result) {
                        if (result != null && result.length > 0) {
                            result.forEach(function (value) {
                                _this.similarBOs.push(value.name);
                            })
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            },
            getAppdomain: function () {
                let domains = [];
                let bot = this.getBOType();
                let defaultUrl = "/BOService/GetCapabilities";
                ajax({
                    url: defaultUrl,
                    type: "get",
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        let appdomain = result.appdomains;
                        if (appdomain != null && appdomain.length > 0) {
                            for (let i = 0; i < appdomain.length; i++) {
                                if (appdomain[i].bot === bot) {
                                    domains = appdomain[i].appdomain;
                                    break;
                                }
                            }
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
                this.scene = domains;
            },
            getPropertySchema: function () {
                this.propertySchema = [];
                let _this = this;
                let appdomain = this.selectScene;
                let bot = this.getBOType();
                let param = { "bot": bot, "appdomain": appdomain }
                let defaultUrl = "/BOService/GetPropertySchema";
                ajax({
                    url: defaultUrl,
                    data: param,
                    type: "post",
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        for (let i = 0; i < result.length; i++) {
                            let property = result[i];
                            for (let j = 0; j < result[i].values.length; j++) {
                                property.values[j] = str.trim(result[i].values[j]);
                            }
                            _this.propertySchema.push(property);
                        }
                        _this.getSelectProperty();//更新当前选中的属性项
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            },
            get3GXByFilter: function () {
                this.boProperty = [];
                let _this = this;
                let bo = this.selectBO;
                let bot = this.getBOType();
                let param = { "bot": bot, "bos": [bo], "category": "P" }
                let defaultUrl = "/BOService/Get3GXByFilter";
                ajax({
                    url: defaultUrl,
                    data: param,
                    type: "post",
                    cache: false,
                    async: false,
                    dataType: 'xml',
                    success: function (result) {
                        let propertyset = $(result).find("PropertySet");
                        if (propertyset != undefined) {
                            propertyset.each(function (ps) {
                                let name = $(this).attr("name");
                                let propertys = $(this).find("P").each(function (p) {
                                    let n = $(this).attr("n");
                                    let t = $(this).attr("t");
                                    let r = $(this).attr("r");
                                    let value = $(this).text();
                                    _this.boProperty.push({ "ns": name, "name": n, "op": r, "value": value })
                                })
                            })

                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
        },
        watch: {
            //当目标对象变化时 重新读取 对应的应用场景以及该目标对应属性集
            selectBO: function () {
                this.getAppdomain();
                let lastScene = this.selectScene;
                this.selectScene = this.scene[0]
                this.get3GXByFilter();
                if (lastScene === this.scene[0]) {
                    this.selectProperty = [];
                    this.getPropertySchema();
                }
            },
            //当应用场景变化时，读取对应的属性集合
            selectScene: function () {
                this.selectProperty = [];
                this.getPropertySchema();
                this.getSimilarBO();
            },
            selectProperty: {
                handler: function () {
                    this.getSimilarBO();
                },
                deep: true
            }
        },
        components: {
            "el-radio": Radio,
            "el-radio-group": RadioGroup,
            "el-card": Card,
            "el-carousel": Carousel,
            "el-carousel-item": CarouselItem,
            "el-row": Row,
            "el-button": Button,
            "el-popover": Popover,
            "el-date-picker": DatePicker,
            "el-input-number": InputNumber
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

    input {
        width: 50px
    }
</style>
