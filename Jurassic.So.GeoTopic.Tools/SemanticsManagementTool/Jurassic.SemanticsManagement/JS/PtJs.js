var Genders = [{ id: 'en-US', text: 'en-US' }, { id: 'zh-CN', text: 'zh-CN' }];
mini.parse();

//_________0、公共代码部分_______________________________________________//

//适应屏幕的大小（有待完善）
$(document).ready(function () {
    var wheight = $(window).height();
    if (wheight > 500 && wheight < 600) {
        $(".mini-layout").css("height", "559px");
        $(".mini-layout-border").css("height", "559px");
    }
    if (wheight > 600) {
        $(".mini-layout").css("height", "691px");
        $(".mini-layout-border").css("height", "691px");
    }
});
//保存成功提示
function showtipsinfor() {
    mini.showTips({
        content: "语种为en-US时,输入译文" + "<br/>" + "语种为zh-CN时,输入别名",
        state: "danger",
        x: "right",
        y: "bottom",
        timeout: 2000
    });
}

//筛选节点
function RefreshNode(termselecttree, termtree) {
    var treeselectNode = termselecttree.getSelectedNode();//获得选中的BP树的节点
    var selectChildNodes = termtree.getChildNodes(treeselectNode);
    var isLeaf = termtree.isLeaf(treeselectNode);//判断是否是叶子节点
    if (isLeaf == false) {
        termtree.loadData(selectChildNodes);
    } else {
        alert("此节点是子节点!!!");
    }
}

//公共变量
var ptTree = mini.get("PtTree");
var bpTree = mini.get("BpTree");
var bptreenode = bpTree.getSelectedNode();
var ptrootNodes = ptTree.getRootNode();  //获得pt树的所有根节点
var ptChecknodes = ptTree.getCheckedNodes(false);//获得check中的所有节点
var ptParent = ptTree.getChildNodes(ptChecknodes[0]);
var ptrootsCount = "";
var ptChildCount = "";



//______________________1、对PT树的操作_______________________________________________//
//添加一级PT节点
function onAddParentPtClick() {
    if (typeof (bptreenode) == "undefined") {
        ptrootsCount = 1;
    } else {
        ptrootsCount = ptrootNodes.children.length + 1 * 1;
    }
    var firstlevelptNode = mini.encode({
        TermClassID: bptreenode.TermClassId,
        Term: "新建一级PT节点",
        PathTerm: "新建一级PT节点",
        OrderIndex: ptrootsCount,
    });

    if (bptreenode != null) {
        $.Ajax({
            url: "/ProductTypeManagement/AddPtNodeResult",
            data: { ptmodel: firstlevelptNode, bpTermId: bptreenode.TermClassId },
            success: function () {
            }
        });
    } else {
        alert("请选择BP节点");
    }
}

//添加PT子节点
function onAddChildPtClick() {
    if (ptParent.length == 0) {
        ptChildCount = 1;
    } else {
        ptChildCount = ptParent.length + 1 * 1;
    }
    var childptNode = mini.encode({
        TermClassID: ptChecknodes[0].TermClassId,
        Term: "新建子节点",
        PathTerm: ptChecknodes[0].Term + "/" + "新建子节点",
        OrderIndex: ptChildCount,
    });
    ptTree.addNode(childptNode, "add", ptChecknodes[0]);
    if (ptChecknodes.length > 0) {
        $.ajax({
            url: "/ProductTypeManagement/AddPtchildNodeResult",
            data: { bpTermId: bptreenode.TermClassId, ptParentId: ptChecknodes[0].TermClassId, ptModel: childptNode },
            success: function () {
            }
        });

    } else {
        alert("请勾选PT节点");
    }

}

//删除节点
function onRemovePtClick() {
    if (ptChecknodes.length > 0) {
        if (confirm("确定删除选中节点?")) {
            for (var i = 0; i < ptChecknodes.length; i++) {
                $.ajax({
                    url: "/ProductTypeManagement/DeleteNodebyIdResult",
                    data: { ptnodeId: ptChecknodes[i].TermClassId },
                    success: function () {
                        ptTree.removeNodes(ptChecknodes);
                    },
                });
            }
        }
    } else {
        alert("删除前，请选中节点");
    }

}

//筛选pt节点
function onRePtfreshClick() {
    var ptselectTree = mini.get("PtSelecttree");
    RefreshNode(ptselectTree, ptTree);
}

//刷新pt树


//__________________2、对BP树的操作_____________________________________________//
//筛选Bp树的节点
function filterNodeClick() {
    var bpselectTree = mini.get("BpSelecttree");
    RefreshNode(bpselectTree, bpTree);
}
//刷新bp节点
function onRefreshClick(parameters) {
    bpTree.load("/ProductTypeManagement/GetBpTreeResult");
}
