<template>
    <input type="checkbox" v-model="_defaultChecked" @change="select()" />
</template>

<script>
    export default {
        props: {
            keys: Array,
            values: Object,
            selectedkeys: Array,
            onselect: Function
        },
        computed: {
            _defaultChecked() {
                let temp = false,
                    selection = [],
                    tempKey = []
                if (this.keys && !!this.values && this.selectedkeys) {
                    for (let value of this.keys) {
                        if (value.key) {
                            let name = value.name
                            selection.push(this.values[name])
                        }
                    }
                    for (let value of this.selectedkeys) {
                        tempKey.push(value)
                    }
                }
                let strKeys = selection.join('|')
                temp = tempKey.indexOf(strKeys) > -1
                return temp
            }
        },
        methods: {
            select() {
                if (this.onselect) {
                    let selection = [],
                        $keys = this.keys,
                        $values = this.values
                    for (let i in $keys) {
                        if ($keys[i].key) {
                            let name = $keys[i].name
                            selection.push($values[name])
                        }
                    }
                    this.onselect(selection.join('|'))
                }
            }
        }
    }
</script>