/** 
* component html and its base property
*/

mini.parse();


WidgetLayout.TextHtml = function () {

    var _typeName;

    this.BuildContent = function () {
        var content = ''; 
        //content += '<p class="alert alert-info lycomponent-init-info">';
        //content += '文本来源未设置';
        //content += '</p>'; 
        return content;
    };


    this.Type = function () {
        return "Text";
    };

    this.setTypeName = function (typeName) {
        _typeName = typeName;
    }

    this.TypeName = function () {
        //return "组件展示";
        return _typeName;
    };

    this.IcoUrl = function () {
        return "../content/Images/LayoutWidget/cog.png";
    };
};

WidgetLayout.Text = function (obj) {
    var thisObj = obj;
    var param; 

    /**
    * set parameter
    * @public
    */
    this.SetParameter = function (newParameter) {
        param = newParameter;
    };

    this.Load = function (newParameter) {
        loadText(newParameter);
    }; 
     
    this.Save = function () {
        return param;
    };

    /**
    * setting event
    * @public
    */
    this.Setting = function () {

        //Walt add --20160926
        mini.open({
            url: "/PageDetails/PTempEdit",
            title: "编辑参数",
            allowResize: false,       //允许尺寸调节
            allowDrag: false,         //允许拖拽位置
            width: 700,
            height: 500,
            onload: function () {
                var iframe = this.getIFrameEl();
                //传入的数据待定
                var data = param;
                //调用弹出页面方法进行初始化
                iframe.contentWindow.SetData(data);
            },
            ondestroy: function (action) {
                if (action == "Ok") {
                    var iframe = this.getIFrameEl();

                    //获取到编辑参数编辑页面的值
                    var data = iframe.contentWindow.data;
                    loadText(data); 
                }
            }
        });

    };

    /**
    * setting html 
    */
    function settingHtml() {
        var content = '';

        content += '<div class="row" style="padding:5px">';
        content += '<label class="col-sm-3 control-label" for="form-field-1">数据库表：</label>';
        content += '<div class="col-sm-9">';
        content += '<select class="width-60 chosen-select" id="db-table"></select>';
        content += '</select>';
        content += '</div>';
        content += '</div>';

        content += '<div class="row" style="padding:5px">';
        content += '<label class="col-sm-3 control-label" for="form-field-1">表字段：</label>';
        content += '<div class="col-sm-9">';
        content += '<select class="width-60 chosen-select" id="db-field"></select>';
        content += '</select>';
        content += '</div>';
        content += '</div>';

        return content;
    }

    /**
    * load data put into dom 
    */
    function loadText(newParameter) {
        param = newParameter;
        //var parameter = JSON.stringify(newParameter);
        //var paraStr = parameter.replace(/\\/g, '');
        //$(thisObj).html(param.id);   //Walt add --20160919
    }

    //加载编辑项数据
    function initSelectList(param) {
        var pValue = new Array();
        if (typeof param !== "undefined") {
            pValue = param.split(';');
        }
        getDBTable($("#db-table"), pValue[0]);
        getDBTableField($("#db-field"), pValue[0], pValue[1]);
    }

    /**
    * select change event
    */
    function selectChange() {
        $("#db-table").chosen().change(function () { 
            getDBTableField($("#db-field"), $(this).val());
        });
    }

    /**
    * get bd table name
    */
    function getDBTable(obj, tableName) {
        $.ajax({
            //url: "/System/GetOrgNameList",        //Walt add --20160919
            type: "get",
            dataType: "json",
            success: function (result) {
                var data = eval(result).AppendData;
                var message = eval(result).Message;
                var content = '<option value="">&nbsp;</option>';
                for (var i = 0; i < data.length; i++) {
                    content += "<option value='" + data[i] + "'>" + data[i] + "</option>";
                }
                obj.html(content);
                obj.val(tableName);
                obj.trigger("chosen:updated");
                obj.chosen({
                    max_selected_options: 10,
                    no_results_text: "没有找到",//message,
                    allow_single_deselect: true
                });
            },
            error: function () {
                //alert("链接超时，请稍后再试。");
            }
        });
    }

    /**
    * get bd table field name
    */
    function getDBTableField(obj, tableName, fieldName) {
        var parObj = {};
        parObj.parameter = tableName;
        $.ajax({
            type: "post",
            //url: "/TopicLayout/GetTextData",      //Walt add --20160919
            data: JSON.stringify(parObj),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var content = '<option value="">&nbsp;</option>';
                if (data.indexOf("总公司") > -1) {

                    content += "<option value='CID'>CID</option>";
                    content += "<option value='COMPANYNAME'> COMPANYNAME</option>";
                }
                if (data.indexOf("工程局") > -1) {
                    content += "<option value='GID'>GID</option>";
                    content += "<option value='GARNAME'>GARNAME</option>";
                }

                obj.html(content);
                obj.val(fieldName);
                obj.trigger("chosen:updated");
                obj.chosen({
                    max_selected_options: 10,
                    no_results_text: "没有找到",//message,
                    allow_single_deselect: true
                });
            },
            error: function () {
                //alert("链接超时，请稍后再试。");
            }
        });
    }
};