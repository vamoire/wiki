# iOS

## DEBUG模式判断
<pre>
#ifdef DEBUG
</pre>

## 谷歌广告集成
依赖库:
GLKit 
AdSupport 
CoreVideo 
MobileCoreServices

## vungle广告集成
依赖库:
AdSupport.framework
AudioToolbox.framework
AVFoundation.framework
CFNetwork.framework
CoreGraphics.framework
CoreMedia.framework
Foundation.framework
libz.dylib or libz.tbd
libsqlite3.dylib or libsqlite3.tbd
MediaPlayer.framework
QuartzCore.framework
StoreKit.framework
SystemConfiguration.framework
UIKit.framework
WebKit.framework

[Vungle SDK的崩溃问题](http://www.jianshu.com/p/b824ed3e8ef5)
Other Link: -ObjC
GameController.framework
libc++.tbd







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
