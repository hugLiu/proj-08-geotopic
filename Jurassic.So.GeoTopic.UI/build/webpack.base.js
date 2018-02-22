'use strict'
const path = require('path')
const webpack = require('webpack')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const CopyWebpackPlugin = require('copy-webpack-plugin')
const ProgressPlugin = require('webpack/lib/ProgressPlugin')
const FriendlyErrors = require('friendly-errors-webpack-plugin')
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const config = require('./config')
const _ = require('./utils')

const base = {
  entry: {
    gtui: './client/app.js'
    // vender: ['vue', 'vue-resource', 'vuex', 'element-ui', 'element-theme-default']
  },
  output: {
    path: _.outputPath,
    filename: '[name].js',
    publicPath: config.publicPath
  },
  devtool: 'source-map',
  performance: {
    hints: process.env.NODE_ENV === 'production' ? 'warning' : false
  },
  resolve: {
    extensions: ['.js', '.vue', '.css', '.json'],
    alias: {
      'vue$': 'vue/dist/vue.common.js',
      // 'vue-resource$': 'vue-resource/dist/vue-resource.es2015.js',
      root: path.join(__dirname, '../client'),
      components: path.join(__dirname, '../client/components')
    },
    modules: [
      _.cwd('node_modules'),
      // this meanse you can get rid of dot hell
      // for example import 'components/Foo' instead of import '../../components/Foo'
      _.cwd('client')
    ]
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
    new ProgressPlugin(),
    new webpack.NoEmitOnErrorsPlugin(),
    new FriendlyErrors(),
    // new HtmlWebpackPlugin({
    //   title: config.title,
    //   template: path.resolve(__dirname, 'index.html'),
    //   filename: _.outputIndexPath
    // }),
    new ExtractTextPlugin('[name].styles.css'),
    new webpack.optimize.CommonsChunkPlugin({
      names: ['manifest']
    }),
    new webpack.LoaderOptionsPlugin(_.loadersOptions()),
    // new CopyWebpackPlugin([
    //   {
    //     from: _.cwd('./dist'),
    //     // to the roor of dist path
    //     to: './'
    //   }
    // ]),
    new webpack.DllReferencePlugin({
      context: __dirname,
      manifest: require(path.join(__dirname, '../dist/vuedll.dll-manifest.json'))
    }),
    new webpack.DllReferencePlugin({
      context: __dirname,
      manifest: require(path.join(__dirname, '../dist/elementui.dll-manifest.json'))
    })
  ],
  target: _.target
}

// extract css in standalone css files
_.cssProcessors.forEach(processor => {
  let loaders
  if (processor.loader === '') {
    loaders = ['postcss-loader']
  } else {
    loaders = ['postcss-loader', processor.loader]
  }
  base.module.loaders.push({
    test: processor.test,
    loader: ExtractTextPlugin.extract({
      use: [_.cssLoader].concat(loaders),
      fallback: 'style-loader'
    })
  })
})

module.exports = base
