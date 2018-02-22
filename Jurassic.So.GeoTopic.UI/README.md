# geotopic-ui

### 建立工作环境:

1. [安装Node和NPM](https://nodejs.org/en/)

2. 安装CNPM:
npm install -g cnpm --registry=https://registry.npm.taobao.org

3. 安装依赖模块:
cnpm install

### 本项目建立步骤:

1. 安装vue脚手架(vue-cli)
npm install -g vue-cli

2. 安装VUE脚手架vuepack
vue init egoist/vuepack geotopic-ui

### 开发和部署:

1. 生成项目依赖
npm run build-dll

2. 生成gtui调试版
npm run dev

3. 生成gtui发行版
npm run build
