// 用户行为模块
import Vue from 'vue'
import ElementUI from 'element-ui'

const userBehaviorModule = {
  state: {
    // 服务URL
    serviceUrl: '/UserBehavior'
  },
  getters: {
    // 获得URL_加入收藏
    getAddFavoriteUrl (state) {
      return state.serviceUrl + '/AddFavoriteMess'
    },
    // 获得URL_删除
    getUserBehaviorRemoveUrl (state) {
      return state.serviceUrl + '/Remove'
    }
  },
  mutations: {
    // 设置服务URL
    setUserBehaviorUrl (state, url) {
      state.serviceUrl = url
    }
  },
  actions: {
    // 检查收藏
    checkCollecting (context, { item, success, error }) {
      if (item.collectId === 0) {
        const data = {
          'BehaviorType': 'favorite',
          'Type': 'list',
          'Title': item.title,
          'Content': JSON.stringify({ id: item.id })
        }
        Vue.http.post(context.getters.getAddFavoriteUrl, { data: JSON.stringify(data) })
          .then(response => {
            item.collectId = Number(response.body)
            if (success) success()
            ElementUI.Notification({ message: '收藏成功！', type: 'success' })
          }, response => {
            if (error) error()
            ElementUI.Notification({ message: '收藏失败！', type: 'error' })
          })
      } else {
        Vue.http.post(context.getters.getUserBehaviorRemoveUrl, { id: item.collectId })
          .then(response => {
            item.collectId = 0
            if (success) success()
            ElementUI.Notification({ message: '取消收藏成功！', type: 'success' })
          }, response => {
            if (error) error()
            ElementUI.Notification({ message: '取消收藏失败！', type: 'error' })
          })
      }
    }
  }
}

export default userBehaviorModule
