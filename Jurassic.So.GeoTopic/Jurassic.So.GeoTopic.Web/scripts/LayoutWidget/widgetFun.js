var WidgetLayout = { REVISION: '01' }

/**
* entrance  
* process: construct modalbody dom , then build container dom , build components dom, last load componets data
*/
WidgetLayout.WidgetFun = function (div) {
    var _layoutDiv = div;
    var _toolBar;
    var _container;
    var _map;
    var _maxFlag=0;

    /**
    * construct modalbody dom and generate instance of toolbar class & container class
    */
    function constructor() {
        buildModalBodyHtml();

        _map = new WidgetLayout.Map();
        _toolBar = new WidgetLayout.Toolbar(".lytoolbar");
        _container = new WidgetLayout.Container();
        

    }

    function buildModalBodyHtml() {
        var content = '';
        content += '<div class="container">';
        content += '<div class="row lytoolbar" >';
        content += '</div>';
        content += '</div>';
        $(_layoutDiv).html(content);
    }

    constructor();

    /**
    * build container html string 
    */
    function buildContainerHtml() {
        var content = '';
        content += '<div class="row lyboxcontainer" style="overflow-y:auto;width:650px;height:260px;">';
        content += '</div>';
        $(content).insertAfter($(".lytoolbar"));
    };




    /**
    * build container dom 
    * @public
    */
    this.BuildContainer = function () {
        if ($(".lyboxcontainer").length < 1) {
            buildContainerHtml();
        }
        _container.BuildHtml(".lyboxcontainer");
    };

    /**
    * register toolbar event & container event
    * @public
    */
    this.Initialization = function () {
        _map.Initialization();
        _toolBar.Initialization(_map, _container);
        _container.Initialization(_map);
    };

    /**
    * Load topic by pageId and it's domId 
    * @param {string} nodeId as domId
    * @public
    */
    this.Load = function (nodeId,id) {
        var topic = {};
        topic.id = id;
        $.ajax({
            async:false,
            type: "post",
            url: "/PageDetails/GetKCardLayout",
            data: JSON.stringify(topic),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {

                if (data != "null") {
                    var maxFlag=0;
                    var arrList = JSON.parse(data);
                    //获取数据库中存在的参数列表
                    var compArr = arrList.component;

                    for (var i = 0; i < compArr.length; i++) {
                        var flagId = compArr[i].param.id; //string Gn
                        var countId = Number(flagId.substr(1)); //int n
                        if (maxFlag < countId)
                            maxFlag = countId;
                    }
                    //获取组件标志最大值
                    _maxFlag = maxFlag;

                    //加载已有参数可视化组件    Walt add --20161020
                    _toolBar.LoadComponenBoxtHtml(compArr);

                    //debugger;
                    var obj = $("#" + nodeId);
                    if (nodeId == "lycontainer") {
                        //obj.html($("#ly-header").prop("outerHTML") + _container.LoadContainerHtml(arrList));

                        var htmlResult = _container.LoadContainerHtml(arrList);
                        obj.html($("#ly-header").prop("outerHTML") + htmlResult);
                    } else {
                        obj.html(_container.LoadContainerHtml(arrList));
                    }

                    //初次加载容器中参数组件的Html
                    _container.LoadComponentHtml(obj, arrList);

                    //asynchronous load components by dom and it's parameters
                    _container.LoadComponent(obj, arrList);
                }
            }
        });
    };




    //Walt add --新建样式布局

    //this.Load = function (nodeId,cellInfoArr) {
    //    //加载已有参数可视化组件    Walt add --20161109
    //    _toolBar.LoadComponenBoxtHtml(cellInfoArr);
    //    //var obj = $("#" + nodeId);
    //    //if (nodeId == "lycontainer") {
    //    //    obj.html($("#ly-header").prop("outerHTML") + _container.LoadContainerHtml(null));
    //    //} else {
    //    //    obj.html(_container.LoadContainerHtml(null));
    //    //}
    //    //_container.LoadComponentHtml(obj, null);
    //    ////asynchronous load components by dom and it's parameters
    //    //_container.LoadComponent(obj, null);
    //};


    this.GetMaxFlag = function () {
        return _maxFlag;
    }

    /**
    * Get save event result, response result    
    * @param {string} modalId       
    * @param {string} nodeId  as domId
    * @public
    */
    this.Save = function (modalId, nodeId, data) {
        //var isFlag = _container.Save(data);
        //if (isFlag) {
        //    $.gritter.add({
        //        title: '提示信息：',
        //        text: '设置成功',
        //        sticky: false,
        //        time: 3000,
        //        class_name: 'gritter-success',
        //    });
        //    $("#" + modalId).modal("hide");
        //    this.Load(nodeId);
        //}
        //else {
        //    $.gritter.add({
        //        title: '提示信息：',
        //        text: '设置失败，请稍后再试',
        //        sticky: false,
        //        time: 3000,
        //        class_name: 'gritter-error',
        //    });
        //}

       _container.Save(data);

    };
};