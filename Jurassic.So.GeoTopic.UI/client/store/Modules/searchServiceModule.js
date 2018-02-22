// 搜索服务模块
// import Vue from 'vue'
// import { Notification } from 'element-ui'

const searchServiceModule = {
  state: {
    // 服务URL:
    serviceUrl: '',
    // 元数据定义数组:
    metadataDefinitions: [],
    // 高亮词数组
    highlightWords: []
  },
  getters: {
    // 获得服务URL
    getSearchServiceUrl (state) {
      return state.serviceUrl
    }
  },
  mutations: {
    // 设置服务URL
    setSearchServiceUrl (state, url) {
      state.serviceUrl = url
    },
    // 设置元数据定义数组
    setMetadataDefinitions (state, metadataDefinitions) {
      state.metadataDefinitions = metadataDefinitions
    },
    // 设置高亮词数组
    setSearchHighlightWords (state, highlightWords) {
      state.highlightWords = highlightWords
    }
  },
  actions: {
  }
}

export default searchServiceModule
