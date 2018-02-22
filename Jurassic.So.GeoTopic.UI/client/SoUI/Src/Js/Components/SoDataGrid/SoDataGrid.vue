<template>
    <div :id="_id" v-show="!hidden">
        <div v-if="!!_data">
            <table class="table table-bordered table-hover vue-table template-table" style="table-layout:fixed;word-wrap:break-word;">
                <thead>
                    <tr>
                        <th v-show="showcheckbox" class="vue-table-cell-icon">
                            <input type="checkbox" @change.stop.prevent="mCheckboxChange($event)" v-model="_mCheckboxChecked"/>
                        </th>
                        <th v-for="(f, index) of _data.fields" class="vue-table-th vue-table-cell-field" @click="sort(index, $event)" v-if="f.visible">  
                            <span v-html="f.title"></span>
                            <span v-if="f.sortable">
                                <div class="icon-sort" :class="{'icon-sort-up':f.ascendingOrder,'icon-sort-down':f.ascendingOrder == false,'icon-sort-disabled':f.ascendingOrder == null}"></div>
                            </span> 
                            <span class="dropdown-icon"><dropdowntemplate :itemlist="_data.fields"></dropdowntemplate></span>
                        </th> 
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="n in _pageSize" v-if="(_pageIndex - 1) * _pageSize + n <= _count" @click.stop="clickRow(n, $event)">
                        <td v-show="showcheckbox" class="vue-table-cell-icon">
                            <checkboxtemplate :keys="_data.fields" :values="_data.records[n-1]" :selectedkeys="_data.selectedKeys" :onselect="checkboxSelected"></checkboxtemplate> 
                        </td>
                        <td v-for="f of _data.fields" class="vue-table-cell-record" v-if="f.visible">
                            <div v-if="f.operationTemplate"><strparsetemplate :strtemplate="f.operationTemplate" :keys="_data.fields" :values="_data.records[n-1]"></strparsetemplate></div>
                            <div v-else>{{_data.records[n-1][f.name]}}</div>
                        </td>
                    </tr>
                </tbody>
                <tfoot> 
                    <tr>
                        <td :colspan="_footLength" class="vue-table-cell-pager">
                            <div class="vue-table-count" v-if="_count > 0">共 {{_count}} 条</div>
                            <div class="vue-table-pager" v-if="_count > _pageSize"><page :pagesize="_pageSize" :count="_count" :items="5" :onselectitem="selectPage" ref="page"></page></div>
                        </td>
                    </tr>
                </tfoot>
            </table> 
        </div> 
    </div>
</template>

