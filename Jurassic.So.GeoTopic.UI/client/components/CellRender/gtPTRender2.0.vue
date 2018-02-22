<template>
    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-heading" v-if="istool">
            <h5 style="position:relative;">
                &nbsp;
                <!-- 标题 -->
                <!-- {{pram.title}} -->
                <div style="float:left;" class='text-overflow' :title="file.name || '未命名'">
                    {{(pram.title?(pram.title +" - "):"")+(file.name || "未命名")}}
                </div>
                <!--  文件切换 -->
                <span style="position:absolute;left:44%;">
                    <!-- 左切 -->
                   <a style="padding-right: 20px;font-size: 18px;" class="el-icon-arrow-left" v-if="fileIndex>0" v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1" @click="left"></a>
                   <span disabled = 'true' style="color: #e8e8e8; padding-right: 20px;font-size: 18px;" class="el-icon-arrow-left" v-else v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1"></span>
                <!-- 切换标识 -->
                <span style="font-size: 12px;" v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1">{{fileIndex+1}} / {{!paramlist[infoIndex]?1:paramlist[infoIndex].files.length}}</span>
                <!-- 右切 -->
                <a style="padding-left: 20px;font-size: 18px;" class="el-icon-arrow-right" v-if="paramlist[infoIndex] && (fileIndex < paramlist[infoIndex].files.length-1)"
                    v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1" @click="right"></a>
                    <span disabled='true' style="color: #e8e8e8;  padding-left: 20px;font-size: 18px;" class="el-icon-arrow-right" v-else v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1"></span>
                    </span>
                    <!-- 工具按钮 -->
                    <span style="float:right;font-size: 16px;">
                    <!-- 评论 -->
                    <template v-if="isremarks">
                        <a style="cursor:pointer" class="glyphicon glyphicon-comment" title="评论" v-if="file.fileUrl != null" target="_blank" @click="gotoFunc('/Render/Remarks?scoap=ptfile&key='+encodeURIComponent(file.ticket))"></a>
                        <!-- <a class="glyphicon glyphicon-comment" title="评论" target="_blank" :href="'/Render/Remarks?scoap=ptfile&key='+encodeURIComponent(file.ticket)"></a> -->
                        <span disabled = 'true' style="color: #e8e8e8;" class="glyphicon glyphicon-comment" v-else></span>
</template>
<!-- 下载 -->
<template v-if="isdown">
    <a class="glyphicon glyphicon-save" title="下载" target="_blank" v-if="file.fileUrl != null" :href="file.fileUrl"></a>
    <span disabled='true' style="color: #e8e8e8;" class="glyphicon glyphicon-save" v-else></span>
</template>
<!-- 预览 -->
<template v-if="isview">
    <a class="el-icon-view" title="预览" target="_blank" v-if="file.url != null" :href="'/Render/PtUrl?param='+encodeURIComponent(JSON.stringify(pram))+'&infoIndex='+infoIndex+'&fileIndex='+fileIndex"></a>
    <span disabled='true' style="color: #e8e8e8;" class="el-icon-view" v-else></span>
</template>
<!-- 提交 -->
<template v-if="issubmit">
    <a class="el-icon-upload" style="cursor:pointer" title="提交" target="_blank" @click="gotoFunc('/Render/Submission?param='+JSON.stringify(goParam2(pram)))"></a>
</template>
</span>
</h5>
</div>
<div class="panel-body" style="height:100%;width:100%;">
    <!-- 测试区域 -->
    <!-- loading -->
    <!-- losding有问题，先显示了没有成果 -->
    <div v-if="isLoading" :style="stylestr">
        <span class="el-icon-loading" style="font-size: 45px;position:absolute;left:45%;top:45%;"></span>
    </div>
    <!-- 内容 -->
    <div v-else>
        <div v-if="paramlist ==null || paramlist.length == 0">
            <div>
                <h4>没有成果</h4>
            </div>
        </div>
        <div v-else>
            <component :is="mo" :param="file" keep-alive></component>
        </div>
    </div>
</div>
<div v-if="isfoottool">
    <!-- 成果切换 -->
    <div v-if="paramlist.length > 1">
        <el-pagination small layout="prev, pager, next" :page-size="1" :total="paramlist.length" :current-page="infoIndex+1" :page-count="paramlist.length"
            @current-change="infoClick">
            </el-pagination>
    </div>
</div>
<div :id="'iframe'+nodeId">
    <iframe style="position:absolute;left:0px;top:0px;" src="about:blank" width="100%" height="100%" scrolling="no" frameborder="0"></iframe>
</div>
<div :id="'ptrendermodel'+nodeId" class="modal fade" aria-hidden="true" data-backdrop="static">
    <div class="modal-dialog" style="width:70%;height:600px;">
        <div class="modal-content">
            <div class="modal-header" style="background-color:#f8f8f8">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h5 class="modal-title">{{pram.title}}</h5>
            </div>
            <div class="modal-body">
                <div class="embed-responsive embed-responsive-16by9">
                    <iframe class="embed-responsive-item" :src="gotoUrl"></iframe>
                </div>
            </div>
        </div>
    </div>
