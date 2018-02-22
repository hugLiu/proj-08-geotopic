<template>
    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-body">
            <div v-bind:style="stylestr">
                <piechart :data="pidata" :style="style">
                </piechart>
            </div>
        </div>
    </div>

    <div class="panel panel-default" style="margin-top:2px">
        <div class="panel-body">
            <div v-bind:style="stylestr">
                <hiechart :data="hidata" :style="style" :type="chart_type">
                </hiechart>
            </div>
        </div>
    </div>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    export default {
        props: {
            param: Array
        },

        data: function () {
            debugger
            return {
                pidata: {
                    "text": "主标题",
                    "subtext": "副标题",
                    "legend": ["中国", "美国", "欧盟", "俄罗斯", "日本"],
                    "data": [10000, 8000, 5000, 3000, 20050]
                },
                hidata: {
                    "text": "主标题",
                    "subtext": "副标题",
                    "legend": ["中国", "美国", "欧盟", "俄罗斯", "日本"],
                    "data": [[9100, 2700,1530, 400], [3000, 7900, 7500, 7000], [5000, 4900, 4700, 4200], [3000, 8900, 3100, 5400], [2250,3850, 5430, 7250]],
                    "axis": {
                        "xaxis": {
                            "type": "category",
                            "data": ["第一季度", "第二季度", "第三季度", "第四季度"]
                        },
                        "yaxis": {
                            "type": "value"
                        }
                    }
                },
                stylestr: {
                    height: this.param[0].heights /2+ 'px'
                },
                style: "width:100%;height:100%",
                chart_type:"bar"
            }
        },
        components: {
            piechart: soUI.piechart,
            hiechart: soUI.histogram
        }
    }
</script>