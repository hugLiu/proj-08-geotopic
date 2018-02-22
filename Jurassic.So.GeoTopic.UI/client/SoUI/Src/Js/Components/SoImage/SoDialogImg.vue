<template>
	<div>
		<img :src="url" v-bind:style="styleobj+';width:100%;'" @click="imgClick" />
		<el-dialog v-model="isShow" top="5%" size="large" @close="Close()">
			<soimg v-if="isLoad" :url="url" ref="imgCom"></soimg>
		</el-dialog>
	</div>
</template>
<script type="text/javascript">
	import { Dialog } from 'element-ui'
	import soimg from './SoImage.vue'
	export default {
		props: {
			url: String,
			styleobj: Object
		},

		data: function () {
			let _this = this;
			return {
				isShow: false,
				isLoad: false
			}
		},
		methods: {
			imgClick: function () {
				this.isShow = true;
				this.isLoad = true;
			},

			Close: function () {
				this.isShow = false;
				this.isLoad = false;
				//this.$refs.imgCom.$destroy();
			},
		},

		components: {
			"el-dialog": Dialog,
			"soimg": soimg
		},
	}
</script>