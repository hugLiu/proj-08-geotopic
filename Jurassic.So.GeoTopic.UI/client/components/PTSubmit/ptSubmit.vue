<template>
    <div>
        <div style="margin:20px">
<div class="panel panel-default" :style="style">
    <div class="panel-body" style="height:auto">
        <div v-if="isForm" style="margin-bottom:5px">
<template v-for="item in formModel">
    <soform :data="item" :style="style" ref="soform"></soform>
</template>
</div>
<!--action="http://192.168.1.152:8077/API/SubmissionService/Upload" 田泽玉-->
<!--action="http://192.168.1.152:8089/API/Submission/Upload" 汪敏-->
<el-upload :action="uploadapi" type="drag" :multiple="true" :on-preview="handlePreview" :on-remove="handleRemove" :on-success="handleSuccess"
    :on-error="handleError" :default-file-list="fileList">
    <i class="el-icon-upload"></i>
    <div class="el-dragger__text">点击上传</em></div>
    <div class="el-upload__tip" slot="tip"></div>
</el-upload>
<div class="submitList">
    <button id="btnSubmit" class="btn btn-info" @click="Submit">提交</button>
    <button id="btnUpdate" class="btn btn-warning disabled" @click="Update">修改</button>
    <button id="btnDelete" class="btn btn-danger disabled" @click="Delete">删除</button>
