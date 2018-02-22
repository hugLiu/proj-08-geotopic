'use strict'
// 切换开发生产模式
process.env.NODE_ENV = 'production'
// process.env.NODE_ENV = 'development'

const path = require('path')
const webpack = require('webpack')
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const FriendlyErrors = require('friendly-errors-webpack-plugin')
const _ = require('./utils')

var gtdll = {
  entry: {
    vuedll: ['vue', 'vue-resource', 'vuex'],
    elementui: ['element-ui', 'element-theme-default']
  },
  output: {
    path: _.outputPath,
    filename: '[name].js',
    publicPath: '/Content/gtui/',
    library: '[name]_[hash]'
  },
  devtool: 'source-map',
  resolve: {
    extensions: ['.js', '.vue', '.css', '.json'],
    alias: {
      'vue$': 'vue/dist/vue.common.js'
      // 'vue-resource$': 'vue-resource/dist/vue-resource.es2015.js'
    }
  },
  module: {
    loaders: [
      {
        test: /\.vue$/,
        loaders: ['vue-loader']
      },
      {
        test: /\.js$/,
        loaders: ['babel-loader'],
        exclude: [/node_modules/]
      },
      {
        test: /\.es6$/,
        loaders: ['babel-loader']
      },
      {
        test: /\.(ico|jpg|png|gif|eot|otf|webp|ttf|woff|woff2)(\?.*)?$/,
        loader: 'file-loader',
        query: {
          // name: 'static/media/[name].[hash:8].[ext]'
          name: 'media/[name].[ext]'
        }
      },
      {
        test: /\.svg$/,
        loader: 'raw-loader'
      }
    ]
  },
  plugins: [
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify(process.env.NODE_ENV)
    }),
    new webpack.LoaderOptionsPlugin(_.loadersOptions()),
    new ExtractTextPlugin('[name].styles.css'),
    new webpack.NoEmitOnErrorsPlugin(),
    new FriendlyErrors(),
    new webpack.DllPlugin({
      context: __dirname,
      path: path.join(_.outputPath, '[name].dll-manifest.json'),
      name: '[name]_[hash]'
    })
  ]
}
if (process.env.NODE_ENV === 'production') {
  gtdll.plugins.push(new webpack.optimize.UglifyJsPlugin({
    sourceMap: true,
    compress: {
      warnings: false
    },
    output: {
      comments: false
    }
  }))
}

// extract css in standalone css files
_.cssProcessors.forEach(processor => {
  let loaders
  if (processor.loader === '') {
    loaders = ['postcss-loader']
  } else {
    loaders = ['postcss-loader', processor.loader]
  }
  gtdll.module.loaders.push({
    test: processor.test,
    loader: ExtractTextPlugin.extract({
      use: [_.cssLoader].concat(loaders),
      fallback: 'style-loader'
    })
  })
})

// minimize webpack output
gtdll.stats = {
  // Add children information
  children: false,
  // Add chunk information (setting this to `false` allows for a less verbose output)
  chunks: false,
  // Add built modules information to chunk information
  chunkModules: false,
  chunkOrigins: false,
  modules: false
}

module.exports = gtdll
