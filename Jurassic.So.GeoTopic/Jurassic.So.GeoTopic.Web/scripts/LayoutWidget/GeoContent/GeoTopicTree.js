/*
Author : Walt
Date ：20170111
实现主题树局部页面操作
*/

mini.parse();
var treeKTopic = mini.get("treeKTopic");
var gridIndexDefinition = mini.get("indexGrid");
var IndexDefinitionEditor = mini.get("IndexDefinitionEditor");
var currentNodeId = 0;

treeKTopic.on("nodeselect", function(e) {
    if (e) {
        gridIndexDefinition.load({ TopicId: e.node.Id });
        currentNodeId = e.node.Id;
        LoadTabs(currentNodeId);
    }
});

//增加同级节点
function onAdd() {
    var node = treeKTopic.getSelectedNode();
    if (node != null) {
        expandNodeFunc(node);
    }
    var newNode = { Title: "新建主题" };
    treeKTopic.addNode(newNode, "after", node);
    treeKTopic.beginEdit(newNode);

}

function expandNodeFunc(node) {
    var parentNode = treeKTopic.getParentNode(node);
    if (parentNode != null) {
        if (!treeKTopic.isExpandedNode(parentNode)) {
            treeKTopic.expandNode(parentNode);
        }
        expandNodeFunc(parentNode);
    }
}
    

//增加子节点
function onAddChild() {
    var node = treeKTopic.getSelectedNode();
    if (node != null) {
        expandNodeFunc(node);
        if (!treeKTopic.isExpandedNode(node)) {
            treeKTopic.expandNode(node);
        }
    }
    var newNode = { Title: "新建主题" };
    treeKTopic.addNode(newNode, "add", node);
    treeKTopic.beginEdit(newNode);
}

//编辑节点
function onEdit() {
    var node = treeKTopic.getSelectedNode();
    console.log(node);
    treeKTopic.beginEdit(node);
}

function endEdit(e) {
    console.log(e.node);
    var node = e.node;
    if (node != null) {
        if (node.Title == "") {
            node.Title = "新建主题";
        }
        if ("Id" in node) {
            console.log("Id in node");
            console.log(node);
            SaveData();
        } else {
            console.log("new node");
            console.log(node);
            NewNodeSaveData(node);
        }
            
    }
}

function NewNodeSaveData(node) {
    var selectedNode = treeKTopic.getSelectedNode();
    var data = treeKTopic.getChanges();
    var nodeParams = mini.encode(data);
    var msgid = mini.loading("数据保存中，请稍后......", "保存数据");
    $.ajax({
        url: "/PageDetails/SaveNewNode?=",
        data: { nodeParams: encodeURIComponent(nodeParams) },
        type: "post",
        async: false,
        success: function (text) {
            //treeKTopic.reload();
            console.log(text);
            mini.hideMessageBox(msgid);
            node.Id = text;
            treeKTopic.accept();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            treeKTopic.reload();
            treeKTopic.selectNode(selectedNode);
            treeKTopic.scrollIntoView(selectedNode);
        }
    });
}

function getTreeNodeList() {
    var treeNodeList = treeKTopic.getList();
    treeNodeList.forEach(function(e) {
        e.fullPath = getNodeFullPath(e);
    });
    return treeNodeList;
}

//删除节点数据
function onRemove() {
    var node = treeKTopic.getSelectedNode();

    if (node) {
        if (!treeKTopic.isLeaf(node)) {
            if (confirm("确定删除选中主题节点及其所有子节点?")) {
                tabs.removeAll();
                gridIndexDefinition.clearRows();
                //获取所有子节点集合
                var nodeAllChild = treeKTopic.getAllChildNodes(node);
                //考虑从叶子节点到根节点方式删除，由于Id--Pid存在外键关联关系
                treeKTopic.removeNodes(nodeAllChild.reverse());
                SaveData();
                treeKTopic.removeNode(node);
                SaveData();
            }
            return;
        }
        if (confirm("确定删除选中主题节点?")) {
            //清除主题联动知识卡信息   清除索引信息列表
            tabs.removeAll();
            gridIndexDefinition.clearRows();
            treeKTopic.removeNode(node);
            SaveData();
        }
    }
}

