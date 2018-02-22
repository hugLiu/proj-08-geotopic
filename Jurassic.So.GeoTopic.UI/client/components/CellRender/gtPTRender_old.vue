<template>
    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-heading">
            <h5 style="position:relative;">
                &nbsp;
                <!-- 标题 -->
                <!-- {{pram.title}} -->
                <div style="float:left;" class='text-overflow' :title="file.name || '未命名'">
                    {{pram.title+" - "+(file.name || "未命名")}}
                </div>
               <!--  文件切换 -->
               <span style="position:absolute;left:44%;">
                    <!-- 左切 -->
                   <a style="padding-right: 20px;font-size: 18px;" class="el-icon-arrow-left" v-if="fileIndex>0" v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1" @click="left"></a>
                   <span disabled = 'true' style="color: #e8e8e8; padding-right: 20px;font-size: 18px;" class="el-icon-arrow-left" v-else v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1"></span>
                   <!-- 切换标识 -->
                    <span style="font-size: 12px;" v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1">{{fileIndex+1}} / {{!paramlist[infoIndex]?1:paramlist[infoIndex].files.length}}</span>
                   <!-- 右切 -->
                   <a style="padding-left: 20px;font-size: 18px;" class="el-icon-arrow-right" v-if="paramlist[infoIndex] && (fileIndex < paramlist[infoIndex].files.length-1)" v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1"  @click="right"></a>
                <span disabled = 'true' style="color: #e8e8e8;  padding-left: 20px;font-size: 18px;" class="el-icon-arrow-right" v-else v-show="paramlist[infoIndex] && paramlist[infoIndex].files.length>1"></span>
               </span>
                <!-- 工具按钮 -->
                <span style="float:right;font-size: 16px;">
                    <!-- 评论 -->
                    <a class="glyphicon glyphicon-comment" title="评论" target="_blank" href=""></a>
                    <!-- 下载 -->
                    <a class="glyphicon glyphicon-save" title="下载" target="_blank" v-if="pram.url.toLowerCase() == props_type_enum['ptrender'] && file.fileUrl != null" :href="file.fileUrl"></a>
                     <span disabled = 'true' style="color: #e8e8e8;" class="glyphicon glyphicon-save" v-else></span>
                     <!-- 预览 -->
                    <a class="el-icon-view" title="预览" target="_blank" v-if="file.url != null" :href="'/Render/PtUrl?url='+encodeURIComponent(file.url)"></a>
                    <span disabled = 'true' style="color: #e8e8e8;" class="el-icon-view" v-else></span>
                    <!-- 提交 -->
                    <a class="el-icon-upload" title="提交" target="_blank" @click="gotoFunc('/Render/Submission?param='+JSON.stringify(goParam2(pram)))"></a>
                    <!-- <a class="el-icon-upload" target="_blank" :href="'/Render/Submission?param='+JSON.stringify(pram)"></a> -->
                </span>
            </h5>
            <!-- <a style="float:right;display:inline;" href="/Render/Submition">+ </a> -->
        </div>
        <div class="panel-body" style="height:100%">
        <!-- 测试区域 -->
<!--         <img :src="file.url" />
        <gt_img :param="file">
                    </gt_img> -->


        <!-- 测试区域 -->
            <!-- loading --> <!-- losding有问题，先显示了没有成果 -->
          <div v-if="isLoading" :style="stylestr">
            <span class="el-icon-loading" style="font-size: 45px;position:absolute;left:45%;top:45%;"></span>
          </div>
          <!-- 内容 -->
          <div v-else>
            <div v-if="paramlist ==null || paramlist.length == 0">
                <h4>没有成果</h4>
            </div>
            <div v-else>
                <!-- 各模块 -->
                <!-- 要按配置输出各个模块 -->
                <template v-if="comtype==type_enum.img">
                    <gt_img :param="file">
                    </gt_img>
                </template>
                <template v-if="comtype==type_enum.rich">
                    <gt_rich :param="file">
                    </gt_rich>
                </template>
                <template v-if="comtype==type_enum.list">
                    <gt_list :param="file">
                    </gt_list>
                </template>
                <template v-if="comtype==type_enum.echart">
                    <gt_echart :param="file">
                    </gt_echart>
                </template>
                <template v-if="comtype==type_enum.gdb">
                    <gt_gdb :param="pram.param.filter">
                    </gt_gdb>
                </template>
                <template v-if="comtype==type_enum.dataset">
                    <gt_dataset :param="file">
                    </gt_dataset>
                </template>
                <template v-if="comtype==type_enum.remark">
                    <gt_remark :scoap="pram.param.filter.scoap"
                               :naturekey="pram.param.filter.naturekey"
                               :id="pram.param.filter.id"
                               :classname="pram.param.filter.classname"
                               :style="pram.param.filter.style"
                               :hidden="pram.param.filter.hidden">
                    </gt_remark>
                </template>

