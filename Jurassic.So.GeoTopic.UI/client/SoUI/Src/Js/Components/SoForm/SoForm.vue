<template>
    <div v-bind:style="style" v-if="!hidden">
        <!--         <form v-if="list.length>0" class="form-horizontal" role="form">
                    <span style="font-size:15px">{{data.groupname}}</span>
                    <form_row v-for="item in list" :data="item"  ref="form_row"></form_row>
                </form>
                   <div class="form-group"> 
                   <hr style="height:2px;border-color:rgb(102, 102, 102);" />
                 </div> -->

                 <form v-if="list.length>0" class="form-horizontal" role="form">
                   <fieldset>
                <legend>{{data.groupname}}</legend>
                <div class="form-group" style="margin:5px 0 5px 0">
                 <form_input v-for="item in list" :data="item"  ref="form_input"></form_input>
                 </div>
                </fieldset>
             </form>

    </div>
</template>

<script>
    import form_input from './FormInput.vue'
    export default{
        props: {
            id: {
                type: String,
                default: null
            },
            class: String,
            style: String,
            hidden: {
                type: Boolean,
                default: false
            },
            data: Object,
            onFormData: Function
        },
        data: function () {
            return {
               // list: this.GetRow()
               list:this.data.fields
            }
        },
        methods: {
            GetRow: function () {
                let list = this.data.fields;
                let rowlist = [];
                let rowArray = [];
                for (let i = 0; i < list.length; i++) {
                    rowArray.push(list[i]);
                    if (rowArray.length == 2 || i == list.length - 1) {
                        rowlist.push(rowArray);
                        rowArray = [];
                    }
                }
                return rowlist;
            },

            getFormData: function () {
                var _this = this;
                var texts = [];
                var rowtexts = [];
                 for (let i = 0; i < this.$refs.form_input.length; i++) {
                    texts.push( {text:this.$refs.form_input[i].inputText , name:this.$refs.form_input[i].name,
                        isok:this.$refs.form_input[i].isok,title:this.$refs.form_input[i].title, uitype:this.$refs.form_input[i].uitype});
                 }
                return texts;
            }

            
        },
        components: {
            form_input: form_input
        }
    }
</script>

<style scoped>
fieldset{
    padding:.35em .62em 0em;margin:0 2px;border:1px solid silver
}

legend{
    padding:0.2em;
    border:0;
    width:auto;
    margin-bottom: 0px;
}
</style>

