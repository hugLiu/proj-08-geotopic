const models = {
	//深克隆，返回一个给定对象的副本。
    deepClone:function(obj){
    	let obj1 = JSON.stringify(obj);
    	return JSON.parse(obj1);
    }
}

export  default  models;
