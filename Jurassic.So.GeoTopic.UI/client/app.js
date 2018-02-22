import Vue from 'vue'
Vue.config.silent = false// true;
Vue.config.devtools = true// false;

// import VueDevtools from 'vue-devtools'
// import { app, BrowserWindow } from 'electron'
// app.on('ready', () => {
//   if (process.env.NODE_ENV !== 'production') {
//     VueDevtools.install()
//   }
// })

import VueResource from 'vue-resource'
import Vuex from 'vuex'
Vue.use(VueResource)
Vue.use(Vuex)

import 'element-theme-default'
import ElementUI from 'element-ui'
Vue.use(ElementUI)

import soUI from './SoUI/soUI'

import store from './store/index'

import ptrendernew from './components/cellRender/GtPTRender2.0.vue'
import ptrender from './components/cellRender/GtPTRender.vue'
import cardrender from './components/cardRender/gtcardrender.vue'
import tree from './components/gttree/gttree.vue'
import gtremark from './components/formatCom/GtRemark.vue'
import gtsubmit from './components/ptsubmit/ptsubmit.vue'
import ptlist from './components/cellRender/GTPTListRender.vue'
import ptlist2 from './components/PTList/ptlist.vue'
import downloadhistory from './components/downloadhistory/downloadhistory.vue'
import newestpt from './components/newestpt/newestpt.vue'
import ptdetail from './components/ptdetail/ptdetail.vue'
import ptdetail2 from './components/ptdetail/ptdetail2.vue'
import favorite from './components/favorite/favorite.vue'
import rencentlook from './components/rencentlook/rencentlook.vue'
import ptcompare from './components/gtptcompare/gtptcompare.vue'
import gtrcmdtabs from './components/gtrecommend/gtrcmdtabs.vue'
import gtrcmdsemantics from './components/gtrecommend/gtRcmdSemantics.vue'
import bonear from './components/GtRecommend/gtRcmdBONear.vue'
import bosimilar from './components/GtRecommend/gtRcmdBOSimilar.vue'
import boassociate from './components/GtRecommend/gtRcmdBOAssociate.vue'
// import eltree from './components/GtRecommend/elTreeTest.vue'
// import model from "./Src/Js/utils/loadmodel"