//导入数据设置窗体
function onImport() {
    var importData;
    mini.open({
        url: "/PageDetails/ImportParams",
        title: "数据导入设置",
        allowResize: false, //允许尺寸调节
        allowDrag: false, //允许拖拽位置
        width: 425,
        height: 320,
        showModal: true,
        onload: function() {
        },
        ondestroy: function (action) {
            if (action == "Ok") {
                var iframe = this.getIFrameEl();
                //获取导入知识模型模板参数是否存在导入知识链数据
                importData = iframe.contentWindow._importData;
                if (typeof (importData) == "undefined")
                    return;
                var msgid = mini.loading("数据导入中，请稍后......", "导入数据");
                //延迟处理，等待窗体关闭后执行
                setTimeout(function () {
                    //判断知识树数据的导入方式
                    if (importData.mode == 1) {
                        //追加方式
                        importData.excelEntity.forEach(function(e) {
                            importModelData(e);
                        });
                    } else {
                        //覆盖方式  删除原知识链数据，添加导入模型数据
                        var treeNodes = treeKTopic.getList();
                        treeKTopic.removeNodes(treeNodes.reverse());
                        SaveData();
                        importData.excelEntity.forEach(function(e) {
                            importModelData(e);
                        });
                    }
                    mini.hideMessageBox(msgid);
                }, 1000);
            }
        }
    });
}

//导入数据参数模型数据插入同步数据库关系表
function importModelData(excelModel) {
    //添加层级树数据   //导入数据的主题全路径 
    var fullPathArr = getNodesPathByNodeFullPath(excelModel.TopicPathTitle);

    fullPathArr.forEach(function(e) {
        var newNode = getNodeByFullPath(e);
        //如果添加的是根节点
        if (newNode.prentFullpath.length <= 0) {
            var rootNodes = treeKTopic.findNodes(function(node) {
                if (getNodeFullPath(node) == newNode.Text)
                    return true;
            });
            //如果不存在根节点
            if (rootNodes.length <= 0) {
                //则树跟结点添加新增节点
                treeKTopic.addNode({ Title: newNode.Text }, "after");
                SaveData();
                treeKTopic.reload();
            }
        } else {
            //查找知识树中是否存在全路径节点(先查节点全路径，再看节点父路径)
            var nodes = treeKTopic.findNodes(function(node) {
                if (getNodeFullPath(node) == e)
                    return true;
            });
            //当不存在全路径节点时
            if (nodes.length <= 0) {
                //找到知识树中存在父路径的节点
                var selectNodes = treeKTopic.findNodes(function(node) {
                    if (getNodeFullPath(node) == newNode.prentFullpath)
                        return true;
                });
                //创建连接子节点
                treeKTopic.addNode({ Title: newNode.Text }, "add", selectNodes[0]);
                SaveData();
                treeKTopic.reload();
            }
        }
        //保存导入主题节点参数模型信息   （同上操作一样）
        if (excelModel.TopicPathTitle == e) {
            //获取当前的知识节点值   、知识卡、知识格子信息
            var currentNodes = treeKTopic.findNodes(function (node) {
                if (getNodeFullPath(node) == excelModel.TopicPathTitle)
                    return true;
            });
            //保存主题索引
            //保存知识卡  知识格子信息  知识参数信息
            SavaImportDataEvnet(excelModel, currentNodes[0].Id);
        }
    });
}


