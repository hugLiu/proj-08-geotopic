/** 
* component html and its base property
*/
WidgetLayout.TableHtml = function () {
    this.BuildContent = function () {
        var content = '';
        content += '<p class="alert alert-info lycomponent-init-info">';
        content += '表格来源未设置';
        content += '</p>';
        content += '<div class="table-responsive">';
        content += '<div class="lytable-info">';
        content += '<table id="lytable" class="table table-striped table-bordered table-hover lytable" width="100%">';
        content += '</table>';
        content += '</div>';
        content += '</div>';
        return content;
    };

    this.Type = function () {
        return "Table";
    };

    this.TypeName = function () {
        return "表格";
    };

    this.IcoUrl = function () {
        return "../content/Images/LayoutWidget/table.png";
    };
};

WidgetLayout.Table = function (obj) {
    var thisObj = obj
    var param;
    var columnName = "";
    var thisTable;

    /**
    * set parameter
    * @public
    */
    this.SetParameter = function (newParameter) {
        param = newParameter;
    };

    this.Load = function (newParameter) {
        loadTable(newParameter);
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
                    loadTable(param);
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

    function getDBTable(obj, tableName) {
        $.ajax({
            //url: "/System/GetOrgNameList",  //Walt add   --20160923
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

    function getDBTableField(obj, tableName, fieldName) {
        var parObj = {};
        parObj.parameter = tableName;
        $.ajax({
            type: "post",
            //url: "/TopicLayout/GetTextData",         //Walt add   --20160923
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

    function loadTable(newParameter) {
        param = newParameter;
        var parObj = {};
        parObj.parameter = newParameter;

        $.ajax({
            type: "get",
            //url: "/Home/GetTableColumn",       //Walt add   --20160923
            data: JSON.stringify(parObj),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                if (data != "") {
                    columnName = data;
                    createTable();
                }
            },
            error: function () {
                //alert("系统发生异常，请稍后再试。");
            }
        });
    }

    /**
    * create table 
    */
    function createTable() {
        // destory old table, then create new table
        if (thisTable) {
            thisTable.clear();
            thisTable.destroy();
            //$('#new-table1 thead tr th').remove();
        }
        buildTable();
    }

    function buildTable() {
        var id = thisObj.find(".lytable")[0].id;
        thisTable = $("#" + id).DataTable({
            //  "bServerSide": true,
            "ajax": "/Home/GetTableData",
            "oLanguage": {
                "sLengthMenu": "每页显示 _MENU_条",
                "sZeroRecords": "没有找到符合条件的数据",
                "sInfo": "当前第 _START_ - _END_ 条　共计 _TOTAL_ 条",
                "sInfoEmpty": "没有记录",
                "sInfoFiltered": "(从 _MAX_ 条记录中过滤)",
                "sSearch": "搜索：",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "前一页",
                    "sNext": "后一页",
                    "sLast": "尾页"
                }
            },

            "aoColumnDefs": buildColumns()
        });
    }

    /**
    * create table field name
    */
    function buildColumns() {
        col = columnName.split(",");
        var title = [];
        for (var i = 0; i < col.length; i++) {
            title[i] = { "sTitle": col[i], "targets": i };
        }
        return title;
    }
};