</div>
</div>
</template>

<script>
    //import  models from '../../utils/getCom'
    import ajax from '../../utils/pollAjax'
    import ao from "../../utils/arrayandobjectoperation"
    import bo from "../../utils/baseoperation"
    import { Pagination } from 'element-ui'
    import model from "../../utils/loadmodel"
    import MetadataDefOperation from "./../../Utils/MetadataDefOperation";
    var renderModel = {
        "_rich": require('../formatcom/gtrich.vue'),
        "_img": require('../formatcom/gtimg.vue'),
        "_gdb": require('../formatcom/gtgdb.vue'),
        "_chart": require('../formatCom/gtpiechart.vue'),
        "_dataset": require('../formatCom/gtdataset.vue'),
        "_noresults": require('../formatCom/gtNoResults.vue')
    };

    //let loadingInstance;
    export default {
        props: {
            pram: {
                type: Object,
                default: {}
            },
            global: {
                type: Object,
                default: null
            },
            heights: {
                type: Number,
                default: 0
            },
            infoindex: {
                type: Number,
                default: 0
            },
            fileindex: {
                type: Number,
                default: 0
            },
            istool: {
                type: Boolean,
                default: true
            },
            isremarks: {
                type: Boolean,
                default: true
            },
            isdown: {
                type: Boolean,
                default: true
            },
            isview: {
                type: Boolean,
                default: true
            },
            issubmit: {
                type: Boolean,
                default: true
            },
            isfoottool: {
                type: Boolean,
                default: true
            },
        },
        data: function () {
            return {
                paramlist: [],
                file: {},
                infoIndex: this.infoindex,//0,
                fileIndex: this.fileindex,//0,
                metadataDefinition: [],
                isLoading: false,
                gotoUrl: "",
                mo: null,
                stylestr: {
                    height: this.heights == 0 ? "100%" : this.heights + 'px'
                },
                nodeId: bo.getGuid()
            }
        },
        created: function () {
            this.init();
        },
        mounted: function () {
            //不能放到init中，因为时机不同。
            let _this = this;
            $("#iframe" + this.nodeId).hide();
            $("#ptrendermodel" + this.nodeId).on('hide.bs.modal', function (e) {
                $("#iframe" + _this.nodeId).hide();
            })
        },
        // updated:function(){
        // },
        methods: {
            gotoFunc: function (url) {
                this.gotoUrl = url;
                //this.isGotoModel = true;
                $('#ptrendermodel' + this.nodeId).modal();
                $("#iframe" + this.nodeId).show();
            },
            infoClick: function (index) {
                this.infoIndex = index - 1;
                this.fileIndex = 0;
                this.setFile(this.infoIndex, this.fileIndex);
            },
            left: function () {
                if (this.fileIndex == 0) {
                    gtUI.message({ message: "前面没有了！", type: 'info' });
                    return;
                }
                this.fileIndex--;
                this.setFile(this.infoIndex, this.fileIndex);
            },
            right: function () {
                if (this.fileIndex == this.paramlist[this.infoIndex].files.length - 1) {
                    gtUI.message({ message: "后面没有了！", type: 'info' });
                    return;
                }
                this.fileIndex++;
                this.setFile(this.infoIndex, this.fileIndex);
            },
            setFile: function (infoIndex, fileIndex) {
                this.file = {};
                this.mo = this.$options.components["_" + model.getConfig(this.paramlist[infoIndex].files[fileIndex].format, "ptrender")];
                let url = "?id="+ MetadataDefOperation.getValue(this.paramlist[infoIndex].kmd, "IIId") +
                    "&title="+ escape(MetadataDefOperation.getValue(this.paramlist[infoIndex].kmd, "FormalTitle")) +
                    "&url="+ escape(this.paramlist[infoIndex].url)+
                    "&ticket=" + escape(this.paramlist[infoIndex].files[fileIndex].ticket) +
                    "&format=" + escape(this.paramlist[infoIndex].files[fileIndex].format);
                this.file = {
                    url: "/FileInfo/GetStream" + url,
                    format: this.paramlist[infoIndex].files[fileIndex].format,
                    heights: this.heights,
                    fileUrl: "/FileInfo/DownLoad" + url + "&name=" + escape(this.paramlist[infoIndex].files[fileIndex].name),
                    name: this.paramlist[infoIndex].files[fileIndex].name,
                    ticket: this.paramlist[infoIndex].files[fileIndex].ticket
                };
            },
            //初始方法， param数据改变时或者第一次进来是调用
            init: function () {
                this.paramlist = [];
                this.infoIndex = 0;
                this.fileIndex = 0;
                this.file = {};//不能省略
                this.mo = null;
                this.getData();
            },
            getParam: function () {
                if(this.metadataDefinition == null){
                  MetadataDefOperation.init();
                  this.metadataDefinition = MetadataDefOperation.metadataDefinition;
                }
            },
            //转化参数，转换成match方法需要的kmd参数，并且替换全局变量。返回一个替换后的备份，不修改原始参数
            goParam: function () {
                if (!this.pram.param || !this.pram.param.filter) return this.pram.param;
                var global = this.global || {};
                let pa = ao.deepClone(this.pram.param);
                //按层次级别遍历，不区分$and或者$or，保留一定扩展性.
                for (let i in pa.filter) {
                    let newFilter = [];
                    for (let j = 0; j < pa.filter[i].length; j++) {
                        //这层默认只有一条对象，这里要做替换处理，所以不太好处理太复杂的逻辑。
                        for (let z in pa.filter[i][j]) {
                            let value = "";
                            if (pa.filter[i][j][z].substring(0, 1) == "@") {
                                value = global[pa.filter[i][j][z].substring(1, pa.filter[i][j][z].length)];
                            }
                            else {
                                value =  pa.filter[i][j][z] ;
                            }
                            newFilter = MetadataDefOperation.buildFilter(z, value);
                        }
                    }
                    pa.filter[i] = newFilter;
                }
                return pa;
            },
            //替换全局变量，用于上传。返回一个替换后的备份，不修改原始参数。
            goParam2: function () {
                if (!this.pram.param || !this.pram.param.filter) return this.pram.param;
                var global = this.global || {};
                // let pa = this.pram.param;
                let pa = ao.deepClone(this.pram.param);
                if (this.metadataDefinition.length == 0) this.getParam();
                //let result = this.metadataDefinition;
                //pDic = result;
                //按层次级别遍历，不区分$and或者$or，保留一定扩展性.
                for (let i in pa.filter) {
                    for (let j = 0; j < pa.filter[i].length; j++) {
                        //这层默认只有一条对象，这里要做替换处理，所以不太好处理太复杂的逻辑。
                        for (let z in pa.filter[i][j]) {
                            if (pa.filter[i][j][z].substring(0, 1) == "@") {
                                pa.filter[i][j][z] = global[pa.filter[i][j][z].substring(1, pa.filter[i][j][z].length)];
                            }
                        }
                    }
                }
                return pa;
            },
            getData: function () {
                this.isLoading = true;
                //这里开始Loading。。。
                //let param = this.goParam();
                //这里需要深拷贝,目的是不改变原始的pram参数，这样可以递归使用PtRender。
                // let param = ao.deepClone(this.goParam());
                // delete param["viewtype"];
                //let _this = this;
                this.Match();
                //要是按异步获取数据走逻辑是没有问题，但是loading是有问题的，而且有出现文件数据获取不完整的情况。
                //异步的情况会先显示没有成果，然后再出成果，在异步的时候这个要处理。
                this.isLoading = false;
            },
            Match: function () {
                //Match和Retrieve方法的异步会让Api调用出现调用失败，这里需要测试。
                //PtRender的代码逻辑是完全支持异步执行的。
                //let pmdata = this.pram.param;
                let param = ao.deepClone(this.goParam());
                let _this = this;
                ajax({
                    url: "/SearchService/Match",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(param),
                    async: false,
                    success: function (result) {
                        for (let i = 0; i < result.metadatas.length; i++) {
                         var metadata = result.metadatas[i];
                           _this.Retrieve(metadata, metadata["source"]["url"]);
                        }
                    }
                });
            },
            Retrieve: function (metadata, urlstr) {
                // this.paramlist = [];
                let filesPa = [];
                let _this = this;
                ajax({
                    url: "/DataService/Retrieve",
                    type: "get",
                    data: { url: urlstr },
                    async: false,
                    success: function (result) {
                        console.log(JSON.stringify(result));
                        //组织成果和文件结构
                        for (let i = 0; i < result.length; i++) {
                            filesPa.push({ ticket: result[i].ticket, format: result[i].format.toLowerCase(), name: result[i].name });
                        }
                        _this.paramlist.push({ kmd:metadata, url: urlstr, files: filesPa, fileCount: result.length });
                        if (_this.paramlist.length == 1 && _this.paramlist[0].files.length >= 1) {
                            _this.setFile(0, 0);
                        } else {
                            if (_this.paramlist.length == _this.infoindex + 1)
                                _this.setFile(_this.infoindex, _this.fileindex);
                        }
                    }
                });
            }
        },
        watch: {
            pram: function () {
                this.init();
            }
        },
        components: Object.assign(renderModel, {
            //"el-dialog": Dialog,
            "el-pagination": Pagination
        })
    }

</script>
<style scoped>
    .text-overflow {
        width: 40%;
        /*    height:40%;
    border:1px solid red;
    border-top:4px solid red;
    padding:3px;*/
        overflow: hidden;
        ;
        /* 内容超出宽度时隐藏超出部分的内容 */
        text-overflow: ellipsis;
        ;
        /* 当对象内文本溢出时显示省略标记(...) ；需与overflow:hidden;一起使用。*/
        white-space: nowrap;
        /* 不换行 */
    }
</style>