//响应数据导出到Excel文件中
function onExport() {

    if (!confirm("确定将整个知识链数据导出到知识模型模板Excel表？")) {
        return;
    }
    //可以相应链接获取文件
    var treeNodeList = getTreeNodeList();
    var nodeParams = JSON.stringify(treeNodeList);
    var msgid = mini.loading("数据导出中，请稍后......", "导出数据");
    $.ajax({
        url: "/PageDetails/DownLoadDataFile",
        data: { nodeParams: encodeURIComponent(nodeParams) },
        type: "post",
        //async: false,     //决定了loading是否显示出来
        success: function (text) {
            mini.hideMessageBox(msgid);
            window.location.href = "/PageDetails/DonwnLoadDataFile";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
}
    
//保存导入数据到数据库
function SavaImportDataEvnet(excelModel, topicId) {
    var tIndexArr = analysisIndexDefinition(excelModel.IndexDefinitionInfo.replace(/[\"\“\”]/g, ''), topicId);
    saveImportTIndex(JSON.stringify(tIndexArr));
    saveImportTCard(excelModel,topicId);
}

//解析主题属性信息
function analysisIndexDefinition(indexDefStr, topicId) {
    var IndexDefObj = [];
    var indexDefArr = indexDefStr.split('|');
    indexDefArr.forEach(function (e) {
        var index = e.split('='); //index[0]=BOT  index[1]=圈闭
        var indexObj = { TopicId: topicId, IndexDefinitionCode: index[0], Value: index[1] };
        IndexDefObj.push(indexObj);
    });
    return IndexDefObj;
}

//解析参数信息
function analysisTCardParams(paramsStr) {
    var tcardparam = {};
    var filter = {};
    tcardparam.filter = filter;
    var filterAnd = "and", filterOr = "or", filterIn = "in";
    var paramArr;
    function constructFilter(filterType, str) {
        paramArr = str.split(filterType);
        filter['$' + filterType] = [];
        paramArr.forEach(function (e) {
            var param = e.trim();
            var paramPairs = param.split('=');
            var paramObj = {};
            paramObj[paramPairs[0]] = paramPairs[1];
            filter['$' + filterType].push(paramObj);
        });
    }
        
    if (paramsStr.indexOf(filterOr) > 0) {
        constructFilter(filterOr, paramsStr);
    }
    //if (paramsStr.indexOf(filterIn) > 0) {
    //    constructFilter(filterIn, paramsStr);
    //}
    if (paramsStr.indexOf(filterAnd) >= -1) {
        constructFilter(filterAnd, paramsStr);
    }
    return JSON.stringify(tcardparam);
}


//保存索引信息项
function saveImportTIndex(tIndexParams) {
    $.ajax({
        url: "/PageDetails/SaveImportTindex?=",
        data: { paramStr: encodeURIComponent(tIndexParams) },
        type: "post",
        success: function (text) {
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
}

//保存知识卡信息项
function saveImportTCard(excelModel, topicId) {
    //存储加载
    var tcardArr = [];
    tcardArr.push(getPrensetTcardData(excelModel, topicId));
    if (excelModel.IsPtList) {
        tcardArr.push({ TopicId: topicId, Title: "成果列表", Definition:"",OrderId:1});
    }
    if (excelModel.IsRemark) {
        tcardArr.push({ TopicId: topicId, Title: "评论列表", Definition: "", OrderId: 2 });
    }
    //添加的tcard列表保存后台
    $.ajax({
        url: "/PageDetails/SaveImportTCard?=",
        data: { paramStr: encodeURIComponent(JSON.stringify(tcardArr)) },
        type: "post",
        //async: false,
        success: function (text) {
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
}

//新加展示知识卡布局页面
function getPrensetTcardData(excelModel, topicId) {
    var tcardObj = {};
    var paramData = {};
    paramData.cells = [];
    excelModel.PtCellInfos.forEach(function (e) {
        //根据Url ---》UrlId---》template.来找到模板参数Defintion
        function getParam(paramsStr,url) {
            //如果导入数据中存在param数据，直接解析参数
            if (paramsStr.trim().length > 0) {
                return analysisTCardParams(paramsStr.replace(/[\"\“\”]/g, ''));
            }
            else {  //获取已有参数模板数据
             return  getTempParamByUrl(url);
            }
        }

        var currentParam = getParam(e.Params, e.Url);
        //paramData.cells.push({ id: e.falgId, type: "Present", url: e.Url, title: e.PtTitle, param: analysisTCardParams(e.Params.replace(/[\"\“\”]/g, '')) });
        paramData.cells.push({ id: e.falgId, type: "Present", url: e.Url, title: e.PtTitle, param: currentParam });
    });

    paramData.layout = excelModel.Layout;
    tcardObj.Definition = JSON.stringify(paramData);
    tcardObj.TopicId = topicId;
    tcardObj.Title = excelModel.tCardTitle;
    tcardObj.OrderId = 0;
    return tcardObj;
}


//通过展示页面url获取参数模板数据
function getTempParamByUrl(url) {
    var param = "";
    $.ajax({
        url: "/DicMaintain/GetPTempByUrl",
        data: { url: encodeURIComponent(url) },
        type: "post",
        async:false,
        success: function (data) {
            param = data.Defintion;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
    return param;
}



//保存树节点信息   测试--后台数据不删除
function SaveData() {
    var selectedNode = treeKTopic.getSelectedNode();
    var data = treeKTopic.getChanges();
    var nodeParams = mini.encode(data);
    var msgid = mini.loading("数据保存中，请稍后......", "保存数据");
    $.ajax({
        url: "/PageDetails/SaveKTopic?=",
        data: { nodeParams: encodeURIComponent(nodeParams) },
        type: "post",
        async: false,
        success: function (text) {
            mini.hideMessageBox(msgid);
            treeKTopic.accept();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            treeKTopic.reload();
            treeKTopic.selectNode(selectedNode);
            treeKTopic.scrollIntoView(selectedNode);
        }
    });
}

//主题索引索引列表增删
function addRow() {
    var newRow = {
        TopicId: currentNodeId,
        IndexDefinitionId: IndexDefinitionEditor.data[0].Id,
        IndexDefinitionTitle: IndexDefinitionEditor.data[0].Title,
        DicNameId: 1,
        Value: ""
    };
    gridIndexDefinition.addRow(newRow, 0);
    gridIndexDefinition.beginEditCell(newRow, "Value");
}

function onActionRendererIndex(e) {
    var uid = e.record._uid;
    var s = '<a class="mini-button frame-submit mini-button-plain" title="删除该行" onclick="delIndexRow(\'' + uid + '\')">'
        + '<span class="mini-button-text mini-button-icon icon-new-delete"  style="margin-top:9px"></span></a>';
    return s;
}

function delIndexRow(uid) {
    var row = gridIndexDefinition.getRowByUID(uid);
    gridIndexDefinition.removeRow(row, true);
}

function removeRow() {
    var rows = gridIndexDefinition.getSelecteds();
    if (rows.length > 0) {
        gridIndexDefinition.removeRows(rows, true);
    }
}

//保存主题索引信息
function saveGridData() {

    var selectedNode = treeKTopic.getSelectedNode();
    if (!selectedNode) {
        toastr["info"]("请先选中知识主题！");
        return;
    }

    gridIndexDefinition.commitEdit();
    var data = gridIndexDefinition.getChanges();


    //测试索引值为空时
    var rowsNull = gridIndexDefinition.findRows(function (row) {
        if (row.Value.trim() == "") return true;
    });
    if (rowsNull.length > 0) {
        gridIndexDefinition.beginEditCell(rowsNull, "Value");
        toastr["error"]("索引属性记录索引值不能为空，数据无法保存！");
        return;
    }

    var rowSame;
    var rowLengthArr = [];
    data.forEach(function (e) {
        rowSame = gridIndexDefinition.findRows(function (row) {
            if (row.IndexDefinitionId == e.IndexDefinitionId && row.Value == e.Value)
                return true;
        });
        rowLengthArr.push(rowSame.length);
    });
    var rowMaxLength = Math.max.apply(null, rowLengthArr);
    if (rowMaxLength >= 2) {
        toastr["error"]("有相同索引属性记录，数据无法保存！");
        return;
    }

    var nodeParams = mini.encode(data);

    var msgid = mini.loading("数据保存中，请稍后......", "保存数据");
    $.ajax({
        url: "/PageDetails/SaveTIndex?=",
        data: { nodeParams: encodeURIComponent(nodeParams) },
        type: "post",
        success: function(text) {
            mini.hideMessageBox(msgid);
            gridIndexDefinition.accept();
        },
        error: function(jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
}

    //获取某个树节点的全路径
    function getNodeFullPath(node) {
        var nodeFullPath;
        var anceNodes = treeKTopic.getAncestors(node);
        if (anceNodes.length <= 0) {
            nodeFullPath = node.Title;
        } else {
            var path = "";
            for (var i = 0; i < anceNodes.length; i++) {
                path += anceNodes[i].Title + '/';
            }
            nodeFullPath = path + node.Title;
        }
        return nodeFullPath;
    }

//解析FullPath添加层级树节点逐次层级的fullPath
function getNodesPathByNodeFullPath(fullPath) {
    //fullPath = '成果展示/成果列表/勘探成果/开发研究';
    var nodeArr = fullPath.split('/');
    var fullPathArr = [];
    for (var i = 0; i < nodeArr.length; i++) {
        var pathCount = i;
        nodeFullPath = "";
        for (var j = 0; j <= pathCount; j++) {
            nodeFullPath += '/' + nodeArr[j];
        }
        nodeFullPath = nodeFullPath.substring(1);
        fullPathArr.push(nodeFullPath);
    }
    return fullPathArr;
}

//输入节点全路径，得到当前结点的父节点全路径和该节点值
function getNodeByFullPath(fullPath) {
    var index = fullPath.lastIndexOf('/');
    var prentFullpath = "";
    var nodeText = "";
    if (index == -1) {
        nodeText = fullPath;
        prentFullpath = "";
    } else {
        nodeText = fullPath.substring(index + 1);
        prentFullpath = fullPath.substring(0, index);
    }
    var newNode = { Text: nodeText, prentFullpath: prentFullpath }
    return newNode;
}