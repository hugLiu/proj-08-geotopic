/** 
* toolbar class
* process: construct toolbar dom, then register event
*/

WidgetLayout.Toolbar = function (div) {

    var _toolbarDiv = div;
    var _map;
    var _container;
    /**
    * register selectLayout event
    * @public
    */
    this.Initialization = function (map,container) {
        _container = container;
        _map = map;
        selectLayout1_2Event();
        selectLayout2_2Event();
        
        addComponentSettingEvent();  //Walt add --20161017
        deleteComponentBoxEvent();
    };


    //
    this.ShowComponent = function (parameter) {
        return showComponent(parameter);
    }


    /**
    * construct toolbar dom 
    */
    function constructor() {
        buildToolBarHtml();
    }

    constructor();

    //加载参数列表，展示出可视化组件
    this.LoadComponenBoxtHtml= function(paramList) {
        for (var i = 0; i < paramList.length; i++) {
            //showComponent(paramList[i]);
            showComponent(paramList[i].param);
        }
    }






    /**************************************
    * html
    **************************************/
    function buildToolBarHtml() {
        //debugger;
        var content = '';
        content += '<div >';

        content += '<div id="BoxesBar" class="col-xs-8 boxes">';
        content += getBoxesBarHtml();
        content += '</div>';

        content += '<div class="col-xs-4 cells">';
        content += getCellsBarHtml();
        content += '</div>';

        //默认布局
        //content += '<div class="col-xs-4 blocks">';
        //content += getBlocksBarHtml();
        //content += '</div>';
        content += '</div>';
        $(_toolbarDiv).html(content);
    };

    function getCellsBarHtml() {
        var content = '';
        content += getLayout_Row1Html();
        content += getLayout_Row2Html();
        content += getLayout_Row3Html(); 
        return content;
    }



    //上层化组件的方法
    function getBoxesBarHtml() {
        var content = '';
        content += getLayout_BoxHtml(); 
        return content;
    }

    function getBlocksBarHtml() {
        var content = '';
        content += '<div class="pull-right">'; 
        content += getLayout_BlockHtml(); 
        content += '</div>'; 
        return content;
    }

    function getLayout_Row1Html() {
        var content = '';
        content += '<div class="lyblock lyblock-col12">';
        content += '<div class="row lyrow">';

        content += '<div class="lyrow-draggable">';
        content += '<a class="btn btn-app btn-light btn-xs drag" style="width:45px;height:45px;">';
        content += '<img src="../content/Images/LayoutWidget/column-1.png" width=25px;height=25px; />';
        content += '</a>';
        content += '<img src="../content/Images/LayoutWidget/transform_move.png" class="cell-drag" />';
        content += '</div>';

        content += '<div class="lyrow-delete">';
        content += '<a>';
        content += '<img src="../content/Images/LayoutWidget/cancel.png" />';
        content += '</a>';
        content += '</div>';

        content += '<div class="col-xs-12 lycolumn">';
        content += '</div>';

        content += '</div>';
        content += '</div>';
        return content;
    }

    function getLayout_Row2Html() {
        var content = '';
        content += '<div class="lyblock lyblock-col6">';
        content += '<div class="row lyrow">';

        content += '<div class="lyrow-draggable">';
        content += '<a class="btn btn-app btn-light btn-xs drag" style="width:45px;height:45px;">';
        content += '<img src="../content/Images/LayoutWidget/column-2.png" width=25px;height=25px;/>';
        content += '</a>';
        content += '<img src="../content/Images/LayoutWidget/transform_move.png" class="cell-drag" />';
        content += '</div>';

        content += '<div class="lyrow-delete">';
        content += '<a>';
        content += '<img src="../content/Images/LayoutWidget/cancel.png" />';
        content += '</a>';
        content += '</div>';

        content += '<div class="col-xs-6 lycolumn">';
        content += '</div>';
        content += '<div class="col-xs-6 lycolumn">';
        content += '</div>';

        content += '</div>';
        content += '</div>';
        return content;
    }

    function getLayout_Row3Html() {
        var content = '';
        //content += '<div class="lyblock lyblock-col6">';
        content += '<div class="lyblock lyblock-col4">';
        content += '<div class="row lyrow">';

        content += '<div class="lyrow-draggable">';
        content += '<a class="btn btn-app btn-light btn-xs drag" style="width:45px;height:45px;">';
        content += '<img src="../content/Images/LayoutWidget/column-3.png" width=25px;height=25px; />';
        content += '</a>';
        content += '<img src="../content/Images/LayoutWidget/transform_move.png" class="cell-drag" />';
        content += '</div>';

        content += '<div class="lyrow-delete">';
        content += '<a>';
        content += '<img src="../content/Images/LayoutWidget/cancel.png" />';
        content += '</a>';
        content += '</div>';

        content += '<div class="col-xs-4 lycolumn">';
        content += '</div>';
        content += '<div class="col-xs-4 lycolumn">';
        content += '</div>';
        content += '<div class="col-xs-4 lycolumn">';
        content += '</div>';

        content += '</div>';
        content += '</div>';
        return content;
    }

    function getLayout_BoxHtml() {


        //var comHtml = new WidgetLayout.ComponentHtml();
        //var content = "";
        //content += comHtml.BuildComponentHtml("Text");
        //content += comHtml.BuildComponentHtml("Picture");
        //content += comHtml.BuildComponentHtml("Table");
        //content += comHtml.BuildComponentHtml("Chart");
        //return content;

        var content = "";
        content += getComponentSettingHtml();
        return content;
    }

    function getComponentSettingHtml() {
        var _content = "";
        //_content += '<div class="ly-setting">';
        //_content += '<a class="mini-button lycomponent-setting lytoolbar-component">添加组件</a>';
        //_content += '</div>';
        _content += '<div class="lyblock-next">';
        _content += '</div>';
        return _content;
    }


    function getLayout_BlockHtml() {
        var content = '';
        content += '<div class="lyblock">'; 
        content += '<a href="#" class="btn btn-app btn-light btn-xs select-layout1_2" style="width:45px;height:45px;">';
        content += '<img src="../content/Images/LayoutWidget/layouts_1.png" width=25px;height=25px; />';
        content += '</a>';
        content += '</div>';
        content += '<div class="lyblock">';
        content += '<a href="#" class="btn btn-app btn-light btn-xs select-layout2_2" style="width:45px;height:45px;">';
        content += '<img src="../content/Images/LayoutWidget/layouts_2.png" width=25px;height=25px;/>';
        content += '</a>';
        content += '</div>';
        return content;
    }


    /*
    Walt add --20161017
    show the component
    */

    function showComponent(parameter) {
        //var type; /** get compoent type by parameter or other as selectlist */
        //type = "Text";
        //debugger;
        //var titleObj = {};
        //titleObj.titleBrief = parameter.id;
        //titleObj.detail = parameter.id;
        //if (parameter.title.length > 3)
        //    titleObj.titleBrief = parameter.title.substring(0, 3);
        var id = "lycomponent" + (new Date() - 0);
        loadComponentHtml(id, parameter.id, parameter.title); /** load component html */

        var map = _map;
        map.GetMap().Remove(id);
        map.GetMap().Set(id, parameter);
    }


    function deleteComponentBox(id) {
        _map.GetMap().Remove(id);
    }

    



    function loadComponentHtml(id, falgId,title) {
        var content = "";
        var comHtml = new WidgetLayout.ComponentHtml();
        content += comHtml.BuildComponentHtml(id,falgId,title);
        $(".lyblock-next").append(content);
        _container.BoxDraggableEvent();
    }


    function setting() {
        mini.open({
            url: "/PageDetails/PTempEdit",
            title: "编辑参数",
            allowResize: false,       //允许尺寸调节
            allowDrag: false,         //允许拖拽位置
            width: 700,
            height: 500,
            onload: function () {
                var iframe = this.getIFrameEl();
                //加载弹出页面模板数据
                iframe.contentWindow.LoadData();
            },
            ondestroy: function (action) {
                if (action == "Ok") {
                    var iframe = this.getIFrameEl();

                    //获取到编辑参数编辑页面的值
                    var data = iframe.contentWindow.data;
                    //设置标识符
                    count++;
                    data.id = 'G' + count.toString();

                    //保存参数
                    showComponent(data);
                }
            }
        });
    }

    



    /**************************************
    * event
    **************************************/
    function selectLayout1_2Event() {
        $(".lytoolbar").on("click", ".select-layout1_2", function () {
            $(".lycontainer").html($("#ly-header").prop("outerHTML") + $(".lyblock-col12").html() + $(".lyblock-col6").html());
            //invoke sortable event by container class 
            _container.ColumnSortableEvent();
        });
    }

    function selectLayout2_2Event() {
        $(".lytoolbar").on("click", ".select-layout2_2", function () {
            $(".lycontainer").html($("#ly-header").prop("outerHTML") + $(".lyblock-col6").html() + $(".lyblock-col6").html());
            _container.ColumnSortableEvent();
        });
    }

    function addComponentSettingEvent() {
        $(".lytoolbar").on("click", ".lycomponent-setting", function () {
            setting();
            return false;
        });
    }


    function deleteComponentBoxEvent() {
        $(".lytoolbar").on("click", ".lytoolBar-remove", function () {
            if (confirm("确定删除已配置参数组件？")) {
                var id = $(this).parents(".lyblock").find(".lycomponentbox")[0].id;
                $(this).parents(".lyblock").remove();
                deleteComponentBox(id);
            }
            return false;
        });
    }

    

};