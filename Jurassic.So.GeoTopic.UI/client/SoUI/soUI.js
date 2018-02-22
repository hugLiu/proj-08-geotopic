// ----------------------yuanquan-----------------------
import sodropdown from './Src/Js/Components/SoDropdown/SoDropDown.vue'
import filter from './src/js/components/SoFilter/SoFilter.vue'
import datalist from './src/js/components/SoDataList/SoDataList.vue'
import searchlist from './src/js/components/SoSearchList/SoSearchList.vue'
import page from './src/js/components/SoPage/SoPage.vue'
import dlimg from './src/js/components/SoImage/SoDialogImg.vue'
// import  image  from   './src/js/components/SoImage/SoImage.vue'
import richtext from './src/js/components/SoRichText/SoRichText.vue'
import tree from './src/js/components/SoTree/SoTree.vue'
import soform from './src/js/components/SoForm/SoForm.vue'

// import './src/js/antd/style/index.less'
// import * as utils from './src/js/antd/utils'
// import notification from './Src/Js/Antd/components/notification'
// import message from './Src/Js/Antd/components/message'

// -----------------------leijing---------------------------
// import dataset from './src/js/components/DataSet/dataSet.vue'
import detail from './src/js/components/SoDetail/SoDetail.vue'
import detail2 from './src/js/components/SoDetail/SoDetail2.vue'
import top from './src/js/components/SoTop/SoTop.vue'

// ---------------------liwenxuan------------------------------
import remark from './src/js/components/SoRemark/SoRemark.vue'
// import linechart from "./src/js/components/SoLineChart/SoLineChart.vue"
// import piechart from "./src/js/components/SoPieChart/SoPieChart.vue"
// import histrogram from "./src/js/components/SoHistogram/SoHistogram.vue"
import gdb from './src/js/components/GdbShow/GdbShow.vue'
import datagrid from './src/js/components/SoDataGrid/SoDataGrid.vue'
import dataset from './src/js/components/SoDataSet/SoDataSet.vue'

const soUI = {
    // notification:notification,
    // message:message,
  dropdown: sodropdown,
  filter: filter,
  datalist: datalist,
  searchlist: searchlist,
  page: page,
    // image:image,
  dlimg: dlimg,
  richtext: richtext,
    // dataset:dataset,
  detail: detail,
  detail2: detail2,
  tree: tree,
  remark: remark,
  soform: soform,
    // linechart:linechart,
    // piechart:piechart,
    // histogram:histrogram,
  gdb: gdb,
  top: top,
  datagrid: datagrid,
  dataset: dataset
}

export default soUI
