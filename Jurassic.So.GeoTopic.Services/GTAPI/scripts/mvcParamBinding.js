var mvcParamBinding = (function () {
    var MvcParameterAdaptive = {};
    //验证是否为数组
    MvcParameterAdaptive.isArray = Function.isArray || function (o) {
        return typeof o === "object" &&
                Object.prototype.toString.call(o) === "[object Array]";
    };

    //将数组转换为对象
    MvcParameterAdaptive.convertArrayToObject = function (/*数组名*/arrName, /*待转换的数组*/array, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, arr) {
            for (var i in arr) {
                if (!MvcParameterAdaptive.isArray(arr[i]) && typeof arr[i] === "object") {
                    for (var j in arr[i]) {
                        var jelement = arr[i][j];
                        if (MvcParameterAdaptive.isArray(jelement)) {
                            func(name + "[" + i + "]." + j, jelement);
                        } else if (typeof jelement === "object") {
                            MvcParameterAdaptive.convertObject(name + "[" + i + "]." + j + ".", jelement, obj);
                        } else {
                            obj[name + "[" + i + "]." + j] = jelement;
                        }
                    }
                } else if (typeof arr[i] !== "function") {
                    obj[name + "[" + i + "]"] = arr[i];
                }
            }
        }

        func(arrName, array);

        return obj;
    };

    //将字典转换为对象
    MvcParameterAdaptive.convertDictionaryToObject = function (/*数组名*/dictName, /*待转换的数组*/dict, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, dict) {
            delete dict.isDictionary;
            var index = 0;
            for (var i in dict) {
                var ivalue = dict[i];
                if (typeof ivalue === "object") {
                    obj[name + "[" + index + "].Key"] = i;
                    if (MvcParameterAdaptive.isArray(ivalue)) {
                        MvcParameterAdaptive.convertArrayToObject(name + "[" + index + "].Value", ivalue, obj);
                    } else {
                        MvcParameterAdaptive.convertObject(name + "[" + index + "].Value", ivalue);
                    }
                    index++;
                } else if (typeof ivalue !== "function") {
                    obj[name + "[" + index + "].Key"] = i;
                    obj[name + "[" + index + "].Value"] = ivalue;
                    index++;
                }
            }
        }

        func(dictName, dict);

        return obj;
    };

    //转换对象
    MvcParameterAdaptive.convertObject = function (/*对象名*/objName,/*待转换的对象*/turnObj, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, tobj) {
            for (var i in tobj) {
                var ivalue = tobj[i];
                if (MvcParameterAdaptive.isArray(ivalue)) {
                    MvcParameterAdaptive.convertArrayToObject(name + i, ivalue, obj);
                } else if (typeof ivalue === "object") {
                    if (ivalue.isDictionary) {
                        MvcParameterAdaptive.convertDictionaryToObject(name + i, ivalue, obj);
                    } else {
                        func(name + i + ".", ivalue);
                    }
                } else {
                    obj[name + i] = ivalue;
                }
            }
        }

        func(objName, turnObj);
        return obj;
    };

    return function (json, arrName) {
        arrName = arrName || "";
        if (typeof json !== "object") throw new Error("请传入json对象");

        if (MvcParameterAdaptive.isArray(json)) {
            if (!arrName) throw new Error("请指定数组名，对应Action中数组参数名称！");
            return MvcParameterAdaptive.convertArrayToObject(arrName, json);
        }
        if (json.isDictionary) {
            if (!arrName) throw new Error("请指定数组名，对应Action中数组参数名称！");
            return MvcParameterAdaptive.convertDictionaryToObject(arrName, json);
        }
        return MvcParameterAdaptive.convertObject(arrName, json);
    };
})();