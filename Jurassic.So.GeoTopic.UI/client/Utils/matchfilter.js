// import ajax from './pollAjax'
import MetadataDefOperation from './MetadataDefOperation'
// var filterTemplate = {
//    "$and": [
//        { "BOWell": "@BOWell" },
//        {"ProductType":"xxxx"}
//    ]
// };
// var metadataDefinition = [
//    {
//        "name": "BOWell",
//        "mapping": {
//            "get": "ep.bo[?(@.type=='Well')].value",
//            "set": [{
//                "key": "ep.bo[4].type",
//                "value": "Well"
//            }, {
//                "key": "ep.bo[4].value",
//                "value": "@value"
//            }]
//        }
//    },
//    {
//        "name": "ProductType",
//        "mapping": {
//            "get": "ep.producttype",
//            "set": [{
//                "key": "ep.producttype",
//                "value": "@value"
//            }]
//        }
//    }
// ];
// var global = {
//    "BOWell":"A1,A2"
// };
function convert (filter, matchFilter, metadataDefinition) {
  for (var p in filter) {
    var propertyValue = filter[p]
        // 属性值为object或array
    if (typeof propertyValue === 'object') {
            // object
      if (window.isNaN(propertyValue.length)) {
        matchFilter[p] = {}
      } else { // array
        matchFilter[p] = []
      }
      convert(filter[p], matchFilter[p], metadataDefinition)
    }

    var type = typeof propertyValue
    if (type === 'string' || type === 'boolean' || type === 'number') {
      var setMapping = getMapping(p, metadataDefinition)
      if (setMapping && setMapping.length > 0) {
        var targetObject = matchFilter
        var isArray = false

        var arrayPrefix = null
        var index = setMapping[0]['key'].search(/\[\d+\]/)
                // 数组对象
        if (index > 0) {
          isArray = true
          arrayPrefix = setMapping[0]['key'].substring(0, index)// 取数组前缀
          matchFilter[arrayPrefix] = {}
          targetObject = matchFilter[arrayPrefix]['$elemMatch'] = {}
        }

        for (var i in setMapping) {
          if (isNaN(i)) continue
          var clearKey = setMapping[i]['key']
          if (isArray > 0) {
            clearKey = clearKey.split('.').pop()
          }
          var replaceValue = setMapping[i]['value'].replace('@value', propertyValue)
                    // 参数值使用,|分割
          var arrayItem = replaceValue.split(/[,，|]/)
                    // 一项,使用$eq
          if (arrayItem.length === 1) {
            if (arrayItem[0].search(/\$regex/) >= 0) {
              targetObject[clearKey] = { '$regex': arrayItem[0].replace(/\$regex/, '') }
            } else {
              targetObject[clearKey] = { '$eq': arrayItem[0] }
            }
          } else { // 多项，使用$in
            targetObject[clearKey] = { '$in': arrayItem }
          }
        }
      }
    }
  }
}

function getMapping (key, metadataDefinition) {
  for (var md in metadataDefinition) {
    if (metadataDefinition[md]['name'] == key) {
      return metadataDefinition[md]['mapping']['set']
    }
  }
  return null
}

const matchfilter = {

  metadataDefinition: null,
  getFilter: function (filterTemplate, global) {
    // console.log(JSON.stringify(filterTemplate))
    // console.log(JSON.stringify(global))

    var filter = filterTemplate
        // 替换全局变量
    if (global) {
      var strFilterTemplate = JSON.stringify(filterTemplate)
      for (var p in global) {
        var searchValue = new RegExp('@' + p, 'g')
        var replaceValue = global[p]
        strFilterTemplate = strFilterTemplate.replace(searchValue, replaceValue)
                // 替换后的filter(变量被替换为实参）
        filter = JSON.parse(strFilterTemplate)
      }
    }
       // console.log(filter);

        // 获取元数据定义
    if (this.metadataDefinition == null) {
      MetadataDefOperation.init()
      this.metadataDefinition = MetadataDefOperation.metadataDefinition
    }
    if (this.metadataDefinition == null) return filter
        // console.log(this.metadataDefinition);

        // 根据元数据定义来转换为match服务需要的参数
    var matchFilter = {}
    convert(filter, matchFilter, this.metadataDefinition)
        // console.log(matchFilter);

    return { 'filter': matchFilter }
  }
}

export default matchfilter
