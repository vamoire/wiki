# Node.js

## 包管理
```
npm install（i） pagejson_name //下载的格式 本地模式  末尾加-g (--global) 就是全局模式

npm uninstall  pagejson_name //删除的格式  末尾加-g (--global) 就是全局模式

npx ......  //新版本命令

```

## 模块 Module

- 方式1
创建模块
```
//module.js
var name;
exports.setName = function(thyName){
    name = thyName;
}
exports.sayHello = function(){
    console.log('hello ' + name);
}
```
导入模块
```
//getmodule.js
var hello = require('./module');
hello.setName('123');
hello.sayHello();
```

- 方式2
创建模块
```
//module.js
function hello() {
    var name;
    this.setName = function(thyName) {
        name = thyName;
    }
    this.sayHello = function(){
        console.log('hello ' + name);
    }
}
module.exports = hello;
```
导入模块
```
//getmodule.js
var Hello = require('./module');
var hello = new Hello();
hello.setName('123');
hello.sayHello();
```

## Web Server

- 简单webserver
创建js文件
```
//hello.js
var http = require('http');
http.createServer(function(req,res){
    res.writeHead(200,{'Content-Type':'text/html'});
    res.write('<h1>Node.js</h1>');
    res.end('<p>Hello World</p>');
}).listen(8888);
```
命令行执行
```
node hello.js
```