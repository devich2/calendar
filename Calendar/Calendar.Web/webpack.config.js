const path = require('path');

module.exports = (env, argv) => {
    return {

        entry: ['./Client/Scripts/main.js', './Client/Sass/site.scss'],
        output: {
            path: path.resolve(__dirname, './wwwroot'),
            filename: "js/bundle.js",
        },
        devtool: 'source-map',
        module: {
            rules: [
                {
                    test: /\.js$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader"
                    }
                },
                {
                    test: /\.scss$/,
                    use: [
                        {
                            loader: 'file-loader',
                            options: {
                                name: '[name].css',
                                context: './',
                                outputPath: '/css',
                            }
                        },
                        {
                            loader: 'extract-loader'
                        },
                        {
                            loader: 'css-loader',
                            options: {
                                sourceMap: argv.mode === 'development'
                            }
                        },
                        {
                            loader: 'sass-loader',
                            options: {
                                sourceMap: argv.mode === 'development'
                            }
                        }
                    ]
                },
                {
                    test: /\.svg$/,
                    use: 'file-loader?name=[name].[ext]&outputPath=images/&publicPath=/images/'
                }
            ]
        },
    }}