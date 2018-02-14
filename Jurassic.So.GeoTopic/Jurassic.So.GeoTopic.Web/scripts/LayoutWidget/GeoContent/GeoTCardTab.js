/*
Author : Walt
Date ：20170111
实现知识卡局部页面操作
*/

mini.parse();
var tabs = mini.get("tabsLayout");
var _nodeId;
var data;

var index = 1;

function LoadTabs(nodeId) {
    _nodeId = nodeId;
    tabs.load("/PageDetails/GetLayoutTabs?topicId=" + nodeId);
}

function onAddLayout() {
    if (typeof (_nodeId) == "undefined" || _nodeId == null) {
        toastr["info"]('请选择知识主题！');
        return;
    }

    //弹框编辑Tab页面 title OrderId
    mini.open({
        url: "/PageDetails/TCardEdit",
        title: "知识卡片信息", width: 250, height: 145,
    allowResize: false,       //尺寸调节
    allowDrag: false,         //拖拽位置
    onload: function () {
    },
    ondestroy: function (action) {
        if (action == "Ok") {
            var iframe = this.getIFrameEl();
            data = iframe.contentWindow.data;
            //应该从主题树中的选中节点传过来的参数   Walt add --20160921
            data.TopicId = _nodeId;
            var tcardJson = mini.encode(data);
            //该url应该加载可视化布局模板TCardContent
            var tab = { title: data.title, url: "/PageDetails/TCardContent?data=" + encodeURIComponent(tcardJson), showCloseButton: true };
        tabs.addTab(tab, Number(data.OrderId));
        tabs.activeTab(tab);
    }
}
});
}

//删除知识卡片信息时相应
tabs.on("beforecloseclick", function (e) {
    var sender = e.sender;
    var tab = e.tab;
    var tabId = tab.Id;

    if (confirm("确定要删除该知识卡片？")) {
        if (typeof (tabId) == "undefined")
            return;
        //后台删除数据传入后台删除数据
        $.ajax({
            url: "/PageDetails/DeleteTCard",
            data: { Id: tabId },
            type: "post",
            success: function (text) {
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseText);
            }
        });
    } else {
        e.cancel = true;
    }
});

function onPrewLayout() {
    if (_nodeId == undefined) {
        toastr["info"]("请选择知识主题！");
        return;
    }

    //弹窗展示预览成果页面
    mini.open({
        url: "/Render/card?topicId=" + _nodeId,
        title: "知识卡预览",
    width: 1050,
    height: 600,
    allowResize: true,    //尺寸调节
    allowDrag: false,  //拖拽位置
    onload: function() {
    },
    ondestroy: function(action) {
    }
});
}

//切换知识面板时加载数据
tabs.on("activechanged", function (e) {
    var sender = e.sender;
    var tab = e.tab;
    if (typeof (tab) == "undefined")
        return;
    //新增加Tab页面中的tab.Id为undefined 不查询数据库展示结果
    var tabId = tab.Id;
    if (typeof (tabId) == "undefined")
        return;
    var tCardObj = { Id: tabId, OrderId: tab.OrderId, TopicId: tab.TopicId, title: tab.title };
    var tcardJson = mini.encode(tCardObj);
    tabs.loadTab("/PageDetails/TCardContent?data=" + encodeURIComponent(tcardJson), tab);
});


//双击修改知识卡标题
tabs.on("tabdblclick", function (e) {
    var tab = e.tab;
    //弹框编辑Tab页面 title
    mini.open({
        url: "/PageDetails/TCardEdit",
        title: "编辑知识卡", width: 250, height: 145,
    allowResize: false,    //尺寸调节
    allowDrag: false,  //拖拽位置
    onload: function () {
        var iframe = this.getIFrameEl();
        //传入的数据
        var data = { title: tab.title, OrderId:tab.OrderId };
        //调用弹出页面方法进行初始化
        iframe.contentWindow.setData(data);
    },
    ondestroy: function (action) {
        if (action == "Ok") {
            var iframe = this.getIFrameEl();
            data = iframe.contentWindow.data;
            tab.TopicId = _nodeId;
            //及时加载数据库当前的TCard记录
            tab.title = data.title;
            tab.OrderId = data.OrderId;
            tabs.updateTab(tab, Number(tab.OrderId));
            //TCard节点信息  title OrderId
            var tabData = { Id: tab.Id, OrderId: tab.OrderId, TopicId: tab.TopicId, title: tab.title }
            var tCardInfo = mini.encode(tabData);
            //应该从主题树中的选中节点传过来的参数   Walt add --20160921
            $.ajax({
                url: "/PageDetails/UpdateGeoTCard",
                data: { data: encodeURIComponent(tCardInfo)},
                type: "post",
                success: function (text) {
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(jqXHR.responseText);
                }
            });
        }
    }
});
});
