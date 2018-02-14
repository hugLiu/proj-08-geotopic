/** 
* component html and its base property
*/
WidgetLayout.ChartHtml = function () {
    this.BuildContent = function () {
        //组件展示内容
        var content = '';
        content += '<p class="alert alert-info lycomponent-init-info">';
        content += '图表来源未设置';
        content += '</p>';
        return content;
    };

    this.Type = function () {
        return "Chart";
    };

    this.TypeName = function () {
        return "图表";
    };

    this.IcoUrl = function () {
        return "../content/Images/LayoutWidget/chart_bar.png";
    };
};

WidgetLayout.Chart = function (obj) {
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
        loadChart(newParameter);
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
                    loadChart(param);
                }
                return true;
            }
        });

        initSelectList(param);
        selectChange();
    };

    /**
    * setting html 
    */
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
            //url: "/System/GetOrgNameList",        //Walt add --20160923
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
                    no_results_text: "没有找到",
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
                    no_results_text: "没有找到",
                    allow_single_deselect: true
                });
            },
            error: function () {
                //alert("链接超时，请稍后再试。");
            }
        });
    }

    function loadChart(newParameter) {
        var parameter = newParameter.split(';')[0];
        var myChart = echarts.init($("#" + thisObj[0].id)[0]);
        window.onresize = myChart.resize;
        initChart(myChart, parameter);
        loadData(myChart, newParameter);
    }

    /**
    * load chart without data
    */
    function initChart(thisChart, parameter) {
        thisChart.setOption({
            title: {
                text: parameter
            },
            //  color: ['#3398DB'],
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                {
                    type: 'category',
                    data: [],
                    axisTick: {
                        alignWithLabel: true
                    }
                }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: [
                {
                    name: '直接访问',
                    type: 'line',//bar ,pie
                    barWidth: '60%',
                    data: []
                }
            ]
        });
        thisChart.showLoading();
    }

    /**
    * load chart with data
    */
    function loadData(thisChart, newParameter) {
        param = newParameter;

        var parObj = {};
        parObj.parameter = newParameter;
        $.ajax({
            type: "post",
            //url: "/TopicLayout/GetChartData",     //Walt add  --20160923
            data: JSON.stringify(parObj),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {

                thisChart.hideLoading();
                thisChart.setOption({
                    xAxis: {
                        data: data.xData
                    },
                    series: [{
                        name: '直接访问',
                        data: data.sData
                    }]
                });
            },
            error: function () {
                //alert("链接超时，请稍后再试。");
            }
        });
    }
};