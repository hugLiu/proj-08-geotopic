
import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

import searchServiceModule from './Modules/searchServiceModule'
import userBehaviorModule from './Modules/userBehaviorModule'

import MetadataDefOperation from './../Utils/MetadataDefOperation'

const store = new Vuex.Store({
  state: {
    siteUrl: '',
    apiHost: '',
    apiToken: ''
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    SearchService: searchServiceModule,
    UserBehavior: userBehaviorModule
  },
  utils: {
    MetadataDefOperation: MetadataDefOperation
  }
})

export default store
