/** 
* component html and its base property
*/
WidgetLayout.PictureHtml = function () {
    this.BuildContent = function () {
        var content = '';
        content += '<p class="alert alert-info lycomponent-init-info">';
        content += '图片来源未设置';
        content += '</p>'; 
        return content;
    };

    this.Type = function () {
        return "Picture";
    };

    this.TypeName = function () {
        return "图片";
    };

    this.IcoUrl = function () {
        return "../content/Images/LayoutWidget/picture.png";
    };
};

WidgetLayout.Picture = function (Obj) {
    var thisObj = Obj;
    var param;

    /**
    * set parameter
    * @public
    */
    this.SetParameter = function (newParameter) {
        param = newParameter;
    };

    this.Load = function (newParameter) {
        loadPicture(newParameter);
    };

    this.Save = function () {
        return param;
    };

    /**
    * setting event
    * @public
    */
    this.Setting = function () {
        bootbox.confirm({
            title: '数据源设置：',
            className: 'popup-confirm',
            size: 'large',
            message: settingHtml(),
            callback: function (result) {
                if (result) {
                    if (!($("#db-table").val() != "" && $("#db-field").val() != "")) {
                        $.gritter.add({
                            title: '提示信息：',
                            text: '设置未完成',
                            sticky: false,
                            time: 3000,
                            class_name: 'gritter-error',
                        });
                        return false;
                    }
                    param = $("#db-table").val() + ";" + $("#db-field").val();
                    loadPicture(param);
                }
                return true;
            }
        });

        initSelectList(param);
        selectChange();
    };

    function settingHtml() {
        var content = '';

        content += '<div class="row" style="padding:5px">'
        content += '<label class="col-sm-3 control-label" for="form-field-1">数据库表：</label>';
        content += '<div class="col-sm-9">';
        content += '<select class="width-60 chosen-select" id="db-table"></select>';
        content += '</select>';
        content += '</div>';
        content += '</div>';

        content += '<div class="row" style="padding:5px">'
        content += '<label class="col-sm-3 control-label" for="form-field-1">表字段：</label>';
        content += '<div class="col-sm-9">';
        content += '<select class="width-60 chosen-select" id="db-field"></select>';
        content += '</select>';
        content += '</div>';
        content += '</div>';

        return content;
    }

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
            //url: "/System/GetOrgNameList",   //Walt add  --20160923
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
            //url: "/TopicLayout/GetTextData",      //Walt add  --20160923
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

    /**
    * load data put into dom 
    */
    function loadPicture(newParameter) {
        param = newParameter;

        var parObj = {};
        parObj.parameter = newParameter;
        $.ajax({
            type: "post",
            //url: "/TopicLayout/GetTextData",      //Walt add  --20160923
            data: JSON.stringify(parObj),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {

                if (data != "") {
                    //var a = loadContent(data);
                    var img = '<img src="../content/Images/LayoutWidget/image.png" width="100%">';
                    $(thisObj).html(img);

                }
            },
            error: function () {
                //alert("链接超时，请稍后再试。");
            }
        });
    } 
}; 