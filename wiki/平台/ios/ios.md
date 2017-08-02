# iOS

## XCode图标替换
[XCode图标替换](XCode图标替换.md)

## QQ URL Schemes 生成规则

<pre>echo 'ibase=10;obase=16;1105898902'|bc</pre>

url schema 格式如下：
"QQ" + 腾讯QQ互联应用appId 转换成十六进制 (不足8位前面补0)
41EAAD96


## 工程依赖

- [Xcode创建子工程以及工程依赖](http://www.jianshu.com/p/f2bc7d155a86)

## Framework创建

- [Xcode7创建.a静态库和framework](http://www.cnblogs.com/XYQ-208910/p/5157673.html)

- [创建你自己的Framework](http://www.cocoachina.com/ios/20150127/11022.html)

## 静态库合并
- 合并
<pre>
lipo -create xx.a yy.a -output zz.a
</pre>

- 信息
<pre>
lipo -info zz.a
</pre>
