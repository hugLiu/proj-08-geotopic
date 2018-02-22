// 导入依赖第三方组件
import Vue from 'vue'
// 是否取消 Vue 所有的日志与警告
Vue.config.silent = false
// 是否是否允许 vue-devtools 检查代码
Vue.config.devtools = true
import VueResource from 'vue-resource'
Vue.use(VueResource)
import { Notification, Message } from 'element-ui'
import 'element-ui/lib/theme-default/index.css'
import { Enumerable } from 'linq-es2015'
// 导入自有组件
import soUI from './Src/Js/SoUI/soUI'
import ptrender from './components/cellRender/GtPTRender.vue'
import cardrender from './components/cardRender/gtCardRender.vue'
import tree from './components/GtTree/GtTree.vue'
import gtremark from './components/formatCom/GtRemark.vue'
import gtsubmit from './components/PTSubmit/PTSubmit.vue'
import ptlist from './components/cellRender/GTPTListRender.vue'
import ptlist2 from './components/PTList/ptlist.vue'
import downloadhistory from './components/DownloadHistory/DownloadHistory.vue'
import newestpt from './components/NewEstPt/newestpt.vue'
import ptdetail from './components/PTDetail/ptdetail.vue'
import favorite from './components/Favorite/Favorite.vue'
import rencentlook from './components/RencentLook/RencentLook.vue'
//  import model from "./Src/Js/utils/loadmodel"

//  -----------------------------------------
function loadUI (name, tags) {
  const items = document.body.childNodes
  function insertAfter (newElement, targetElement) {
    const parent = targetElement.parentNode
    if (parent.lastChild === targetElement) {
      parent.appendChild(newElement)
    } else {
      parent.insertBefore(newElement, targetElement.nextSibling)
    }
  }
  function load (parent, name, tags) {
    name = name.toLowerCase()
    for (let i = 0; i < parent.length; i++) {
      const node = parent[i]
      if (node.nodeName.length < name.length) continue
      const tagName = node.nodeName.toLowerCase()
      if (!tagName.startsWith(name)) {
        load(node.childNodes, name, tags)
        continue
      }
      // 1.替换id
      // 这里还有个策略是指获取带id的，所有嵌套只需要在最外层加id.
      // 获取自定义属性也应该将嵌套属性挂在最外层的域上。
      // bug:子元素的id没有去掉，嵌套的组件只在最上层写id。
      // bug:未扫描子元素的自定义属性。
      // bug:自动加载模块关闭的逻辑中没有获取自定义属性。
      // bug:没有去掉id的话应该判断父节点是不是存在添加过的div。
      // 自动加载的方式应该只使用与自定义组件，引用第三方和基础组件不应该适用，因为会修改参数增加id，和修改返回事件。
      let soId = null
      const soParam = {}
      for (let j = 0; j < node.attributes.length; j++) {
        const nodeAttribute = node.attributes[j]
        const nodeAttributeName = nodeAttribute.name.toLowerCase()
        if (nodeAttributeName === ':id') {
          soId = nodeAttribute.value
          // 添加例外：第三方组件
          if (tagName.length >= 5 && (tagName.substr(3, 2) === 'a_' || tagName.substr(3, 2) === 's_')) {
            node.removeAttributeNode(nodeAttribute)
            j--
          }
        } else {
          const dataTabPrefix = ':data-'
          if (nodeAttributeName.startsWith(dataTabPrefix)) {
            const soParamName = nodeAttributeName.substr(dataTabPrefix, nodeAttributeName.length - dataTabPrefix.length)
            soParam[soParamName] = nodeAttribute.value
            node.removeAttributeNode(nodeAttribute)
            j--
          }
        }
      }
      if (!soId) continue
      const newNode = document.createElement('div')
      newNode.setAttribute('id', 'div' + soId)
      newNode.setAttribute('style', 'display:inline')
      // let node = node;
      // tit.appendChild(node);
      // node.insertBefore(tit);
      insertAfter(newNode, node)
      newNode.appendChild(node)
      // insertAfter(document.getElementById(soId),node);
      // let node = document.getElementsByTagName(node.nodeName.toLowerCase());
      // let div = document.getElementById(soId);
      // node.removeChild(node);
      // 2.绑定
      const mod = []
      for (const tag in tags) {
        mod.push(tags[tag])
      }
      const vm = gtUI.bind({
        scope: 'div' + soId,
        model: mod
      })
      // 3.添加到vm队列,在非自动加载模式下bind中也需要添加
      gtModels.viewModels[soId] = {
        key: soId,
        name: tagName,
        vm: vm,
        // 用户自定义参数，data-*
        param: soParam
      }
    }
  }

  load(items, name, tags)
}

