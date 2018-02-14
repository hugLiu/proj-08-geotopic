//list->Json
//function ToCardDef(data, pid) {
//    var result = {};
//    for (var i = 0; i < data.length; i++) {
//        if (data[i].Pid == pid) {
//            var key = data[i].Key;
//            var type = data[i].Type;
//            if (type == "string") {
//                var value = data[i].Value;
//                result[key] = value;
//            }
//            if (type == "object") {
//                result[key] = ToCardDef(data, data[i].Id);
//            }
//            if (type == "array") {
//                result[key] = ParseArray(data, data[i].Id);
//            }
//        }
//    }
//    return result;
//}

function ParseArray(data, pid) {
    var result = [];
    for (var i = 0; i < data.length; i++) {
        if (data[i].Pid == pid) {
            var key = data[i].Key;
            var value = data[i].Value;
            var type = data[i].Type;
            var obj = {};
            obj[key] = value;
            if (type == "string") {
                obj[key] = value;
            }
            if (type == "object") {
                obj[key] = ToCardDef(data, data[i].Id);
            }
            if (type == "array") {
                obj[key] = ParseArray(data, data[i].Id);
            }
            result.push(obj);
        }
    }
    return result;
}

var id = 1;


//Json->list
function ToTreeGrid(data, result, pid) {
    if (data instanceof Array) {
        for (var i = 0; i < data.length; i++) {
            addResult(data[i], result, pid);
        }
    } else {
        addResult(data, result, pid);
    }

    return result;
}

function addResult(data, result, pid) {
    for (var attr in data) {
        if (typeof data[attr] == 'string') {
            var obj = {}
            obj.Id = id;
            id += 1;
            obj.Pid = pid;
            obj.Key = attr;
            obj.Value = data[attr];
            obj.Type = 'string'
            result.push(obj);
        } else {
            var obj = {}
            obj.Id = id;
            id += 1;
            obj.Pid = pid;
            obj.Key = attr;
            obj.Value = "";
            if (data[attr] instanceof Array) {
                obj.Type = 'array';
            } else {
                obj.Type = 'object';
            }
            result.push(obj);
            ToTreeGrid(data[attr], result, obj.Id);
        }
    }
}


function BuildLayout(data, colNum) {
    var length = data.cells.length;    var layout = {};    layout.style = null;    layout.Rows = [];    var rowNum = Math.ceil(length / colNum);    for (var i = 0; i < rowNum; i++) {
        var row = {};        row.style = null;        row.Cols = [];        for (var j = 0; j < colNum; j++) {
            var col = {};            col.style = null;            col.Rows = [];            var index = i * colNum + j;            if (index >= length) break;            col.CellId = data.cells[index].id;            row.Cols.push(col);
        }        layout.Rows.push(row);
    }    return layout;
}