<!--                 <template v-if="comtype==type_enum.many">
                    <gt_many :param="file">
                    </gt_many>
                </template> -->
                <template v-if="comtype==type_enum.piechart">
                    <gt_piechart :param="pram.param.filter">
                    </gt_piechart>
                </template>
                <template v-if="comtype==type_enum.histogram">
                    <gt_histogram :param="pram.param.filter">
                    </gt_histogram>
                </template>
                <template v-if="comtype==type_enum.linechart">
                    <gt_linechart :param="pram.param.filter">
                    </gt_linechart>
                </template>
                <!-- 各模块 -->
            </div>
          </div>
        </div>
        <!-- 成果切换 -->
        <div v-if="paramlist.length > 1">
            <el-pagination
              small
              layout="prev, pager, next"
              :page-size="1"
              :total="paramlist.length"
              :current-page="infoIndex+1"
              :page-count="paramlist.length"
              @current-change="infoClick">
            </el-pagination>
        </div>
        <!-- 通用模态框 -->
        <el-dialog
            :title="pram.title+' - '+(file.name || '未命名')"
            v-model="isGotoModel"
            top="12%"
            size="small">
          <div class="embed-responsive embed-responsive-16by9">
            <iframe class="embed-responsive-item" :src="gotoUrl"></iframe>
          </div>
        </el-dialog>
    </div>
</template>