function getNames (name) {
  const tagNames = {}
  const uiNodes = document.querySelectorAll('body *')
  name = name.toLowerCase()
  const minTagLength = name.length
  for (let i = 0; i < uiNodes.length; i++) {
    const uiNodeName = uiNodes[i].nodeName.toLowerCase()
    if (uiNodeName.length >= minTagLength && uiNodeName.substr(0, minTagLength) === name) {
      tagNames[uiNodeName] = uiNodeName
    }
  }
  return tagNames
}

const models = {
  'gt:ptrender': ptrender,
  'gt:cards': cardrender,
  'gt:ktree': tree,
  'gt:remark': gtremark,
  'gt:submit': gtsubmit,
  'gt:downloadhistory': downloadhistory,
  'gt:newestpt': newestpt,
  'gt:favorite': favorite,
  'gt:ptdetail': ptdetail,
  'gt:ptlist': ptlist,
  'gt:rencentlook': rencentlook,
  'gt:searlist2':ptlist2
}
// model.getConfig(models,"gt:");
const gtUI = {
  isAutoLoadUI: true,
  bind: function (com) {
    let model = {}
    if (com.model) {
      if (typeof (com.model) === 'string') {
        model = {
          [com.model]: models[com.model]
        }
      } else {
        for (let i = 0; i < com.model.length; i++) {
          model[com.model[i]] = models[com.model[i]]
        }
      }
    } else {
      // 出于性能的考虑，目前必须手动注册使用到的模块。
      // model=models;
    }
    // for(let i in event){
    //     model[i]=event[i];
    // }
    for (const i in model) {
      const nodes = document.getElementById(com.scope).getElementsByTagName(i)
      for (let n = 0; n < nodes.length; n++) {
        for (let j = 0; j < nodes[n].attributes.length; j++) {
          const ab = nodes[n].attributes[j]
          if (ab.name.substr(0, 1) == ':') {
            let abobject = ab.value
                        // let abtype = "string";
            try {
              abobject = eval(ab.value)
                            // abtype = typeof eval(ab.value)
            } catch (ex) {
                            // 特别针对参数加：的情况字符串的处理
                            // 这个处理会额外增加data
                            // if(!com.data) com.data = {};
                            // if(!com.data[ab.value])
                            //    com.data[ab.value]=abobject;
                            // js的属性方法无法修改键，只有删除再添加，这样会有改变数组的小问题。
                            // 这样处理会造成循环多次，但不会影响结果。
                            // if(ab.name.substr(0, 1) == ":") {
              nodes[n].removeAttribute(ab.name)
              nodes[n].setAttribute(ab.name.substr(1, ab.name.length), ab.value)
              j--
                            // }
            }
                        // if(abtype == "object" || abtype=="function"){
            if (ab.name.substr(0, 3) == ':on') {
              if (!com.methods) com.methods = {}
              if (!com.methods[ab.value]) { com.methods[ab.value] = abobject }
            } else {
              if (!com.data) com.data = {}
              if (!com.data[ab.value]) { com.data[ab.value] = abobject }
            }
                        // }
          }
        }
      }
    }

    const vm = new Vue({
      el: null,
      data: com.data,
      methods: com.methods,
      components: model
    })
    if (com.scope) { vm.$mount('#' + com.scope) }
    return vm
  },
  // soUI.notification,
  notification: Notification,
  // soUI.message,
  message: Message,
  // messageBox:MessageBox,
  getDataByVm: function (vm, name) {
    // return vm.$get(name);
    return vm.$data[name]
  },
  setDataByVm: function (vm, name, data) {
        // vm.$set(name, data);
    vm.$data[name] = data
  },
  getDataById: function (id, name) {
    const vm = gtUI.getViewModel(id)
        // return vm.$get(name);
    return vm.$data[name]
  },
  setDataById: function (id, name, data) {
    const vm = gtUI.getViewModel(id)
        // vm.$set(name, data);
    vm.$data[name] = data
  },
  bindViewModel: function (vm, scope) {
    vm.$mount('#' + scope)
  },
  getViewModel: function (id) {
        // 按id获取自动加载UI是的vm
    let r = null
    if (!gtUI.isAutoLoadUI) { return r }
    if (gtModels.viewModels[id]) { r = gtModels.viewModels[id].vm }
    return r
  }
}

const gtModels = {
  // 两种模式下都需要
  viewModels: {},
  getReturnModel: function (id, data) {
    // 获取返回值模型
    // 需要在每个模块内定义一个id参数。
    let mod = null
    if (!gtUI.isAutoLoadUI) return mod
    if (gtModels.viewModels[id]) {
      const r = gtModels.viewModels[id]
      mod = {
        target: {
          id: r.key,
          tagName: r.name
        },
        dataset: r.param,
        detail: data
      }
    }
    return mod
  }
}

window.onload = function () {
  const tag = 'gt:'
  if (gtUI.isAutoLoadUI) {
    loadUI(tag, getNames(tag))
  }
}

// 分开变量的目的是为了解耦调用名称和打包名称。然后将无法预估大小的vm和主功能分开。
window.gtModels = gtModels
window.gtUI = gtUI
