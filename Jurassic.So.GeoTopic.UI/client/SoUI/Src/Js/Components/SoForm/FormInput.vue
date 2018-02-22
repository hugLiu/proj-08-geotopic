<template>
    <div>
        <!--日期-->
        <template v-if="data.uitype.toLowerCase()==type_menu.date">
            <div class="col-xs-2">
                <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
              </label></div>
            <div class="col-xs-4" style="margin-bottom:5px">
<datepic v-model="inputText" size="small" type="date" placeholder="选择日期">
</datepic>
<!--<input type="date" class="form-control" v-bind:id="inputid" v-model="inputText">-->
</div>
</template>

<!--日期时间-->
<template v-if="data.uitype.toLowerCase()==type_menu.datetime">
    <div class="col-xs-2">
        <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
              </label></div>
    <div class="col-xs-4" style="margin-bottom:5px">
<datepic v-model="inputText" type="datetime" size="small" placeholder="选择日期时间">
</datepic>
<!--<input type="datetime-local" class="form-control" v-bind:id="inputid" v-model="inputText">-->
</div>
</template>

<!--固定值 lable类型，实际上是 readonly的text-->
<template v-if="data.uitype.toLowerCase()==type_menu.lable">
    <div class="col-xs-2">
        <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
        </label>
    </div>
    <div class="col-xs-4">
        <input type="text" readonly="readonly" class="form-control" v-bind:id="inputid" v-model="inputText" @blur="InputBlur">
        </label>
    </div>
</template>

<!--下拉框-->
<template v-if="data.uitype.toLowerCase()==type_menu.dropdownlist">
    <div class="col-xs-2">
        <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
              </label></div>
    <div class="col-xs-4" style="margin-bottom:5px">
<el-select v-model="inputText" filterable clearable placeholder="请选择" size="small">
    <el-option v-for="item in data.items" :label="item.text" :value="item.value">
    </el-option>
</el-select>
<!--<select class="form-control" v-model="inputText">-->
<!--<template v-for="item in data.options">-->
<!--<option v-html="item.text"></option>-->
<!--</template>-->
<!--</select>-->
</div>
</template>

<!--文本框-->
<template v-if="data.uitype.toLowerCase()==type_menu.text">
    <div v-bind:class="inputClass">
        <div class="col-xs-2">
            <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
              </label></div>
        <div class="col-xs-4">
            <input type="text" class="form-control" v-bind:id="inputid" v-model="inputText" @blur="InputBlur">
            <template v-if="showOk">
                <span class="glyphicon glyphicon-ok form-control-feedback"></span>
            </template>
            <template v-if="showError">
                <span class="glyphicon glyphicon-remove form-control-feedback"></span>
                <span style="color:#a94442" v-html="errorStr"></span>
</template>
</div>
</div>
</template>

<!--文本域-->
<template v-if="data.uitype.toLowerCase()==type_menu.textarea">
    <div v-bind:class="inputClass">
        <div class="col-xs-2">
            <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
              </label></div>
        <div class="col-xs-4">
            <textarea class="form-control" v-bind:id="inputid" rows="2" v-model="inputText" @blur="InputBlur">
                </textarea>
            <template v-if="showOk">
                <span class="glyphicon glyphicon-ok form-control-feedback"></span>
            </template>
            <template v-if="showError">
                <span class="glyphicon glyphicon-remove form-control-feedback"></span>
                <span style="color:#a94442" v-html="errorStr"></span>
</template>

</div>
</div>
</template>
<!--数组编辑框-->
<template v-if="data.uitype.toLowerCase()==type_menu.tageditor">
    <div class="col-xs-2">
        <label v-bind:for="inputid" class="control-label" v-html="GetTitle()">
              </label></div>
    <div class="col-xs-4">
        <tag-editor :ongetvalue="GetTagValue"></tag-editor>
    </div>
