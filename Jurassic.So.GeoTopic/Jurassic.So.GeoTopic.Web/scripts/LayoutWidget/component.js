/** 
* componenthtml class build compoents dom by type
*/
document.write('<script src="~/scripts/LayoutWidget/ConvertModel.js"><\/script>');


WidgetLayout.ComponentHtml = function () {
    //组件创建具体的HTML
    function buildHtml(com, id,falgId,title) {
        //var _content = '<div id=' + flagId + ' class="lyblock ">';
        //com.setTypeName(title.detail);
        var _content = '<div class="lyblock ">';
        _content += '<div class="row lybox">';

        _content += '<div class="lybox-draggable">';

        _content += '<a class="btn btn-app btn-light btn-xs drag" style="width:45px;height=30px;" href="#" data-toggle="tooltip" data-placement="top" title=' + title + '>';

        //walt  add   {delete button and event}
        //_content += '<div href="#" data-action="close" class="lytoolBar-remove lybox-delete">';
        //_content += title.titleBrief;
        //_content += title;
        //_content += '<span class="icon-remove" style="float:right" ></span>';
        //_content += '</div>';
        //---------------------
        _content += '<img src="'+com.IcoUrl()+'" width=25px;height=20px;/>';
        
        //组件标识文字
        _content += '<div>';
        _content += falgId;
        _content += '</div>';

        _content += '</a>';
        _content += '</div>';

        _content += '<div class="col-xs-12">';
        _content += '<div class="widget-box lycomponentbox" id="' + id + '">';

        _content += '<div class="component-type" style="display:none">';
        _content += com.Type();
        _content += '</div>';

        _content += '<div class="widget-header">';

        _content += '<h5>';
        _content += '<img src="../content/Images/LayoutWidget/transform_move.png" class="lybox-draggable" />';
        //_content += com.TypeName(); 
        //_content += title.detail;
        _content += falgId + '[' + title + ']';

        _content += '</h5>';

        _content += '<div class="widget-toolbar">';

        //隐藏参数设置按钮
        //_content += '<a href="#" data-action="settings" class="i-set lycomponent-set">';
        //_content += '<i class="icon-cog"></i>';
        //_content += '</a>';

        _content += '<a href="#" data-action="reload" class="i-download">';
        _content += '<i class="icon-download-alt"></i>';
        _content += '</a>';
        _content += '<a href="#" data-action="collapse" class="chevron-up">';
        _content += '<i class="icon-chevron-up"></i>';
        _content += '</a>';
        _content += '<a href="#" data-action="close" class="i-remove lybox-delete">';
        _content += '<i class="icon-remove"></i>';
        _content += '</a>';

        _content += '</div>';

        _content += '</div>';

        _content += '<div class="widget-body">';
        //_content += '<div class="widget-body-inner" style="display: block;">';
        _content += '<div class="widget-body-inner" style="height:0px;">';
        //_content += '<div id="lywidgetmain" class="widget-main" style="min-height:160px">';
        _content += '<div id="lywidgetmain" class="widget-main" style="height:0px">';
        _content += com.BuildContent();
        _content += '</div>';
        _content += '</div>';
        _content += '</div>';

        _content += '</div>';
        _content += '</div>';

        _content += '</div>';
        _content += '</div>';
        return _content;
    }


    //this.BuildComponentHtml = function (type) {
    //    //创建组件展示模块
    //    if (type == "Text") {
    //        return buildHtml(new WidgetLayout.TextHtml());
    //    }
    //    if (type == "Picture") {
    //        return buildHtml(new WidgetLayout.PictureHtml());
    //    }
    //    if (type == "Table") {
    //        return buildHtml(new WidgetLayout.TableHtml());
    //    }
    //    if (type == "Chart") {
    //       return buildHtml(new WidgetLayout.ChartHtml());
    //    }
    //};

    this.BuildComponentHtml = function () {
        //创建组件展示模块
        return buildHtml(new WidgetLayout.TextHtml());
    };

    //Walt add --20161011
    this.BuildComponentHtml = function (id, falgId,title) {
        //创建组件展示模块
        return buildHtml(new WidgetLayout.TextHtml(), id, falgId, title);
    };
};

