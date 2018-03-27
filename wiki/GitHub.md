# Github

## 忽略文件/目录
```
我希望不加入git管理。
在代码目录下建立.gitignore文件：vim .gitignore ,内容如下：
[plain] view plaincopy
#过滤数据库文件、sln解决方案文件、配置文件  
*.mdb  
*.ldb  
*.sln  
*.config  

#过滤文件夹Debug,Release,obj  
Debug/  
Release/  
obj/  
然后调用git add. ，执行 git commit即可。
```
```
Github客户端->Repository->Repository Setting->Ignored files
```