<template>
    <so_remark
            :classname="classname"
            :style="style"
            :hidden="hidden"
            :data="data"
            :count="count"
            :user="user"
            :size="size"
            :ondelete="ondelete"
            :onreply="onreply"
            :onloadmore="onloadmore"
            :onpraise="onpraise"
            :onfilter="onfilters"
    >
    </so_remark>
</template>
<script>
    import  soUI from '../../SoUI/soUI';
    export  default{
        props: {
            id:String,
            scoap:String,
            naturekey:String,
            classname:{
                type:String,
                default:"remark"
            },
            style:{
                type:String,
                default:"width:100%;height:100%"
            },
            hidden:{
                type:Boolean,
                default:false
            }
        },
        data() {
            return {
                size:10,
                count:0,
                user:this.getUserInfo(),
                data:null
            }
        },
        created:function(){
            this.initData();
        },
        methods: {
            getUserInfo:function () {
               var userinfo;
                $.ajax({
                    url:"/Remark/GetUserInfo",
                    async:false,
                    type:"post",
                    success:function (data) {
                        userinfo={
                            userId:data.UserId,
                            userName:data.UserName
                        }
                    },
                    error:function () {
                        return;
                    }
                })
                return userinfo;
            },
            initData:function () {
                this.data=this.getData(this.scoap,this.naturekey,0,this.size,"All")
            },
            ondelete:function (data) {
                var self=this;
                $.ajax({
                    url:"/Remark/DeleteRemark",
                    data:{Id:data.id},
                    async:false,
                    type:"post",
                    success:function () {
                        self.data=self.getData(self.scoap,self.naturekey,0,self.size,"All");
                    }
                })

            },
            onreply:function (data) {
                var dataSouce={
                    Scoap:this.scoap,
                    Naturekey:this.naturekey,
                    Comment:data.comments,
                    UserId:data.userId,
                    UserPhotoUrl:data.photoUrl,
                }
                var self=this;
                $.ajax({
                    url:"/Remark/AddRemark",
                    data:{model:dataSouce},
                    async:false,
                    type:"post",
                    success:function () {
                        self.data=self.getData(self.scoap,self.naturekey,0,self.size,"All");
                    }
                })
            },
            onloadmore:function (data) {
                return this.getData(this.scoap,this.naturekey,data.pager.pageIndex,data.pager.pageSize,data.filterOption);
            },
            onpraise:function (data) {
                //var self=this;
                var praise=data.praised.toString();
                $.ajax({
                    url:"/Remark/PraiseRemark",
                    data:{id:data.id,userId:data.userId,praised:praise},
                    async:false,
                    type:"post",
                    success:function () {
                    }
                })
            },
            onfilters:function (filter) {
                this.data=this.getData(this.scoap,this.naturekey,0,this.size,filter)
            },
            getData:function (scoap,naturekey,index,size,filter) {
                if(this.scoap==null||this.naturekey==null||this.id==null){
                    return;
                }
                var dataSouce=[];
                var self=this;
                $.ajax({
                    url:"/Remark/GetRemarkList",
                    data:{scoap:scoap,naturekey:naturekey,index:index,size:self.size,filter:filter},
                    type:"post",
                    async:false,
                    success:function (datas) {
                        self.count=datas.total;
                        for(var i=0;i<datas.data.length;i++){
                           var model={
                                id:datas.data[i].Id,
                                remarkerId:datas.data[i].UserId,
                                userPhotoUrl:datas.data[i].UserPhotoUrl,
                                comments:datas.data[i].Comment,
                                praiseUsers:datas.data[i].UserProfile1
                            };
                            dataSouce.push(model);
                        }
                    }
                })
                return dataSouce;
            }
        },
        components: {
            so_remark:soUI.remark
        }
    }
</script>
