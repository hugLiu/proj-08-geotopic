<template>
    <div class="ptdetail2">
        <detail2 :data="data" :showpreview="true" :showdownload="true"
                :style="style" :ischange="ischange">
            <div slot="mySlot"> 
                <ptrender :pram="jsonobj" :jsondetail="data" :issubmit="false" :isremarks="false" :heights="0"></ptrender>
            </div>
        </detail2>
    </div>
</template>
<script>
    import soUI from '../../SoUI/soUI';
    import ptrender from '../cellRender/gtPTRenderForDetail.vue'
    export default {
        props: {
            id: {
                type: String,
                default: null
            },
            iiid: String,
            style: {
                type: String,
                default: ""
            },
            //detaildataurl: String,
            onpreview: Function,
            ondownload: Function
        },
        data: function data() {
            return {
                data:null,
                ischange:0,
                jsonobj:null
            }
        },
        mounted: function () {
            this.getData();
        },
        methods: {
            //胡波GetMetaData方法
            getData: function () {
                let _param={
                    id:"G1",
                    type:"Present",
                    url:"/render/pt",
                    title:"",
                    param:{
                        filter:{
                            $and:[{IIId:this.iiid}]
                        }
                    }
                };
                let _this = this;
                let _iiid = _this.iiid;
                $.ajax({
                    //url: this.detaildataurl,
                    url:"/PTDetail/GetDetailData",
                    type: "get",
                    async: true,
                    data: { iiid: _iiid },
                    success: function (result) {
                        //var dataObj=JSON.parse(result);
                        var dataObj=eval("("+result+")");
                        //dataObj.thumbnailUrl="data:image/jpg;base64,"+dataObj.thumbnailUrl;
                        _this.data=dataObj;
                        _this.jsonobj=_param;
                    }
                });
            }
        },
        components: {
            detail2: soUI.detail2,
            ptrender:ptrender
        }
    }
</script>