/** 
* component class
* map , save components' dom and its event
* components operate and its event
*/
WidgetLayout.Components = function () {
    var _componentMap;

    /**
    * save components' dom and its event
    */
    function map() {
        var keys = new Array();
        var datas = new Array();

        this.Has = function (key) {
            for (var i = 0; i < keys.length; i++) {
                if (key.is(keys[i])) {
                    return true;
                }
            }
            return false;
        };

        this.Get = function (key) {
            for (var i = 0; i < keys.length; i++) {
                if (key.is(keys[i])) {
                    return datas[i];
                }
            }
            return false;
        };

        this.Set = function (key, data) {
            if (this.Has(key)) {
                for (var i = 0; i < keys.length; i++) {
                    if (key.is(keys[i])) {
                        datas[i] = data;
                    }
                }
            }
            else {
                keys.push(key);
                datas.push(data);
            }
            return false;
        };

        this.Remove = function (key) {
            if (this.Has(key)) {
                for (var i = 0; i < keys.length; i++) {
                    if (key.is(keys[i])) {
                        keys.splice(i, 1);
                        datas.splice(i, 1);
                    }
                }
            }
            return false;
        };

        this.Keys = function () {
            return keys;
        };
    }

    /**************************************
    * event
    **************************************/
    function settingEvent() {
        $(".lycontainer").on("click", ".lycomponent-set", function () {
            var lycomponentbox = $(this).parents(".lycomponentbox");
            var component = getComponent(lycomponentbox);
            component.Setting();
            return false;
        });
    }

    function deleteBoxEvent() {
        $(".lycontainer").on("click", ".lybox-delete", function () {
            if (confirm("确定要删除吗？")) {
                $(this).parents(".lyblock").remove();
                var lycomponentbox = $(this).parents(".lybox").find(".lycomponentbox");
                operateComponents(lycomponentbox, "delete");
            }
            return false;
        });
    }

    /**************************************
    * map operate
    * components operate
    **************************************/
    //function operateComponents(lycomponentbox, opType) {
    //    var parameter;
    //    if (_componentMap.Get(lycomponentbox)) {
    //        parameter = _componentMap.Get(lycomponentbox).Save();
    //    }
    //    _componentMap.Remove(lycomponentbox);
    //    if (opType !== "delete") {
    //        setComponent(lycomponentbox, parameter);
    //    }
    //}

    //如上方法改造
    function operateComponents(lycomponentbox,opType,newParameter) {
        var parameter = newParameter;
        if (_componentMap) {
             if (_componentMap.Get(lycomponentbox)) {
            parameter = _componentMap.Get(lycomponentbox).Save();
        }
        _componentMap.Remove(lycomponentbox);
        }
        if (opType !== "delete") {
            setComponent(lycomponentbox,opType, parameter);
        }
    }

    this.operateComponents = function (lycomponentbox, parameter) {
        operateComponents(lycomponentbox, parameter);
    }

    function getComponent(lycomponentbox) {
        setComponent(lycomponentbox);
        return _componentMap.Get(lycomponentbox);
    }

    function setComponent(lycomponentbox,opType, parameter) {
        if (typeof _componentMap !== "undefined") {
            var has = _componentMap.Has(lycomponentbox);
            if (!has) {
                var component;
                //var type = lycomponentbox.children(".component-type").html();
                var dom = lycomponentbox.find(".widget-main");
                if (dom[0].id.indexOf("-") < 0) {
                    dom[0].id = dom[0].id + "-" + (new Date() - 0);
                }
                //if (type == "Text") {
                //    component = new WidgetLayout.Text(dom);
                // }
                //else if (type == "Picture") {
                //    component = new WidgetLayout.Picture(dom);
                //}
                //else if (type == "Table") {
                //    if (dom.find(".lytable")[0].id.indexOf("-") < 0) {
                //        dom.find(".lytable")[0].id = dom.find(".lytable")[0].id + "-" + (new Date() - 0);
                //    }
                //    component = new WidgetLayout.Table(dom);
                //}
                //else if (type == "Chart") {
                //    component = new WidgetLayout.Chart(dom);
                //}
                component = new WidgetLayout.Text(dom);  //Walt add --20161012
                component.SetParameter(parameter);
                loadComponent(dom, opType, parameter); /** load dom with data */

                _componentMap.Set(lycomponentbox, component);
            }
        }
    }

    function loadComponent(dom, type, parameter) {
        var component;
        if (dom[0].id.indexOf("-") < 0) {
            dom[0].id = dom[0].id + "-" + (new Date() - 0);
        }
        //if (type == "Text") {
        //    component = (new WidgetLayout.Text(dom));
        //}
        //else if (type == "Picture") {
        //    component = new WidgetLayout.Picture(dom);
        //}
        //else if (type == "Table") {
        //    dom.find(".lytable")[0].id = dom.find(".lytable")[0].id + "-" + (new Date() - 0);
        //    component = new WidgetLayout.Table(dom);
        //}
        //else if (type == "Chart") {
        //    component = new WidgetLayout.Chart(dom);
        //}

        //Walt add --20161017
        component = (new WidgetLayout.Text(dom));

        component.Load(parameter);
    }

    //this.OperateCompnoents = function (lycomponentbox, opType) {
    //    operateComponents(lycomponentbox, opType);
    //};

    //parameter
    //改造上面方法
    this.OperateCompnoents = function (lycomponentbox,opType,newParameter) {
        operateComponents(lycomponentbox, opType, newParameter);
    };

    /**
    * register components event and map init
    * @public
    */
    this.Initialization = function () {
        _componentMap = new map();
        settingEvent();
        deleteBoxEvent();
    };

    /** 
    * load components html by dom and db data
    * @public
    */
    this.LoadComponentHtml = function (obj, data) {
        var component = new WidgetLayout.ComponentHtml();
        var propertyList = data.component;
        for (var k = 0; k < propertyList.length; k++) {
            var rowNum = propertyList[k].rowNum;
            var columnNum = propertyList[k].columnNum;
            var index = propertyList[k].index;
            var falgId = propertyList[k].param.id;
            var title = propertyList[k].param.title;
            //创建component的Html
            //var content = component.BuildComponentHtml("", { detail: title });
            var content = component.BuildComponentHtml("", falgId, title);
            obj.find(".lyrow").eq(rowNum).find(".lycolumn").eq(columnNum).append(content);
        }
    };

    /** 
    * load components by dom and db data
    * @public
    */
    this.LoadComponent = function (obj, arr) {
        var propertyList = arr.component;
        for (var k = 0; k < propertyList.length; k++) {
            var rowNum = propertyList[k].rowNum;
            var columnNum = propertyList[k].columnNum;
            var index = propertyList[k].index;
            var type = propertyList[k].type;
            var parameter = propertyList[k].param;

            var dom = obj.find(".lyrow").eq(rowNum).find(".lycolumn").eq(columnNum).find(".lybox").eq(index).find(".widget-main");
            var boxObj = dom.parents(".lycomponentbox");

            //加载参数数据设置参数   Walt add pramater:""  --20161018
            setComponent(boxObj,"",parameter);

            if (typeof parameter !== "undefined") {
                dom.find(".lycomponent-init-info").css("display", "none");
                loadComponent(dom, type, parameter);
            }
        }
    };

    /**
    * save by components position and its parameters 
    * @public
    */
    this.Save = function (data) {
        var keys = _componentMap.Keys();
        var layout = {};
        var component = [];
        var ways = [];
        var lyrow = document.getElementById("lycontainer").getElementsByClassName("lyrow");
        for (var i = 0; i < lyrow.length; i++) {
            ways.push(lyrow[i].getElementsByClassName("lycolumn").length.toString());
        }

        var i;
        for(i=0;i<keys.length;i++){
            var key = keys[i];
            var paramList = {};
            paramList.index = key.parents(".lyblock").index();
            paramList.rowNum = key.parents(".lyrow").index() - 1;
            paramList.columnNum = key.parents(".lycolumn").index() - 2;
            paramList.type = key.find(".component-type")[0].innerHTML;
            paramList.param = _componentMap.Get(key).Save();
            component.push(paramList);
        }
        component.sort(function (a, b) { return a.index - b.index; });
        layout.name = "主题1";
        layout.ways = ways;
        layout.component = component;


        //debugger;
        //可以考虑将模型转换在前端js转换   可以转换成功
        //var testJson = toCardDef(layout);
        //var tCardObj = JSON.parse(data);
        //tCardObj.Definition = JSON.stringify(testJson);
        //var tCardJson = JSON.stringify(tCardObj);

        var tCardObj = JSON.parse(data);
        tCardObj.Definition = JSON.stringify(layout);
        var tCardJson = JSON.stringify(tCardObj);


        //测试去掉\转义符号,转有string->json对象存在问题
        //string节点的"不是字符而被当作是符号节点
        //var dataObj1 = JSON.parse(tCardJson);
        //var dataJson = tCardJson.replace(/\\/g, '');
        //var dataObj2 = JSON.parse(dataJson);
        //var isFlag = false;

        $.ajax({
            type: "POST",
            url: "/PageDetails/SaveTCardLayout?data=" + encodeURIComponent(tCardJson),
            //url: "/PageDetails/SaveTCardLayoutNoConvert?data=" + encodeURIComponent(tCardJson),
            dataType: "json",
            async: false,
            success: function (text) {
                if (text == "succ") {
                    //isFlag = true;
                }
            }
        });
        //return isFlag;
    };




    function toCardDef(cardView) {
        var rowNumArr = [];
        cardView.component.forEach(function (d) {
            rowNumArr.push(d.rowNum)
        });
        var rowNum = Math.max.apply(null, rowNumArr);
        var colNumArr = [];
        cardView.component.forEach(function (d) {
            colNumArr.push(d.columnNum)
        });
        var colNum = Math.max.apply(null, colNumArr);

        var cardDef = {};
        cardDef.cells = [];
        cardDef.layout = {};
        cardDef.layout.style = null;
        cardDef.layout.Rows = [];
        for (var i = 0; i <= rowNum; i++) {
            var row = {};
            row.style = null;
            row.Cols = [];
            for (var j = 0; j <= colNum; j++) {
                var col = {};
                col.style = null;
                col.Rows = [];
                col.CellId = null;

                var components = [];
                cardView.component.forEach(function (d) {
                    if (d.rowNum == i && d.columnNum == j) components.push(d)
                });

                if (components.length < 1) continue;

                var indexArr = [];
                components.forEach(function (d) {
                    indexArr.push(d.index)
                });
                var index = Math.max.apply(null, indexArr);

                for (var k = 0; k <= index; k++) {
                    var minRow = {};
                    minRow.style = null;
                    minRow.Cols = [];
                    var minCol = {};
                    minCol.style = null;
                    minCol.Rows = [];
                    minCol.CellId = null;

                    var component = {};
                    console.log(cardView.component);
                    cardView.component.forEach(function (d) {
                        if (d.rowNum == i && d.columnNum == j && d.index == k) component = d
                    });
                    console.log(component);
                    if (component != {}) {
                        minCol.CellId = component.param.id;
                        var cellModel = {};
                        cellModel.id = component.param.id;
                        cellModel.type = component.param.type;
                        cellModel.url = component.param.url;
                        cellModel.title = component.param.title;
                        cellModel.param = component.param.param;
                        cardDef.cells.push(cellModel);
                    }
                    minRow.Cols.push(minCol);
                    col.Rows.push(minRow);
                }
                row.Cols.push(col);
            }
            cardDef.layout.Rows.push(row);
        }

        return cardDef;
    }





};