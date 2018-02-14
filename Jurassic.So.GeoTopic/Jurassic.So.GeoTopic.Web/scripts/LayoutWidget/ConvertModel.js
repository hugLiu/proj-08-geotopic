var cardView = {
	"name": "主题1",
	"ways": ["2", "1"],
	"component": [{
		"index": 0,
		"rowNum": 0,
		"columnNum": 0,
		"type": "Text",
		"param": {
			"id": "G1",
			"type": "Present",
			"url": "/render/pt",
			"title": "测试结果展示123",
			"param": "{\n    \"filter\":{\n        \"$and\":[\n            {\n                \"BoOrganization\":\"@BoOrganization\"\n            },\n            {\n                \"ThemeRT\":\"@ThemeRT\"\n            }\n        ]\n    }\n}\n"
		}
	}, {
		"index": 0,
		"rowNum": 0,
		"columnNum": 1,
		"type": "Text",
		"param": {
			"id": "G2",
			"type": "Refer",
			"url": "https://www.baidu.com/",
			"title": "研究成果分析",
			"param": ""
		}
	}, {
		"index": 0,
		"rowNum": 1,
		"columnNum": 0,
		"type": "Text",
		"param": {
			"id": "G3",
			"type": "Refer",
			"url": "huxiu.com",
			"title": "开发研究",
			"param": ""
		}
	}]
};

var cardDef = {
	"cells": [{
		"id": "G1",
		"type": "Present",
		"url": "/render/pt",
		"title": "测试结果展示123",
		"param": "{\n    \"filter\":{\n        \"$and\":[\n            {\n                \"BoOrganization\":\"@BoOrganization\"\n            },\n            {\n                \"ThemeRT\":\"@ThemeRT\"\n            }\n        ]\n    }\n}\n"
	}, {
		"id": "G3",
		"type": "Refer",
		"url": "huxiu.com",
		"title": "开发研究",
		"param": ""
	}, {
		"id": "G2",
		"type": "Refer",
		"url": "https://www.baidu.com/",
		"title": "研究成果分析",
		"param": ""
	}],
	"layout": {
		"style": null,
		"Rows": [{
			"style": null,
			"Cols": [{
				"style": null,
				"Rows": [{
					"style": null,
					"Cols": [{
						"style": null,
						"Rows": [],
						"CellId": "G1"
					}]
				}],
				"CellId": null
			}, {
				"style": null,
				"Rows": [{
					"style": null,
					"Cols": [{
						"style": null,
						"Rows": [],
						"CellId": "G3"
					}]
				}, {
					"style": null,
					"Cols": [{
						"style": null,
						"Rows": [],
						"CellId": "G2"
					}]
				}],
				"CellId": null
			}]
		}]
	}
};
//var result2 = ToCardView(cardDef);
//console.log("-------------ToCardView------------");
//console.log(JSON.stringify(result2));

//var result1 = ToCardDef(result2);
//console.log("-------------ToCardDef------------");
//console.log(JSON.stringify(result1));



function ToCardDef(cardView) {
	var rowNumArr = [];
	cardView.component.forEach(function(d) {
		rowNumArr.push(d.rowNum)
	});
	var rowNum = Math.max.apply(null, rowNumArr);
	var colNumArr = [];
	cardView.component.forEach(function(d) {
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
			cardView.component.forEach(function(d) {
				if (d.rowNum == i && d.columnNum == j) components.push(d)
			});

			if (components.length < 1) continue;

			var indexArr = [];
			components.forEach(function(d) {
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
				cardView.component.forEach(function(d) {
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

function ToCardView(cardDef) {
	var components = [];
	var ways = [];
	var rows = cardDef.layout.Rows;
	for (var i = 0; i < rows.length; i++) {
		var colNum = rows[i].Cols.length;
		for (var j = 0; j < colNum; j++) {
			var minRowNum = rows[i].Cols[j].Rows.length;
			if (minRowNum > 0) {
				for (var k = 0; k < minRowNum; k++) {
					var cellId = rows[i].Cols[j].Rows[k].Cols[0].CellId;
					var cellModel = {};
					cardDef.cells.forEach(function(d) {
						if (d.id == cellId) cellModel = d
					});
					var component = {};
					component.index = k;
					component.rowNum = i,
						component.columnNum = j;
					component.param = {};
					component.param.id = cellModel.id;
					component.param.type = cellModel.type;
					component.param.url = cellModel.url;
					component.param.title = cellModel.title;
					component.param.param = cellModel.param;
					components.push(component);
				}
			} else {
				var cellId = rows[i].Cols[j].CellId;
				var cellModel = {};
				cardDef.cells.forEach(function(d) {
					if (d.id == cellId) cellModel = d
				});
				var component = {};
				component.index = k;
				component.rowNum = i,
					component.columnNum = j;
				component.param = {};
				component.param.id = cellModel.id;
				component.param.type = cellModel.type;
				component.param.url = cellModel.url;
				component.param.title = cellModel.title;
				component.param.param = cellModel.param;
				components.push(component);
			}
		}
		ways.push(colNum);
	}
	var cardView = {};
	cardView.name = "主题1";
	cardView.ways = ways;
	cardView.component = components;
	return cardView;
	// body...
}