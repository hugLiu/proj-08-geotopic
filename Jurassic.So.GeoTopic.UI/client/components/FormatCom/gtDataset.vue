<template>
    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-heading">
            <h5 v-html="param[0].title"></h5>
        </div>
        <div class="panel-body">
            <div v-bind:style="stylestr">
                <gt_dataset  :datasource="datasource">
                </gt_dataset>
            </div>
        </div>
    </div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    //import ajax from '../../utils/coment'
    export default {
        props: {
            param: Array,
            datasource: Array
        },
        data: function () {
            var _this = this;
            // ajax.getdata(this.param[0], function (data) {
            //     debugger
            //       var _data = data.reurl;
            //     _this.GetData(_data)
            // });
            return {
                stylestr:{
                    height:this.param[0].heights+'px'
                }
            }
        },
        methods:{
            GetData :function (datas) {
                  debugger;
                var obj = eval('(' + datas + ')');

                var datasList=[];
                var datasObj={};
                datasObj.name="表格1"
                datasObj.totalcount=obj.Rows.length
                var column=[],data=[];

                for(var i=0;i<obj.Columns.length;i++){
                    var colObj={};
                    colObj.field=obj.Columns[i];
                    colObj.text=obj.Columns[i];
                    column.push(colObj)
                }

                for(var j=0;j<obj.Rows.length;j++){
                    var dataObj={};
                    for(var k=0;k<obj.Columns.length;k++){
                        var field=obj.Columns[k]
                        dataObj[field]=obj.Rows[j][k];
                    }
                    data.push(dataObj)
                }
                datasObj.column=column;
                datasObj.data=data;
                datasList.push(datasObj)
                this.datasource=datasList;
            }
        },
        components: {
            gt_dataset: soUI.dataset
        }
    }
</script>