<script>
    import page from '../SoPageTemplate/SoPageTemplate.vue'
    import checkboxtemplate from '../SoCheckboxTemplate/SoCheckboxTemplate.vue'
    import strparsetemplate from '../SoStrParseTemplate/SoStrParseTemplate.vue'
    import dropdowntemplate from '../SoDropDownTemplate/SoDropDownTemplate.vue'

    export default {
        props: {
            id: {
                type: String,
                default: null
            },
            hidden: {
                type: Boolean,
                default: false
            },
            data: Object,
            count: {
                type: Number,
                default: 0
            },
            pagesize: {
                type: Number,
                default: 0
            },
            showcheckbox: {
                type: Boolean,
                default: false
            },
            onsort: Function,
            onclickrow: Function,
            onselectpage: Function
        },
        computed: {
            _data() {
                return this.data
            },
            _count() {
                let temp = 0
                if (this.count) {
                    temp = this.count
                } else if (!!this.data && this.data.records && this.data.records.length) {
                    temp = this.data.records.length
                } else {
                    temp = 0
                }
                return temp
            },
            _pageSize() {
                let temp = 0
                if (this.pagesize) {
                    temp = this.pagesize
                } else if (!!this.data && this.data.records && this.data.records.length) {
                    temp = this.data.records.length
                } else {
                    temp = 0
                }
                return temp
            },
            _footLength() {
                let temp = 0
                if (!!this.data && this.data.fields) {
                    let fields = this.data.fields,
                        len = fields.length
                    for (let value of fields) {
                        if (!value.visible) {
                            len = len - 1
                        }
                    }
                    temp = this.showcheckbox ? len + 1 : len
                }
                return temp
            }
        },
        watch: {
            data() {
                this.pageActiveItemReset() // set current page to page 1 
                this.cacheDataRecords() // set records cache
                this.selectionSetAttribute() // set arrtribute selection
            }
        },
        methods: {
            sort(fieldIndex, event) {
                if (event.target.className == "icon-reorder") { // dropdown event not sort event
                    return
                }
                this._pageIndex = 1
                let hasSortField = false,
                    fields = this._data.fields
                if (!fields[fieldIndex].sortable) {
                    return
                }
                fields.forEach((value, index) => {
                    if (value.sortable) {
                        if (index == fieldIndex) {
                            if (value.ascendingOrder) {
                                value.ascendingOrder = false
                            } else {
                                value.ascendingOrder = true
                            }
                        } else {
                            value.ascendingOrder = null
                        }
                        hasSortField = true
                    }
                })
                if (!hasSortField) {
                    return
                }
                let $detail = {},
                    sortField = fields[fieldIndex].name,
                    ascendingOrder = fields[fieldIndex].ascendingOrder
                $detail.sortField = sortField
                $detail.ascendingOrder = ascendingOrder
                $detail.selectedKeys = this._data.selectedKeys
                if (this.onsort) {
                    if (this.id) {
                        let returnRecords = this.onsort(soModels.getReturnModel(this._id, $detail)) // invoke common function return additional info 
                        returnRecords && this.updateRecords(returnRecords)
                    } else {
                        this.onsort($detail)
                    }
                } else {
                    let tempRecords = sessionStorage.getItem(this._id)
                    if (tempRecords) {
                        tempRecords = this.sortHelper(JSON.parse(tempRecords), sortField, ascendingOrder)
                        this.updateRecords(tempRecords)
                    }
                }
                this._mCheckboxChecked = false
                this.pageActiveItemReset()
            },
            clickRow(n, event) {
                if (event.target.tagName != "TD") { // other event not row event
                    return
                }
                let index = n - 1
                if (this.onclickrow) {
                    let fields = this._data.fields,
                        record = this._data.records[index],
                        $detail = {},
                        temp = []
                    for (let field of fields) {
                        let name = field.name
                        if (record[name]) {
                            $detail[name] = record[name]
                        }
                        if (field.key) {
                            temp.push(record[name])
                        }
                    }
                    $detail.key = temp.join('|')
                    if (this.id) {
                        this.onclickrow(soModels.getReturnModel(this._id, $detail))
                    } else {
                        this.onclickrow($detail)
                    }
                }
            },
            selectPage(pageIndex) {
                this._pageIndex = pageIndex
                let $detail = {}
                $detail.pager = {
                    "pageSize": this.pagesize,
                    "pageIndex": pageIndex
                }
                let ascendingOrder, sortField
                for (let value of this._data.fields) {
                    let visible = value.visible,
                        sortable = value.sortable
                    ascendingOrder = value.ascendingOrder
                    if (visible && sortable && ascendingOrder != null) {
                        sortField = value.name
                        break
                    }
                }
                $detail.sortField = sortField
                $detail.ascendingOrder = ascendingOrder
                $detail.selectedKeys = this._data.selectedKeys
                if (this.onselectpage) {
                    if (this.id) {
                        let returnRecords = this.onselectpage(soModels.getReturnModel(this._id, $detail))
                        returnRecords && this.updateRecords(returnRecords)
                    } else {
                        this.onselectpage($detail)
                    }
                } else {
                    let tempRecords = sessionStorage.getItem(this._id)
                    if (tempRecords) {
                        tempRecords = JSON.parse(tempRecords)
                        if (sortField) {
                            tempRecords = this.sortHelper(tempRecords, sortField, ascendingOrder)
                        }
                        let start = this.pagesize * (pageIndex - 1),
                            end = this.pagesize * pageIndex,
                            sliceRecords = tempRecords.slice(start, end)
                        if (sliceRecords.length < 1) return
                        this.updateRecords(sliceRecords)
                    }
                }
                this._mCheckboxChecked = false
            },
            mCheckboxChange(event) { //select all  
                let targets = event.currentTarget,
                    fields = this._data.fields,
                    records = this._data.records.slice(0, this._pageSize)
                if (targets.checked) {
                    for (let r of records) {
                        let keyVal = "",
                            temp = []
                        for (let f of fields) {
                            if (f.key) {
                                let name = f.name
                                temp.push(r[name])
                            }
                        }
                        keyVal = temp.join('|')
                        if (keyVal && this._data.selectedKeys.indexOf(keyVal) < 0) {
                            this._data.selectedKeys.push(keyVal)
                        }
                    }
                } else {
                    for (let r of records) {
                        let keyVal = "",
                            temp = []
                        for (let f of fields) {
                            if (f.key) {
                                let name = f.name
                                temp.push(r[name])
                            }
                        }
                        keyVal = temp.join('|')
                        let index = this._data.selectedKeys.indexOf(keyVal)
                        if (index > -1) {
                            this._data.selectedKeys.splice(index, 1)
                        }
                    }
                }
                this.selectionSetAttribute()
            },
            checkboxSelected(key) {
                if (this.showcheckbox) {
                    let index = this._data.selectedKeys.indexOf(key)
                    if (index > -1) {
                        this._data.selectedKeys.splice(index, 1)
                    } else {
                        this._data.selectedKeys.push(key)
                    }
                    this.selectionSetAttribute()
                }
            },
            sortHelper(arr, field, order) {
                let refer = [],
                    result = [],
                    self = this
                arr.forEach((value, index) => {
                    let temp = (isNaN(value[field]) ? value[field] : self.padLeft(value[field], 15)) + ':' + index
                    refer.push(temp)
                })
                refer.sort()
                if (!order) {
                    refer.reverse()
                }
                for (let value of refer) {
                    let index = value.split(':')[1]
                    result.push(arr[index])
                }
                return result
            },
            padLeft(field, width) {
                let len = field.toString().length
                while (len < width) {
                    field = "0" + field
                    len = len + 1
                }
                return field
            },
            pageActiveItemReset() {
                if (this.$refs && this.$refs.page) {
                    this.$refs.page.activeItem = 1
                }
            },
            selectionSetAttribute() {
                if (!!this._data && this._data.selectedKeys) {
                    $("#" + this._id)[0].setAttribute("selection", this._data.selectedKeys)
                }
            },
            cacheDataRecords() {
                if (!!this.data && this.data.records) {
                    sessionStorage.removeItem(this._id)
                    sessionStorage.setItem(this._id, JSON.stringify(this.data.records))
                }
            },
            updateRecords(newRecords) {
                let fields = this._data.fields
                this._data.records.forEach((record, index) => {
                    if (index < newRecords.length) {
                        fields.forEach(field => {
                            let name = field.name
                            record[name] = newRecords[index][name]
                        })
                    }
                })
            }
        },
        components: {
            page,
            checkboxtemplate,
            strparsetemplate,
            dropdowntemplate
        },
        beforeMount() {
            if (this.id) {
                this._id = this.id
            } else {
                this._id = "grid-" + Date.now()
            }
            this._pageIndex = 1
            this._mCheckboxChecked = false
        },
        mounted() {
            this.selectionSetAttribute()
            this.cacheDataRecords()
        }
    }
</script>

<style scoped>
    .vue-table-cell-pager {
        text-align: right;
        overflow: hidden;
    }
    
    .vue-table-count {
        float: left;
        font-size: medium;
        margin: 15px 0;
    }
    
    .vue-table-pager {
        float: right;
    }
    
    .vue-table thead,
    .vue-table tfoot {
        background-color: #f8f8f8;
    }
    
    .vue-table-cell-field {
        text-align: center;
    }
    
    .vue-table-cell-record {
        text-align: left;
    }
    
    .vue-table-cell-icon {
        width: 35px;
        text-align: center;
        font-size: 17px;
    }
    
    .vue-table-th {
        cursor: pointer;
        position: relative;
    }
    
    .icon-sort-disabled {
        color: #999;
    }
    
    .vue-table-empty {
        text-align: center;
        line-height: 60px!important;
    }
    
    .icon-sort:before,
    .icon-sort-up:before,
    .icon-sort-down:before {
        font-size: 15px;
        margin-left: 35px;
    }
    
    .dropdown-icon {
        float: right;
        position: absolute;
    }
    
    .table-bordered>thead>tr>td,
    .table-bordered>thead>tr>th {
        border-bottom-width: 0px !important;
    }
</style>