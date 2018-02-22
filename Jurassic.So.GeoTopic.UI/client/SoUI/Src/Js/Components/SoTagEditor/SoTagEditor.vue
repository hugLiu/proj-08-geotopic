<template>
    <div>
        <div style="max-width: 900px;margin-bottom:8px">
<textarea v-bind:id="idstr"></textarea>
</div>
</div>
</template>
<script type="text/javascript">
    require('./lib/editor.js');
    import './lib/editor.css'
    export default {
        props: {
            value: String,
            ongetvalue: Function
        },
        data: function () {
            return {
                idstr: this.GetUiid()
            }
        },
        mounted: function () {
            let _this = this;
            $('#' + _this.idstr).tagEditor({
                placeholder: '',
                onChange: function (field, editor, tags) {
                    //let tagdata = $('#'+this.idstr).tagEditor('getTags')[0].tags;
                    _this.ongetvalue && _this.ongetvalue(tags);
                }
            });
        },
        methods: {
            GetUiid: function () {
                let s = [];
                let hexDigits = "0123456789abcdef";
                for (let i = 0; i < 36; i++) {
                    s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
                }
                s[14] = "4";
                s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); 
                s[8] = s[13] = s[18] = s[23] = "-";
                let uuid = s.join("");
                return uuid;
            }
        }
    }
</script>

<style scoped>
</style>