// -----------------------------------------
function loadUI (name, tags) {
  const items = document.getElementsByTagName('body')[0].childNodes
  function insertAfter (newElement, targetElement) {
    var parent = targetElement.parentNode
    if (parent.lastChild === targetElement) {
      parent.appendChild(newElement, targetElement)
    } else {
      parent.insertBefore(newElement, targetElement.nextSibling)
    };
  };
  function load (data, name, tags) {
    for (let i = 0; i <= data.length - 1; i++) {
      if (data[i].nodeName.toLowerCase().substr(0, 3) == name.toLowerCase()) {
        // alert(data[i].nodeName.toLowerCase());
        // 1.替换id
        // 这里还有个策略是指获取带id的，所有嵌套只需要在最外层加id.
        // 获取自定义属性也应该将嵌套属性挂在最外层的域上。
        // bug:子元素的id没有去掉，嵌套的组件只在最上层写id。
        // bug:未扫描子元素的自定义属性。
        // bug:自动加载模块关闭的逻辑中没有获取自定义属性。
        // bug:没有去掉id的话应该判断父节点是不是存在添加过的div。
        //     自动加载的方式应该只使用与自定义组件，引用第三方和基础组件不应该适用，因为会修改参数增加id，和修改返回事件。
        let soId = null
        const soParam = {}
        const tagName = data[i].nodeName.toLowerCase()
        for (let j = 0; j <= data[i].attributes.length - 1; j++) {
          if (data[i].attributes[j].name === ':id') {
            // if(data[i].attributes[j] && data[i].attributes[j].name == ":id"){
            soId = data[i].attributes[j].value
            // 添加例外：第三方组件。
            if (data[i].nodeName.toLowerCase().substr(3, 2) == 'a_' || data[i].nodeName.toLowerCase().substr(3, 2) == 's_') {
              data[i].removeAttribute(data[i].attributes[j].name)
              j--
            }
            // break;
          } else {
            if (data[i].attributes[j].name.toLowerCase().substr(0, 6) == ':data-') {
              soParam[data[i].attributes[j].name.substr(6, data[i].attributes[j].name.length - 1)] = data[i].attributes[j].value
              data[i].removeAttribute(data[i].attributes[j].name)
              j--
            }
          }
        }
        if (!soId) continue
        const tit = document.createElement('div')
        tit.setAttribute('id', 'div' + soId)
        tit.setAttribute('style', 'display:inline')
        // let node = data[i];
        // tit.appendChild(node);
        // data[i].insertBefore(tit);
        insertAfter(tit, data[i])
        document.getElementById('div' + soId).appendChild(data[i])
        // insertAfter(document.getElementById(soId),data[i]);
        // let node = document.getElementsByTagName(data[i].nodeName.toLowerCase());
        // let div = document.getElementById(soId);
        // data[i].removeChild(data[i]);
        // 2.绑定
        const mod = []
        for (const tag in tags) {
          mod.push(tags[tag])
        }
        const vm = gtUI.bind({
          scope: 'div' + soId,
          tagName: tagName,
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
      } else {
        load(data[i].childNodes, name, tags)
      }
    }
  }

  load(items, name, tags)
}

function getNames (name) {
  const tags = {}
  const uiTags = document.querySelectorAll('body *')
  for (let i = 0; i <= uiTags.length - 1; i++) {
    const item = uiTags[i].nodeName.toLowerCase()
    if (item.substr(0, 3) === name.toLowerCase()) { tags[item] = item }
  }
  return tags
}
const models = {
  'gt:ptrender': ptrender,
  'gt:ptrendernew': ptrendernew,
  'gt:cards': cardrender,
  'gt:ktree': tree,
  'gt:remark': gtremark,
  'gt:submit': gtsubmit,
  'gt:downloadhistory': downloadhistory,
  'gt:newestpt': newestpt,
  'gt:favorite': favorite,
  'gt:ptdetail': ptdetail,
  'gt:ptdetail2': ptdetail2,
  'gt:ptlist': ptlist,
  'gt:rencentlook': rencentlook,
  'gt:ptcompare': ptcompare,
  'gt:searlist2': ptlist2,
  'gt:rcmdbonear': bonear,
  'gt:rcmdbosimilar': bosimilar,
  'gt:rcmdboassociate': boassociate,
  'gt:rcmdtabs': gtrcmdtabs,
  'gt:rcmdsemantics': gtrcmdsemantics
  // 'gt:tree': eltree
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
      el: '#' + com.scope,
      store: store,
      data: com.data,
      methods: com.methods,
      // template: '<div></div>',
      // render: h => h('div', { id: '', style: '' }),
      components: model
    })
    // if (com.scope) { vm.$mount('#' + com.scope) }
    return vm
  },
  notification: ElementUI.Notification, // soUI.notification,
  message: ElementUI.Message, // soUI.message,
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
  },
  initViewModel: function (options) {
    options.store = store
    const vm = new Vue(options)
    return vm
  },
  store: store
}

const gtModels = {
  // 两种模式下都需要
  viewModels: {},
  getReturnModel: function (id, data) {
    // 获取返回值模型
    // 需要在每个模块内定义一个id参数。

    let mod = null
    if (!gtUI.isAutoLoadUI) { return mod }
    if (gtModels.viewModels[id]) {
      const r = gtModels.viewModels[id]
      mod = {
        target: {
          id: r.key,
          tagName: r.name
        },
        dataset: r.param,
        detail: `data`
      }
    }
    return mod
  }
}

window.onload = function () {
  const tag = 'gt:'
  if (gtUI.isAutoLoadUI) { loadUI(tag, getNames(tag)) }
}
// 分开变量的目的是为了解耦调用名称和打包名称。然后将无法预估大小的vm和主功能分开。
window.gtModels = gtModels
window.gtUI = gtUI
