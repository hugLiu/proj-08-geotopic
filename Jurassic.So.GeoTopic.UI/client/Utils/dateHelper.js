const models = {
    //格式化CST日期的字串
    formatCSTDate: function (strDate, format) {
        return this.formatDate(new Date(strDate), format);
    },
    //格式化日期,
    formatDate: function (date, format) {
        var paddNum = function (num) {
            num += "";
            return num.replace(/^(\d)$/, "0$1");
        }
        //指定格式字符
        var cfg = {
            yyyy: date.getFullYear() //年 : 4位
            , yy: date.getFullYear().toString().substring(2)//年 : 2位
            , M: date.getMonth() + 1  //月 : 如果1位的时候不补0
            , MM: paddNum(date.getMonth() + 1) //月 : 如果1位的时候补0
            , d: date.getDate()   //日 : 如果1位的时候不补0
            , dd: paddNum(date.getDate())//日 : 如果1位的时候补0
            , hh: date.getHours()  //时
            , mm: date.getMinutes() //分
            , ss: date.getSeconds() //秒
        }
        format || (format = "yyyy-MM-dd hh:mm:ss");
        return format.replace(/([a-z])(\1)*/ig, function (m) { return cfg[m]; });
    },
    //获得当前日期的前一天
    getYesterday: function (date) {
        var yesterday_milliseconds = date.getTime() - 1000 * 60 * 60 * 24;
        var yesterday = new Date();
        yesterday.setTime(yesterday_milliseconds);
        return yesterday;
    },
    //获得当前日期的前一月
    getLastMonth: function (date) {
        var daysInMonth = new Array([0], [31], [28], [31], [30], [31], [30], [31], [31], [30], [31], [30], [31]);
        var strYear = date.getFullYear();
        var strDay = date.getDate();
        var strMonth = date.getMonth() + 1;
        if (strYear % 4 == 0 && strYear % 100 != 0) {
            daysInMonth[2] = 29;
        }
        if (strMonth - 1 == 0) {
            strYear -= 1;
            strMonth = 12;
        }
        else {
            strMonth -= 1;
        }
        strDay = daysInMonth[strMonth] >= strDay ? strDay : daysInMonth[strMonth];
        return new Date(strYear, strMonth, strDay);
    },
    //获得当前日期的前一年
    getLastYear: function (date) {
        var strYear = date.getFullYear() - 1;
        var strDay = date.getDate();
        var strMonth = date.getMonth() + 1;
        return new Date(strYear, strMonth, strDay);
    }
}
export default models;
