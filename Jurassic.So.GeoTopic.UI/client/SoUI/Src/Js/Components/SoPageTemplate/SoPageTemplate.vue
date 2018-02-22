<template>
    <div>
        <nav class="boot-nav">
            <ul class="pagination boot-item">
                <li v-if="_showFirstPrev">
                    <a href="##" @click="onFirst">
                        <span aria-hidden="true">首页</span>
                    </a>
                </li>
                <li v-if="_showFirstPrev">
                    <a href="##" @click="onPrev">
                        <span aria-hidden="true">上一页</span>
                    </a>
                </li>
                <li v-for="item in _items" :class="activeItem === item ? 'active' : ''">
                    <a href="##" v-text="item" @click="onCurrent(item)"></a>
                </li>
                <li v-if="_showNextLast">
                    <a href="##" @click="onNext">
                        <span aria-hidden="true">下一页</span>
                    </a>
                </li>
                <li v-if="_showNextLast">
                    <a href="##" @click="onLast">
                        <span aria-hidden="true">末页</span>
                    </a>
                </li>
            </ul>
        </nav>
    </div>
</template>

<script>
    export default {
        props: {
            pagesize: {
                type: Number,
                default: 10
            },
            count: {
                type: Number,
                default: 1
            },
            items: {
                type: Number,
                default: 10
            },
            onselectitem: Function
        },
        data() {
            return {
                activeItem: 1
            }
        },
        computed: {
            _showFirstPrev() {
                return this.activeItem > 1
            },
            _showNextLast() {
                return this.activeItem < Math.ceil(this.count / this.pagesize)
            },
            _items() {
                let temp = [],
                    maxItem = Math.ceil(this.count / this.pagesize),
                    len = maxItem > this.items ? this.items : maxItem,
                    midIndex = this.items % 2 == 0 ? Math.ceil((this.items + 1) / 2) : Math.ceil(this.items / 2)
                for (let i = 1; i <= len; i++) {
                    temp.push(i)
                }
                if (this.activeItem > midIndex) {
                    let span = this.activeItem - midIndex
                    for (let i = 0; i < span && temp[temp.length - 1] < maxItem; i++) {
                        temp.shift(temp[i])
                        temp.push(len + 1 + i)
                    }
                }
                return temp
            }
        },
        watch: {
            activeItem() {
                this.selectItem()
            }
        },
        methods: {
            onFirst() {
                this.activeItem = 1
            },
            onPrev() {
                this.activeItem = this.activeItem - 1
            },
            onCurrent(item) {
                this.activeItem = item
            },
            onNext() {
                this.activeItem = this.activeItem + 1
            },
            onLast() {
                this.activeItem = Math.ceil(this.count / this.pagesize)
            },
            selectItem() {
                this.onselectitem && this.onselectitem(this.activeItem);
            }
        }
    }
</script>

<style scoped>
    .boot-select {
        float: right;
        width: 80px;
    }
    
    .pagination {
        margin: 10px 0;
    }
    
    .boot-nav {
        float: left;
    }
    
    .boot-item {
        display: inline-block;
        vertical-align: middle;
    }
    
    .item-total {
        display: inline-block;
        vertical-align: middle;
    }
</style>