const models = {
    trim: function (str) { //删除左右两端的空格
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    ltrim: function (str) { //删除左边的空格
        return str.replace(/(^\s*)/g, "");
    },
    rtrim: function (str) { //删除右边的空格
        return str.replace(/(\s*$)/g, "");
    }
}
export default models;