<script>
    //import  models from '../../utils/getCom'
    import ajax from '../../utils/pollAjax'
    import ao from "../../utils/arrayandobjectoperation"
    import {Dialog,Pagination} from 'element-ui'
    import model from "../../utils/loadmodel"
    var renderModel = {
        rich: require('../formatcom/gtrich.vue'),
        img: require('../formatcom/gtimg.vue'),
        gdb: require('../formatcom/gtgdb.vue'),
        chart: require('../formatCom/gtpiechart.vue'),
        dataset: require('../ormatCom/gtdataset.vue')
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
                type: Number
            }
        },
        data: function () {
            return {
                paramlist: [],
                file:{},
                infoIndex:0,
                fileIndex:0,
                metadataDefinition:[],
                isLoading:false,
                gotoUrl:"",
                isGotoModel:false,
                comtype:"",
                type_enum: {
                    rich: "rich", //富文本
                    img: "img",   //图片
                    list: "list",      //列表
                    echart: "echart",    //echart 图表
                    gdb: "gdb",     //gdb格式
                    dataset: "dataset",  //数据表
                    remark: "remark",    //评论
                    nofun: "nofun",   //无法显示格式
                    piechart: "piechart",//饼状图
                    linechart: "linechart",//折线图
                    histogram: "histogram"//柱状图
                },
                props_type_enum: {
                    ptrender: "/render/pt",
                    ptlist: "/render/ptlist",
                    // 评论可能不需要作为使用类型，一般是作为其他模块的辅助功能
                    remark: "/render/remark",
                    url: ""
                },
                stylestr:{
                    height: this.heights == null?"100%":this.heights+'px'
                }
            }
        },
        created:function(){
            this.$options.components["render"] = renderModel[model.getConfig(file.format,"ptrender")];
        },
        mounted: function () {
            //ptrender加载时获取配置
            //this.getParam();
            this.init();
        },
        methods: {
            gotoFunc:function(url){
                this.gotoUrl = url;
                this.isGotoModel = true;
            },
            infoClick:function(index){
                this.infoIndex = index-1;
                this.fileIndex = 0;
                this.setFile(this.infoIndex,this.fileIndex);
                this.JudgeFormat(this.paramlist[this.infoIndex].files[this.fileIndex].format);
            },
            left:function(){
                if(this.fileIndex == 0){
                    gtUI.message({message: "前面没有了！",type: 'info'});
                    return;
                }
                this.fileIndex--;
                this.setFile(this.infoIndex,this.fileIndex);
                this.JudgeFormat(this.paramlist[this.infoIndex].files[this.fileIndex].format);
            },
            right:function(){
                if(this.fileIndex == this.paramlist[this.infoIndex].files.length-1){
                    gtUI.message({message: "后面没有了！",type: 'info'});
                    return;
                }
                this.fileIndex++;
                this.setFile(this.infoIndex,this.fileIndex);
                this.JudgeFormat(this.paramlist[this.infoIndex].files[this.fileIndex].format);
            },
            setFile:function(infoIndex,fileIndex){
                this.file={
                    url: "/FileInfo/GetStream?url="+this.paramlist[infoIndex].url+"&ticket="+this.paramlist[infoIndex].files[fileIndex].ticket+"&format="+this.paramlist[infoIndex].files[fileIndex].format,
                    format:this.paramlist[infoIndex].files[fileIndex].format,
                    heights: this.heights,
                    fileUrl: "/FileInfo/DownLoad?url="+this.paramlist[infoIndex].url+"&ticket="+this.paramlist[infoIndex].files[fileIndex].ticket+"&format="+this.paramlist[infoIndex].files[fileIndex].format,
                    name:this.paramlist[infoIndex].files[fileIndex].name
                };
            },
            //初始方法， param数据改变时或者第一次进来是调用
            init:function () {
                this.paramlist = [];
                this.infoIndex=0;
                this.fileIndex=0;
                this.file={};
                //let tag=document.getElementById("gtbody");
              // loadingInstance = gtUI.loading.service({text:"拼命加载中",target:tag});

                //对应成果展示
                if (this.pram.url.toLowerCase() == this.props_type_enum["ptrender"]) {
                    //调用后台
                    //this.Match();

                    this.getData();
                    return;
                }
                //对应列表
                else if (this.pram.url.toLowerCase() == this.props_type_enum["ptlist"]) {
                    this.comtype = this.type_enum.list;
                    // this.file={};
                    // this.file=this.goParam();
                    return;
                }
                //不存在的类型，应该在成果展示中
//                 if (this.pram.param.viewtype.toLowerCase() == "echart") {
// //                    this.paramlist.push({"heights": this.heights - 20});
//                       this.comtype = this.type_enum.echart;
//                     return;
//                 }
                //不存在的类型，应该在成果展示中
                // if (this.pram.param.viewtype.toLowerCase() == "gdb") {
                //     //调用后台
                //     //this.Match();
                //     //this.getData();
                //     this.comtype = this.type_enum.gdb;
                //     return;
                // }
                //对应评论类型
                else if (this.pram.url.toLowerCase() == this.props_type_enum["remark"]) {
                    this.comtype = this.type_enum.remark;
                    return;
                }
                // 对应url
                else {
                    this.comtype = this.type_enum.rich;
                    // this.file = {};
                    // this.file={"url": this.pram.url, "heights": this.heights};
                    return;
                }
                //不存在的类型，应该在成果展示中
                // if (this.pram.param.viewtype.toLowerCase() == "piechart") {
                //     this.comtype = this.type_enum.piechart;
                //     return;
                // }
                //不存在的类型，应该在成果展示中
                // if (this.pram.param.viewtype.toLowerCase() == "linechart") {
                //     this.comtype = this.type_enum.linechart;
                //     return;
                // }
                //不存在的类型，应该在成果展示中
                // if (this.pram.param.viewtype.toLowerCase() == "histogram") {
                //     this.comtype = this.type_enum.histogram;
                //     return;
                // }
            },
            getParam: function(){
                //metadataDefinition
                let _this = this;
                ajax({
                    url: "/SearchService/GetMetadataDefinition",
                    type: "post",
                    async: false,
                    success: function (result) {
                        _this.metadataDefinition = result;
                    }
                });
            },
            //转化参数，转换成match方法需要的kmd参数，并且替换全局变量。返回一个替换后的备份，不修改原始参数
            goParam: function () {
                if (!this.pram.param || !this.pram.param.filter) return this.pram.param;
                var global = this.global || {};
                // let pa = this.pram.param;
                let pa = ao.deepClone(this.pram.param);
                if(this.metadataDefinition.length==0) this.getParam();
                let result = this.metadataDefinition;
                        //pDic = result;
                        //按层次级别遍历，不区分$and或者$or，保留一定扩展性.
                        for (let i in pa.filter) {
                            let ssParam = [];
                            for (let j = 0; j < pa.filter[i].length; j++) {
                                //这层默认只有一条对象，这里要做替换处理，所以不太好处理太复杂的逻辑。
                                for (let z in pa.filter[i][j]) {
                                    for (let k = 0; k < result.length; k++) {
                                        if (result[k].name == z) {
                                            for (let s = 0; s < result[k].mapping.set.length; s++) {
                                                let sParam = {};
                                                //sParam[]
                                                //for (let l in result[k].mapping.set[s]) {
                                                let _key = result[k].mapping.set[s].key.replace(/\[.*\]/g, "");
                                                let _value;
                                                if (result[k].mapping.set[s].value.substring(0, 1) == "@") {
                                                    if (result[k].mapping.set[s].value == "@value") {
                                                        if (pa.filter[i][j][z].substring(0, 1) == "@") {
                                                            _value = {"$eq": global[pa.filter[i][j][z].substring(1, pa.filter[i][j][z].length)]};
                                                        } else {
                                                            _value = {"$eq": pa.filter[i][j][z]};
                                                        }
                                                    }
                                                } else {
                                                    _value = {"$eq": result[k].mapping.set[s].value};
                                                }
                                                sParam[_key] = _value;
                                                ssParam.push(sParam);
                                                //}
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            pa.filter[i] = ssParam;
                        }
                return pa;
            },
            //替换全局变量，用于上传。返回一个替换后的备份，不修改原始参数。
            goParam2: function () {
                if (!this.pram.param || !this.pram.param.filter) return this.pram.param;
                var global = this.global || {};
                // let pa = this.pram.param;
                let pa = ao.deepClone(this.pram.param);
                if(this.metadataDefinition.length==0) this.getParam();
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

                this.isLoading = false;
            },
            Match: function () {
               //let pmdata = this.pram.param;
               let param = ao.deepClone(this.goParam());
               let _this = this;
               ajax({
                   url: "/SearchService/Match",
                   type: "post",
                   contentType: "application/json; charset=utf-8",
                   data: JSON.stringify(param),
                   async:false,
                   success: function (result) {
                       for (let i = 0; i < result.metadatas.length; i++) {
                           _this.Retrieve(result.metadatas[i]["source"]["url"]);
                       }
                   }
               });
           },
            Retrieve: function (urlstr) {
               // this.paramlist = [];
               let filesPa = [];
               let _this = this;
               ajax({
                   url: "/DataService/Retrieve",
                   type: "get",
                   data: {url: urlstr},
                   async:false,
                   success: function (result) {
                        //组织成果和文件结构
                        for(let i=0;i<result.length;i++){
                            filesPa.push({ticket:result[i].ticket,format:result[i].format.toLowerCase(),name:result[i].name});
                        }
                        _this.paramlist.push({url:urlstr,files:filesPa,fileCount:result.length});
                        if(_this.paramlist.length==1 && _this.paramlist[0].files.length >= 1){
                            _this.setFile(0,0);
                            _this.JudgeFormat(_this.paramlist[0].files[0].format);
                        }
                   }
               });
           },
            //判断文档转换后的方法
            JudgeFormat: function (format) {
                var _this = this;
                var imgArray = ["jpg", "png", "jpeg", "gif", "tif", "bmp", "tiff", "ico"];
                var htmlArray = ["html", "htm", "txt", "ppt", "pptx", "pdf", "xls", "xlsx", "doc", "docx"];

                for (let i = 0; i < imgArray.length; i++) {
                    if (imgArray[i] == format) {
                        _this.comtype = _this.type_enum.img;
                        return;
                    }
                }
                for (let i = 0; i < htmlArray.length; i++) {
                    if (htmlArray[i] == format) {
                        _this.comtype = _this.type_enum.rich;
                        return;
                    }
                }
                if (format == "gdb") {
                    _this.comtype = _this.type_enum.gdb;
                    return;
                }
                else if (format == "datatable") {
                    _this.comtype = _this.type_enum.dataset;
                    return;
                }
                else {
                    _this.comtype = _this.type_enum.nofun;
                    return;//"未识别格式";
                }
            }
        },
        watch:{
            pram: function () {
                this.init();
            }
        },
        components: {
            "el-dialog": Dialog,
            "el-pagination": Pagination,
            // gt_img: models.imgcom,
            // gt_rich: models.richcom,
            // gt_list: models.listcom,
            // gt_echart: models.echartcom,
            // gt_dataset: models.datasetcom,
            // gt_remark: models.gtremark,
            // gt_piechart: models.gtpiechart,
            // gt_linechart: models.gtlinechart,
            // gt_histogram: models.gthistogram,
            // gt_gdb: models.gtgdb
        }
    }
</script>
<style scoped>
.text-overflow{
    width:40%;
/*    height:40%;
    border:1px solid red;
    border-top:4px solid red;
    padding:3px;*/
    overflow:hidden;;/* 内容超出宽度时隐藏超出部分的内容 */
    text-overflow:ellipsis;;/* 当对象内文本溢出时显示省略标记(...) ；需与overflow:hidden;一起使用。*/
    white-space:nowrap;/* 不换行 */
}
</style>


