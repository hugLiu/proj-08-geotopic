var settingsData = null
function getData () {
	// return [
	//     {modelName:"ptrender",key:"/render/pt",modelUrl:"../components/cellRender/GtPTRender.vue",childsModel:[
	// 	    {modelName:"rich",key:"html,htm,txt,ppt,pptx,xls,xlsx,doc,docx,pdf",modelUrl:"../components/formatcom/gtrich.vue"},
	// 	    {modelName:"img",key:"jpg,png,jpeg,gif,tif,bmp,tiff,ico",modelUrl:"../components/formatcom/gtimg.vue"},
	// 	    {modelName:"gdb",key:"gdb",modelUrl:"../components/formatcom/gtgdb.vue"},
	// 	    {modelName:"chart",key:"chart",modelUrl:"../components/formatCom/gtechart.vue"},
	// 	    {modelName:"dataset",key:"dataset",modelUrl:"../components/formatCom/gtdataset.vue"}
	//     ]},
	//     {modelName:"ptlistrender",key:"/render/ptlist",modelUrl:"../components/cellRender/GTPTListRender.vue",childsModel:[
	//     	{modelName:"searchlist",key:"search",modelUrl:"../components/PTList/ptlist.vue'"}
	//     ]},
	//     {modelName:"urlrender",key:"default",modelUrl:"../components/cellRender/GtUrlRender.vue"}
	// ];
  if (!settingsData) {
    $.ajax({
	        url: '/Settings/GetModule',
	        // data: param,
	        type: 'get',
	        async: false,
	        // cache: false,
	        dataType: 'json',
	        success: function (data) {
	            settingsData = JSON.parse(data)
	        }
	    })
  }
  return settingsData
}
// webpack的加载模块不支持变量，这个问题需要解决才可以按路径动态加载模块.
// webpack是在编译时就要确定模块的路径，所以不支持变量。暂时在每个模块单独注册使用的模块，而不动态按路径加载。
function addModel (item, com, head) {
  const pa = {}
  const ptrender = require('../components/cellRender/GtPTRender.vue')
  pa[item.modelName] = ptrender
  com[head + item.modelName] = pa[item.modelName]

	// require.ensure([], function(require) {
	// 	//let pa = {};
	// 	//window[head+item.modelName] = require(".."+item.modelUrl);
	// 	//pa[head+item.modelName] = require(".."+item.modelUrl);
	// 	var ptrender = require("../components/cellRender/GtPTRender.vue");
	//     com["ptrender"] = ptrender;
	// });

	// require.ensure([], function(require) {
 //    	var ptrender = require("../components/cellRender/GtPTRender.vue");
 //    	com["ptrender"] = ptrender;
	// });
}
const models = {
	// 获取配置的动态加载模块信息
	// 目前是一次性预加载需要的组件，这里可以动态加载，但是不确定和webpack打包的按需加载有没冲突。
  load: function (scop, name) {

  },
  getConfig: function (key, name) {
    	const data = getData()
    	let mo
    	if (name) {
    		let def
    		for (let i = 0; i < data.length && !mo; i++) {
    			if (data[i].modelName.toLowerCase() == name.toLowerCase()) {
	    			for (let j = 0; j < data[i].childsModel.length && !mo; j++) {
	    				const keys = data[i].childsModel[j].key.split(',')
	    				for (let z = 0; z < keys.length && !mo; z++) {
	    					if (key) {
					    		if (keys[z].toLowerCase() == key.toLowerCase()) {
					    			mo = data[i].childsModel[j].modelName
					    			break
					    		}
					    	}
					    	if (keys[z].toLowerCase() == 'default')		    					{ def = data[i].childsModel[j].modelName }
			    		}
    				}
    			}
    		}
    		if (!mo && def) mo = def
    	} else {
    		let def
    		for (let i = 0; i < data.length; i++) {
    			if (data[i].key.toLowerCase() == key.toLowerCase()) {
    				mo = data[i].modelName
    				break
    			}
    			if (data[i].key.toLowerCase() == 'default')    				{ def = data[i].modelName }
    		}
    		if (!mo && def) mo = def
    	}
    	return mo
  },
  getConfig1: function (com, head, name) {
    	const data = getData()
    	const components = com || {}
    	const he = head || ''
    	// 调试
    	    for (let i = 0; i < data.length; i++) {
    			if (data[i].modelName == 'ptrender') {
    				addModel(data[i], components, he)
    			}
    		}
    	// if(name){
    		// for(let i=0;i<data.length;i++){
    		// 	if(data[i].modelName.toLowerCase() == name.toLowerCase()){
    		// 		addModel(data[i],components,he);
    		// 	}
    		// }
    	// }else{
    	// 	for(let i=0;i<data.length;i++){
    	// 		addModel(data[i],components,he);
    	// 	}
    	// }
    	return components
  }
}

export default models
