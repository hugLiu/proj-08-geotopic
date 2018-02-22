'use strict'
process.env.NODE_ENV = 'production'

const webpack = require('webpack')
const base = require('./webpack.base')

// add webpack plugins
base.plugins.push(
  new webpack.optimize.UglifyJsPlugin({
    sourceMap: true,
    compress: {
      warnings: false
    },
    output: {
      comments: false
    }
  })
)

// minimize webpack output
base.stats = {
  // Add children information
  children: false,
  // Add chunk information (setting this to `false` allows for a less verbose output)
  chunks: false,
  // Add built modules information to chunk information
  chunkModules: false,
  chunkOrigins: false,
  modules: false
}

module.exports = base
