
var path = require('path');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var ExtractTextPlugin = require("extract-text-webpack-plugin");

const plugins = [
    new HtmlWebpackPlugin({
        template: 'index.html',
        filename: 'Default.html'
    }),
    new ExtractTextPlugin("/bundle.css")
];

module.exports = {
    entry: './index.js',
    output: {
        filename: '/bundle.js',
        path: path.resolve(__dirname, '../')
    },
    devServer: {
        inline: true,
        hot: true,
        port: 7000,
        historyApiFallback: true,
        proxy: {
            '/api/*': {
                target: 'http://localhost:9966',
                secure: false
            }
        }
    },
    module: {
        loaders: [
            {
                test: /\.(js|jsx)?$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015', 'react'],
                    plugins: ["transform-object-rest-spread"]
                }
            },
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract({ fallback: 'style-loader', use: 'css-loader' })//"style-loader!css-loader"
            },
        ]
    },
    plugins
};