/** 
* container class
*/
WidgetLayout.Container = function () {
    var _containerDiv;
    var _components;


    /**
    * generate instance of components class 
    */
    function constructor() {
        _components = new WidgetLayout.Components();
    }

    constructor();

    /**************************************
    * html
    **************************************/
    function getHeaderHtml() {
        var content = '';
        content += '<div class="lyheader" id="ly-header">';

        //content += '<div class="lytitle">';
        //content += '<a class="lytitle-a lylayout-edit">';
        //content += '编辑布局';
        //content += '</a>';
        //content += '</div>';

        //content += '<div class="lytitle">';
        //content += '<a class="lytitle-a lylayout-preview">';
        //content += '预览布局';
        //content += '</a>';
        //content += '</div>';

        content += '</div>';
        return content;
    }

    function buildHtml() {
        var content = '';
        content += '<div class="col-xs-12">';
        content += '<div id="lycontainer" class="lycontainer ui-sortable" style="width:600px;height:250px;">';
        content += getHeaderHtml();
        content += '</div>';
        content += '</div>';

        $(_containerDiv).html(content);
    }

    /**************************************
    * event
    **************************************/
    function containerSortableEvent() {
        $(".lycontainer").sortable({
            connectWith: ".lyrow",
            opacity: .5,
            handle: ".lyrow-draggable",
            placeholder: "sortable-placeholder"
        });
    }


    //function columnSortableEvent() {
    //    $(".lycontainer .lycolumn").sortable({
    //        connectWith: ".lycolumn",
    //        opacity: .5,
    //        handle: ".lybox-draggable",
    //        placeholder: "sortable-placeholder",
    //        refreshPositions: true,
    //        update: function (e, t) {
    //            // drag event result in the change of components's position 
    //            // change components's map
    //            _components.OperateCompnoents(t.item.find(".lycomponentbox"));
    //        }
    //    });
    //}

    //parameter
    //以上方法改造
    function columnSortableEvent() {

        $(".lycontainer .lycolumn").sortable({
            connectWith: ".lycolumn",
            opacity: .5,
            handle: ".lybox-draggable",
            placeholder: "sortable-placeholder",
            refreshPositions: true,
            update: function (e, t) {
                // drag event result in the change of components's position 
                // change components's map
                //源代码
                //_components.OperateCompnoents(t.item.find(".lycomponentbox"));

                //修改传入参数   Walt add --20161017
                var parameter;
                var id = "lycomponent" + (new Date() - 0);
                var obj = t.item.find(".lycomponentbox");
                var map = _map;
                parameter = map.GetMap().Get($(obj)[0].id);
                $(obj)[0].id = id;
                _components.OperateCompnoents(obj, "", parameter);

            }
        });
    }


    function rowDraggableEvent() {
        $(".cells .lyrow").draggable({
            connectToSortable: ".lycontainer",
            helper: "clone",
            handle: ".lyrow-draggable",
            drag: function (e, t) {
                t.helper.width(400);
            },
            stop: function (e, t) {
                columnSortableEvent();
            }
        });
    }




    //function boxDraggableEvent() {
    //    $(".boxes .lyblock").draggable({
    //        cursor: "move",
    //        connectToSortable: ".lycolumn",
    //        helper: "clone",
    //        handle: ".lybox-draggable",
    //        drag: function (e, t) {
    //            t.helper.width(400);
    //        },
    //        stop: function (e, t) {
    //            columnSortableEvent();
    //        }
    //    });
    //}

    //添加parameter参数
    //以上方法改造
    function boxDraggableEvent() {
        $(".boxes .lyblock").draggable({
            cursor: "move",
            connectToSortable: ".lycolumn",
            helper: "clone",
            handle: ".lybox-draggable",
            drag: function (e, t) {
                t.helper.width(400);
            },
            stop: function (e, t) {
                columnSortableEvent();
            }
        });
    }


    function deleteRowEvent() {
        $(".lycontainer").on("click", ".lyrow-delete", function () {
            if (confirm("确定要删除吗？")) {
                $(this).parents(".lyrow").remove();
                var lycolumn = $(this).parents(".lyrow").find(".lycolumn");
                updateComponents(lycolumn);
            }
            return false;
        });
    }

    function updateComponents(lycolumn) {
        for (var i = 0; i < lycolumn.length; i++) {
            var lybox = lycolumn.eq(i).find(".lybox");
            for (var j = 0; j < lybox.length; j++) {
                var lycomponentbox = lybox.eq(j).find(".lycomponentbox");
                _components.OperateCompnoents(lycomponentbox, "delete");
            }
        }
    }

    function editLayoutEvent() {
        $(".lycontainer").on("click", ".lylayout-edit", function () {
            $("#lycontainer").removeClass("lypreview");
            $("#lycontainer").addClass("lycontainer");
            return false;
        });
    }

    function previewLayoutEvent() {
        $(".lycontainer").on("click", ".lylayout-preview", function () {
            $("#lycontainer").removeClass("lycontainer");
            $("#lycontainer").addClass("lypreview");
            return false;
        });
    }

    /**
    * register container event & components event (components init)
    * @public
    */
    this.Initialization = function (map) {
        _map = map;
        _components.Initialization();

        containerSortableEvent();
        columnSortableEvent();
        rowDraggableEvent(); 
        boxDraggableEvent();
        deleteRowEvent();
        editLayoutEvent();
        previewLayoutEvent();
    };

    this.BuildHtml = function (div) {
        _containerDiv = div;
        buildHtml();
    };

    /**
    * 
    * @public
    */
    this.ColumnSortableEvent = function () {
        columnSortableEvent();
    };

    //Walt add  --20161017
    this.BoxDraggableEvent = function () {
        boxDraggableEvent();
    };


    /** 
    * load container html by db data
    * @public
    */
    this.LoadContainerHtml = function (data) {
        var content = '';
        if (data.ways != null) {
            var rowLen = data.ways.length;
            for (var m = 0; m < rowLen; m++) {
                content += $(".lyblock-col" + 12 / data.ways[m]).html();
            }
        }
        return content;
    };


    /** 
    * load components html by dom and db data
    * @public
    */
    this.LoadComponentHtml = function (obj, data) {
        // column init sortable
        columnSortableEvent();
        _components.LoadComponentHtml(obj, data);
    };

    /** 
    * load components by dom and db data
    * @public
    */
    this.LoadComponent = function (obj, arr) {
        _components.LoadComponent(obj, arr);
    };

    /** 
    * save components position and its event
    * @public
    */
    this.Save = function (data) {
        //return _components.Save(data);
        _components.Save(data);
    };


    //other 
    this.getboxdraggleevent = function () {
        boxDraggableEvent();
    };
};
