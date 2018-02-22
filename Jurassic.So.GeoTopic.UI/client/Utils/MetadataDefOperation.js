import ajax from './pollAjax'
import jsonPath from './jsonpath'

const models = {
  metadataDefinition: [],
  getMetadataDefinition: function (url) {
    const defaultUrl = '/SearchService/GetMetadataDefinition'
    const targetUrl = url || defaultUrl
    const _this = this
    ajax({
      url: targetUrl,
      type: 'post',
      async: false,
      success: function (result) {
        _this.metadataDefinition = result
      }
    })
  },
  init: function () {
    if (this.metadataDefinition.length == 0) { this.getMetadataDefinition() }
  },
  removeSubscript: function (str) {
    return str.replace(/\[.*\]/g, '')
  },
  getJsonPath: function (name) {
    this.init()
    const result = this.metadataDefinition
    const jsonpaths = jsonPath(result, '$.[?(@.name=="' + name + '")].mapping.get')
    if (jsonpaths.length > 0) { return jsonpaths[0] }
  },
  getTitle: function (name) {
    this.init()
    const result = this.metadataDefinition
    const title = jsonPath(result, '$.[?(@.name=="' + name + '")].title')
    return title[0] || ''
  },
  getKeys: function (name) {
    this.init()
    const result = this.metadataDefinition
    const originkeys = jsonPath(result, '$.[?(@.name=="' + name + '")].mapping.set[*].key')
    const targetkeys = []
    if (originkeys.length > 0) {
      for (let s = 0; s < originkeys.length; s++) {
        const _key = this.removeSubscript(originkeys[s])
        targetkeys.push(_key)
      }
      return targetkeys
    }
  },
  getKey: function (name, type) {
    this.init()
    const result = this.metadataDefinition
    const originkey = jsonPath(result, '$.[?(@.name=="' + name + '")].mapping.set[?(@.value=="' + type + '")].key')[0]
    const targetkey = this.removeSubscript(originkey)
    return targetkey
  },
  getType: function (name) {
    this.init()
    const result = this.metadataDefinition
    const originValues = jsonPath(result, '$.[?(@.name=="' + name + '")].mapping.set[*].value')
    if (originValues.length > 0) {
      for (let s = 0; s < originValues.length; s++) {
        if (originValues[s] != '@value') { return originValues[s] }
      }
    }
  },
  getDataType: function (name) {
    this.init()
    const result = this.metadataDefinition
    const type = jsonPath(result, '$.[?(@.name=="' + name + '")].type')
    return type[0] || ''
  },
  buildFilter: function (name, value) {
    const filters = []
    const keys = this.getKeys(name)

    if (keys.length > 1) {
      const type = this.getType(name)
      if (type != null) {
        const typefilter = {}
        const typekey = this.getKey(name, type)
        typefilter[typekey] = { '$eq': type }
        const index = keys.indexOf(typekey)
        keys.splice(index, 1)
        filters.push(typefilter)
      }
    }
    const filter = {}
    const valuekey = keys[0]

    const datatype = this.getDataType(name)
    if (datatype === 'DateString' && value != '') {
      filter[valuekey] = { '$gte': value }
    } else {
      filter[valuekey] = { '$eq': value }
    }
    filters.push(filter)
    return filters
  },
  getValue: function (kmd, key) {
    const path = '$.' + this.getJsonPath(key)
    const value = jsonPath(kmd, path)
    return value[0] || ''
  }
}

export default models
