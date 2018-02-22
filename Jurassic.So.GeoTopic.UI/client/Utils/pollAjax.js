import store from '../store/index'

// 调用控制器的方法也需要封装，统一调用和处理异常。
// Ajax获取Api服务的统一方法：包含获取域名和Token处理。
function PollAjax (ajaxPram) {
  function tryError (err) {
    if (typeof err === 'string') {
      const statusText = err
      err = { statusText: statusText }
    }
    switch (err.statusText) {
      case 'Error':
            // gtUI.notification.error({message:"错误",description:"Error", duration:outTime});
        gtUI.message({ message: 'Error', type: 'error' })
        break
      case 'Not Authenticated':
                // 服务器没有开启token验证，这里token未调试！！！
                // 服务器封装返回错误信息的格式不详，需要调试！！！
        Vue.http.get('/Token/CleanToken', { token: token })
                // 因为token失效，重新刷新页面强制重新获取，这里的这种方式可以考虑修改掉，给当前模块自己处理。
        location.reload()
        break
      case 'Internal Server Error':
                // gtUI.notification.error({message:"错误",description:err.data.Message, duration:outTime});
        gtUI.message({ message: '错误', type: 'error' })
        break
      case 'No Transport':
                // gtUI.notification.error({message:"错误",description:"No Transport", duration:outTime});
        gtUI.message({ message: err.data.Message, type: 'error' })
        break
      case 'Token Error':
                // gtUI.notification.error({message:"错误",description:"未获取到授权！", duration:outTime});
        gtUI.message({ message: '未获取到授权！', type: 'error' })
        break
      case 'Host Error':
                // gtUI.notification.error({message:"错误",description:"未获取到域名！", duration:outTime});
        gtUI.message({ message: '未获取到域名！', type: 'error' })
        break

      default:
                // gtUI.notification.error({message:"错误",description:err.message, duration:outTime});
        gtUI.message({ message: err.message, type: 'error' })
                // ajaxPram.error(err);
    }
  };

    // 内部调用，不需要太多参数验证。
    // 获取域名
  let host = store.state.apiHost
  let token = store.state.apiToken
    // -------------------调试去掉-------
    // host = "http://192.168.1.152:8077/API";
    // Vue.http.get("/Settings/GetApiHost").then((result)=>{
    //             host = result.data;
    //     });
  if (host.length === 0) {
    $.ajax({
      url: store.state.siteUrl + '/Settings/GetApiHost',
        // data: param,
      type: 'get',
      async: false,
        // cache: false,
      success: function (data) {
        host = data
      }
    })
    if (!host) {
      tryError('Host Error')
      return
    }
    store.state.apiHost = host
  }

    // 获取Token
    // Vue.http.get("/Token/GetToken").then((result)=>{
    //             token = result.data;
    //     });
  if (token.length === 0) {
    $.ajax({
      url: store.state.siteUrl + '/Token/GetToken',
        // data: param,
      type: 'get',
      async: false,
        // cache: false,
      success: function (data) {
        token = data
      }
    })
    if (!token) {
      tryError('Token Error')
      return
    }
    store.state.apiToken = token
  }
    // else{
    //     Vue.http.headers.common.Authorization = "Bearer " + token;
    // }
    // if(ajaxPram.async){
    //         //----------------------------
    //         if(ajaxPram.type.toLowerCase()=="get"){
    //             let pram = null;
    //             for(let item in ajaxPram.data){
    //                 if(pram) pram += ("&"+ item +"="+ ajaxPram.data[item]);
    //                 else pram = ("?" +item +"="+ ajaxPram.data[item]);
    //             }
    //             Vue.http.get(host+ajaxPram.url+pram).then((result)=>{
    //                 ajaxPram.success(result);
    //             }).catch(function(result){
    //                 tryError(result);
    //             })
    //         }

    //         if(ajaxPram.type.toLowerCase()=="post"){
    //             Vue.http.post(host+ajaxPram.url,ajaxPram.data).then((result)=>{
    //                 ajaxPram.success(result);
    //             }).catch(function(result){
    //                 tryError(result);
    //             })
    //         }
    //     }else{
  $.ajax({
    url: host + ajaxPram.url,
    type: ajaxPram.type,
    data: ajaxPram.data,
    contentType: ajaxPram.contentType != null ? ajaxPram.contentType : 'application/x-www-form-urlencoded',
    dataType: ajaxPram.dataType,
    async: ajaxPram.async != null ? ajaxPram.async : true,
    headers: {
      'Authorization': 'Bearer ' + token
    },
    success: function (result) {
      ajaxPram.success(result)
    },
    error: function (ex) {
      gtUI.message({ message: "服务'" + ajaxPram.url + "'错误 - " + JSON.parse(ex.responseText).message, type: 'error' })
                    // tryError(ex.statusText);
    }
  })
        // }
}

export default PollAjax
