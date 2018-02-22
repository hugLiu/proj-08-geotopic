<template>
    <div class="nearbo" style="width: 300px">
        <el-popover ref="popoverset" placement="top-end" trigger="click">
            <div class="popovercontent">
                <div>目标对象：
                <el-radio-group v-model="selectBO" v-for="bo in bos" size="small">
                    <el-radio :label="bo.value">{{bo.value}}</el-radio>
                </el-radio-group>
                </div>
                <p style="cursor:pointer;margin-top:8px;margin-bottom:8px;">
                    查询范围：相邻
                    <input class="inputdistance" type="number" v-model="distance"></input> 米的 <span style="color: red;">{{getBOType()}}</span>
                </p>
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
                            <a href="#" @click="itemclick(nearbo)" v-for="nearbo in getPatialNearBOs(item,r)">{{nearbo}}</a>
                        </el-row>
                    </el-carousel-item>
                </el-carousel>
            </div>
        </el-card>

    </div>
</template>

<script>
    import ajax from '../../utils/pollAjax'
    import { Popover, Button, CarouselItem, Carousel, Row, RadioGroup, Radio, Card } from 'element-ui'
    export default {
        props: {
            id: String,
            name: String,
            bos:  { type: Array, default: function () { return [] } },
            distance: {
                type: Number,
                default: 1000
            },
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
                nearBOs: [],
                row: 4,
                col: 4
            };
        },
        mounted: function () {
            this.getNearBO();
        },
        computed: {
            total: function () {
                return this.row * this.col;
            },
            carouselItemCount: function () {
                if (this.nearBOs.length > this.total) {
                    return Math.ceil(this.nearBOs.length / this.total);
                }
                return 1;
            }
        },
        methods: {
            getPatialNearBOs: function (item, row) {
                let start = (item - 1) * this.total + (row - 1) * this.col;
                let end = start + this.col;
                let patialBOs = this.nearBOs.slice(start, end);
                return patialBOs;
            },
            getBOType: function () {
                for (var i = 0; i < this.bos.length; i++) {
                    if (this.bos[i].value == this.selectBO)
                        return this.bos[i].type;
                }
            },
            itemclick: function (boname) {
                this.onclick(boname);
            },
            getNearBO: function () {
                let botype = this.getBOType();
                let nearbonames = [];
                let param = { "botype": botype, "boname": this.selectBO, "distance": this.distance, "bot": botype }
                let defaultUrl = "/BOService/GetNearBOByName";
                ajax({
                    url: defaultUrl,
                    data: param,
                    type: "post",
                    cache: false,
                    dataType: 'json',
                    success: function (result) {
                        if (result != null && result.length > 0) {
                            result.forEach(function (value) {
                                nearbonames.push(value.name);
                            })
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
                this.nearBOs = nearbonames;
            }
        },
        watch: {
            selectBO: function () {
                this.getNearBO();
            },
            distance: function () {
                this.getNearBO();
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

    p .inputdistance {
        color: red;
        width: 50px
    }
</style>