</div>
</div>
</div>
</div>
</div>
</template>
<script>
    import soUI from '../../SoUI/soUI';
    import {Upload} from 'element-ui';
    export default {
        props: {
            uploadapi: String,
            metadataapi: String,
            style: String,
            param: Object
        },
        mounted: function () {
            this.GetDataMeta();
        },
        data: function () {
            return {
                isForm: false,
                formModel: [
                    {
                        "groupname": "",
                        "fields": [
                            {}
                        ]
                    }
                ],
                fileList: [],
                subnum: 1,
                fileInfo: {
                    action: "",
                    naturekey: "",
                    fileIDs: [],
                    formList: []
                }
            }
        },
        methods: {
            handleSuccess(result, file, fileList) {
                this.fileInfo.fileIDs = [];
                if (fileList.length > 0) {
                    for (let i in fileList) {
                        //let data = JSON.parse(fileList[i]["response"]);
                        let data = fileList[i]["response"]; //7.31
                        this.fileInfo.fileIDs.push(data.fileID);
                    }
                }
                gtUI.message({ message: "上传成功！", type: 'success' });
            },
            handleError(result) {
                gtUI.message({ message: "上传失败！", type: 'warning' });
            },
            handleRemove(file, fileList) {
                this.fileInfo.fileIDs = [];
                if (fileList.length > 0) {
                    for (let i in fileList) {
                        //let data = JSON.parse(fileList[i]["response"]);
                        let data = fileList[i]["response"]; //7.31
                        this.fileInfo.fileIDs.push(data.fileID);
                    }
                }
            },

            //            Upload: function () {
            //                alert("确定上传吗？")
            //                var _this=this;
            //                var data = new FormData();
            //                var files = $("#fileUpload").get(0).files;
            //                if (files.length > 0) {
            //                    for (var i = 0; i < files.length; i++) {
            //                        data.append(i.toString(), files[i]);
            //                    }
            //                }
            //                else{
            //                    alert("请选择文件")
            //                }
            //                $.ajax({
            //                    type: "post",
            //                    url: "http://192.168.1.152:8077/API/SubmissionService/Upload",
            //                    contentType: false,
            //                    cache: false,
            //                    currentType: false,
            //                    processData: false,
            //                    data: data,
            //                    success: function (result) {
            //                        alert(1);
            //                       var data=  JSON.parse(result);
            //                         _this.param.fileIDs.push(data.fileID);
            //                       // _this.AddHtml();
            //                    }
            //                });
            //            },
            Submit: function () {
                if (this.fileInfo.naturekey != "") return;
                var r = confirm("确定提交吗？")
                if (r == true)
                    this.PostData("submit");
                else
                    return;
            },

            Delete: function () {
                if (this.fileInfo.naturekey == "") return;
                var r = confirm("确定删除吗？")
                if (r == true)
                    this.PostData("delete");
                else
                    return;
                // _this.PostData("delete");
            },

            Update: function () {
                if (this.fileInfo.naturekey == "") return;
                var r = confirm("确定更新吗？")
                if (r == true)
                    this.PostData("update");
                else
                    return;
                //_this.PostData("update");
            },

            GetDataMeta: function () { 
                let _this = this;
                $.ajax({
                    url: _this.metadataapi, //"/API/SearchService/GetMetadataDefinition",
                    type: "post",
                    data: {},
                    success: function (result) { 
                        let groupnames = [];
                        let arrays = [];
                        let paramData = [];
                        let items = [];
                        if (_this.param)
                            paramData = _this.param["filter"]["$and"];
                        //1：替换传过来的参数，放在value属性里面
                        for (let i = 0; i < result.length; i++) {
                            if (result[i]["innertag"] == false) {
                                for (let j = 0; j < paramData.length; j++) {
                                    for (let pname in paramData[j]) {
                                        if (result[i]["name"] && pname == result[i]["name"]) {
                                            result[i]["uitype"] = "label";
                                            result[i].value = paramData[j][pname];
                                        }
                                    }
                                }
                                let temp=result[i]["groupname"]
                                if(groupnames.indexOf(temp)<0){
                                    groupnames.push(temp) //去重复 得到分组信息，形成数组
                                }

                                arrays.push(temp)
                                items.push(result[i]);
                            }
                        }

                        //2：得到分组信息，形成数组
                        //groupnames = Array.from(new Set(arrays));  //去重复
                        
                        items = _this.Sort(items, "itemorder");  //排序元数据items
                        groupnames = _this.OrderGroup(items, groupnames); //排列group
                        _this.formModel = [];
                        for (let i = 0; i < groupnames.length; i++) { 
                            _this.formModel.push({
                                "groupname": groupnames[i]["groupname"],
                                "order": groupnames[i]["order"], "fields": []
                            });
                        }

                        //3:将元数据装进不同分组的数组里面
                        for (let i = 0; i < items.length; i++) {
                            for (let j = 0; j < _this.formModel.length; j++) {
                                if (items[i]["groupname"] == _this.formModel[j]["groupname"]) {
                                    _this.formModel[j].fields.push(items[i]);
                                }
                            }
                        }
                        _this.isForm = true;
                    },
                    error: function (result) {
                        console.log(result);
                    }
                })
            },

            //冒泡排序 得到分组后的group数组
            OrderGroup: function (items, groups) {
                let order_group = [];
                for (let i = 0; i < groups.length; i++) {
                    for (let j = 0; j < items.length; j++) {
                        if (groups[i] == items[j]["groupname"]) {
                            order_group.push({ "groupname": groups[i], "order": items[j]["grouporder"] });
                            break;
                        }
                    }
                }
                return this.Sort(order_group, "order");
            },

            Sort: function (array, orderStr) {
                var temp = {};
                for (var i = 0; i < array.length; i++) {
                    for (var j = 0; j < array.length - i; j++) {
                        if (array[j] && array[j + 1]) {
                            if (array[j][orderStr] > array[j + 1][orderStr]) {
                                temp = array[j];
                                array[j] = array[j + 1];
                                array[j + 1] = temp;
                            }
                        }
                    }
                }
                return array
            },

            PostData: function (value) {
                var _this = this;
                this.fileInfo.action = value;
                var forms = _this.$refs.soform;
                var formData;
                _this.fileInfo.formList = [];
                for (let i = 0; i < forms.length; i++) {
                    formData = forms[i].getFormData();
                    for (let j = 0; j < formData.length; j++) {
                        if (formData[j].isok == false) {
                            gtUI.message({ message: '[' + formData[j].title + ']有错误！', type: 'warning' });
                            return;
                        }
                    }
                    _this.fileInfo.formList.push(forms[i].getFormData());
                }
                if(_this.fileInfo.fileIDs == null || _this.fileInfo.fileIDs.length < 1){
                        gtUI.message({ message: '请至少上传一个文件！', type: 'warning' });
                        return;
                }
                $.ajax({
                    type: "post",
                    url: "/Submit/Submited",
                    data: { param: JSON.stringify(_this.fileInfo) },
                    success: function (result) {
                        let resultObj = JSON.parse(result);
                        _this.fileInfo.naturekey = resultObj.naturekey;
                        if (_this.fileInfo.action == "submit") {
                            gtUI.message({ message: "提交成功！", type: 'success' });
                            $("#btnSubmit").addClass("disabled");
                            $("#btnUpdate").removeClass("disabled");
                            $("#btnDelete").removeClass("disabled");
                        } else if (_this.fileInfo.action == "update") {
                            gtUI.message({ message: "更新成功！", type: 'success' });
                        } else if (_this.fileInfo.action == "delete") {
                            gtUI.message({ message: "删除成功！", type: 'success' });
                            _this.fileInfo.naturekey = "";
                            $("#btnSubmit").removeClass("disabled");
                            $("#btnUpdate").addClass("disabled");
                            $("#btnDelete").addClass("disabled");
                        }
                    },
                    error: function (result) {
                        if (_this.fileInfo.action == "submit") {
                            gtUI.message({ message: "提交失败！", type: 'warning' });
                        } else if (_this.fileInfo.action == "update") {
                            gtUI.message({ message: "更新失败！", type: 'warning' });
                        } else if (_this.fileInfo.action == "delete") {
                            gtUI.message({ message: "删除失败！", type: 'warning' });
                        }
                    }
                });
            }
        },

        components: {
            "soform": soUI.soform,
            "el-upload": Upload,
        },
    }
</script>
<style scoped>
    #uploadList {
        margin: 10px;
    }

    .subadd {
        margin-left: 15px;
    }

    /*这个属性要在实例页面设置 写在这里并没有什么卵用*/
    input[type=file] {
        display: none;
    }
</style>