</template>
</div>
</template>
<script>
    import { DatePicker, Select, Option } from 'element-ui'
    import tagEditor from '../SoTagEditor/SoTagEditor.vue'
    export default {
        props: {
            data: Object
        },
        mounted: function () {
            if (this.data.uitype.toLowerCase() == this.type_menu.lable) {
                this.inputText = this.data.value;
                this.isok = true;
            }
        },
        data: function () {
            return {
                pickerOptions0: {
                    disabledDate(time) {
                        return time.getTime() < Date.now() - 8.64e7;
                    }
                },
                title: this.data.title,
                uitype: this.data.uitype,
                isok: !(this.data.required),//判断组件是否通过验证，required为false,isok为true
                showOk: false,
                showError: false,
                inputClass: {
                    'has-feedback': true,
                    'has-error': false,
                    'has-success': false
                },
                name: this.data.name,
                inputText: "",
                errorStr: "",
                inputid: this.data.uitype + '-' + this.GetId(),
                type_menu: {
                    lable: "label",
                    text: "text",
                    textarea: "textarea",
                    date: "date",
                    datetime: "datetime",
                    dropdownlist: "dropdownlist",
                    tageditor: "tageditor"
                }
            }
        },
        methods: {
            InputBlur: function () {
                // 非空判断——>正则判断——>长度判断
                var nullTest = true;
                var lenthTest = true;
                var regTest = true;
                if (this.data.required == true && !this.inputText.trim())
                    nullTest = false;

                if (this.data.maxLength) {
                    lenthTest = this.TestLenth();
                }

                if (this.data.regexp) {
                    regTest = this.TestReg();
                }
                if (nullTest == false) {
                    this.ShowError('此项不能为空！')
                    return;
                }
                if (regTest == false) {
                    this.ShowError('不符合验证要求！')
                    return;
                }

                if (lenthTest == false) {
                    this.ShowError('字数超过要求！')
                    return;
                }
                this.ShowOk();
            },
            ShowError: function (str) {
                this.inputClass['has-error'] = true;
                this.inputClass['has-success'] = false;
                this.showError = true;
                this.showOk = false;
                this.isok = false;
                this.errorStr = '*  ' + str + '*';
            },
            ShowOk: function () {
                this.inputClass['has-error'] = false;
                this.inputClass['has-success'] = true;
                this.showError = false;
                this.errorStr = "";
                this.showOk = true;
                this.isok = true;
            },
            GetId: function () {
                var guid = "";
                for (var i = 1; i <= 32; i++) {
                    var n = Math.floor(Math.random() * 16.0).toString(16);
                    guid += n;
                    if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                        guid += "-";
                }
                return guid;
            },

            TestLenth: function () {
                if (this.inputText.trim().length > this.data.maxLength)
                    return false;
                else
                    return true;
            },

            GetTagValue: function (value) {
                this.inputText = value;
            },

            TestReg: function () {
                var reg = eval(this.data.regexp);
                return reg.test(this.inputText.trim());
            },

            GetTitle: function () {
                if (this.data.required == true) {
                    return this.data.title + "  "+"<span style=' color: rgb(187, 0, 0); font-size: 12px;font-family: \"Lucida Grande\", verdana, arial, helvetica, sans-serif;'>*</span> :";
                }
                else {
                    return this.data.title + ':';
                }
            }
        },
        components: {
            "datepic": DatePicker,
            "el-select": Select,
            "el-option": Option,
            "tag-editor": tagEditor
        }
    }

</script>

<style scoped>
/*    .control-label {
        padding-right: 0;
        margin-left: 0px;
    }

    .control-label {
        padding-left: 0;
        font-size: 14px;
        line-height: 1.75;
        margin-bottom: 1em;
    }*/
  .control-label{
       padding-top: 5px;
       float: right;
  }
    textarea {
        resize:none;
    }

    input{
        height: 27px;
        margin-bottom: 10px;
    }
</style>