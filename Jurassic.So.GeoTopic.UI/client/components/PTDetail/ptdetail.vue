<template>
    <div class="ptdetail">
        <detail :data="data" :showpreview="true" :showdownload="true"
                :style="style" :ischange="ischange">
            <div slot="mySlot">
            <!-- heights=250 -->
                <ptrender :pram="jsonobj" :issubmit="false" :isremarks="false" :heights="250"></ptrender>
            </div>
        </detail>
    </div>
</template>
<script>
    import soUI from '../../SoUI/soUI';
    import ptrender from '../cellRender/GtPTRender.vue'
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
            detail: soUI.detail,
            ptrender:ptrender
        }
    }
</script>
