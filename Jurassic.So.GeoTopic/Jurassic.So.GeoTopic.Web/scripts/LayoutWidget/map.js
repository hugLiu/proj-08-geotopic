/** 
*  common map , save id and its paramters
* Walt add -20161017
*/
WidgetLayout.Map = function () {
    var _commonMap;

    function map() {
        var keys = new Array();
        var datas = new Array();

        this.Has = function (key) {
            for (var i = 0; i < keys.length; i++) {
                if (key == keys[i]) {
                    return true;
                }
            }
            return false;
        };

        this.Get = function (key) {
            for (var i = 0; i < keys.length; i++) {
                if (key == keys[i]) {
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
                    if (key == keys[i]) {
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

    this.GetMap = function () {
        return _commonMap;
    };

    this.Initialization = function () {
        _commonMap = new map();
    };
};