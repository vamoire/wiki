# cocos2d

## Android返回按钮监听
[Cocos2d-x android使用onKeyDown监听返回键实现二次返回退出](https://blog.csdn.net/canglang_123/article/details/43971023)
```
//1.修改源码framework/scocos2d-x/cocos/2d/platform/android/java/src/org/cocos2dx/lib/Cocos2dx/GLSurfaceView.java

@Override  
public boolean onKeyDown(final int pKeyCode, final KeyEvent pKeyEvent) {  
    switch (pKeyCode) {  
        case KeyEvent.KEYCODE_BACK:  
            //cocos不接收返回按键时间
               return false;
        case KeyEvent.KEYCODE_MENU:  
            this.queueEvent(new Runnable() {  
             @Override  
            public void run() {  
                Cocos2dxGLSurfaceView.this.mCocos2dxRenderer.handleKeyDown(pKeyCode);  
            }  
        });  
        return true;  
        default:  
            return super.onKeyDown(pKeyCode, pKeyEvent);  
    }
} 


//2.在android的AppActivity中重写onKeyDown方法
    private long mkeyTime = 0;  
    public boolean onKeyDown(int keyCode, KeyEvent event) {  
        //二次返回退出  
        if (keyCode == KeyEvent.KEYCODE_BACK) {  
            if ((System.currentTimeMillis() - mkeyTime) > 2000) {  
                mkeyTime = System.currentTimeMillis();  
                Toast.makeText(this, "再按一次退出游戏", Toast.LENGTH_LONG).show();  
            } else {  
                finish();  
                System.exit(0);  
            }  
            return false;  
        }  
        return super.onKeyDown(keyCode, event);  
    }  
```

## 修改背景颜色
* 搜索glClearColor
* cocos2d_libs/renderer/CCRenderer.cpp
* // default clear color
* _clearColor = Color4F::BLACK;


## 修改应用包名
```
修改proj.android\AndroidManifest.xml中的包名
修改AppActivity.java等java文件中的引用包名，例如
import com.test.test.R;
```

## 修改安装包名字
```
项目路径\proj.android\build.xml

<project name="安装包名"default="help">
```
 
## 修改游戏名/程序名
```
项目路径\proj.android\res\value\strings.xml

<string name="app_name">程序名</string>
```

## 修改程序图标
```
项目路径\proj.android\res目录下，3个文件夹对应3个分辨率。执行打包之后，项目路径\proj.android\bin\res下的3个文件夹会自动地更新。
```

## 命令行打包
```
cd /Users/apple/Downloads/cocos2d-x-3.15.1/tools/cocos2d-console/bin 
python cocos.py compile -s /Users/apple/Desktop/AFBChildEn  -p android --ap android-19
```

## 二维码生成

[QRCode](http://code.ciaoca.com/javascript/qrcode/)
[cocos2d-x绘制二维码(2.x)](https://github.com/neoliang/cocos2d-qrsprite)
[页面中加载webview方式集成二维码]()

## Mac修改cocos2d环境变量
<pre>
pico .bash_profile
</pre>

## cocos creator 开发文档

- [Cocos Creator 开发文档](http://www.cocos.com/docs/creator/getting-started/index.html)

## cocos2dxlua3.15iOS模拟器崩溃
- [cocos2dxlua3.15iOS模拟器崩溃](http://www.cnblogs.com/skar/p/7066254.html)

- [CocosCreator手记03——配置VSCode的TypeScript环境](http://www.tuicool.com/articles/U3mU3my)


## 常见错误
- [常见错误](常见错误.md)

## vungle广告集成

- [Vungle CN](https://support.vungle.com/hc/zh-cn)

- [Vungle SDK](https://dashboard.vungle.com/sdk)

- [Vungle集成文档](https://support.vungle.com/hc/en-us/articles/115000477452)

- [SDKBOX集成Vungle](http://docs.sdkbox.com/zh/plugins/vungle/v3-cpp/)
已测试sdkbox list中已经没有vungle选项

## SDKBOX集成
<pre>
sdkbox import vungle --server china
</pre>

[SDKBOX安装](http://docs.sdkbox.com/en/installer/)

[SDKBOX GITHUB](https://github.com/sdkbox/sdkbox-sample-vungle)


## cocos2d framework

- 添加cocos2dlib工程到framework中
- 添加search等cocos2d的路径搜索
- 添加cocos2dios.a等相关库
- 编译生成framework
>1. Edit Scheme，Build Configuration下选择Release 
>2. 真机 编译生成Release版的framework
>3. Edit Scheme，Build Configuration下选择Debug
>4. 模拟器 编译生成Debug版的framework
>5. 使用lipo命令合并生成真机模拟器通用版本

---
![image](res/添加cocos2dlib工程到framework中)
![image](res/添加search等cocos2d的路径搜索1)
![image](res/添加search等cocos2d的路径搜索2)
![image](res/添加search等cocos2d的路径搜索3)
![image](res/添加search等cocos2d的路径搜索4)
![image](res/添加search等cocos2d的路径搜索5)
![image](res/添加cocos2dios.a等相关库)


## 加速编译

- [cocos2d-x 减少编译时间/免除重复编译](http://blog.csdn.net/u014335219/article/details/50492088)

- [Cocos2dx 3.x 新建项目编译很慢的解决方案windows平台](http://blog.csdn.net/crocodile__/article/details/51133835)


## 着色器shader
[着色器shader](着色器shader.md)

## CSLoader使用
[CSLoader使用](CSLoader使用.md)

## JS到java和oc的反射
[JS到java和oc的反射1][1]
[JS到java和oc的反射2][2]

## Cocos2d-x在xcode下开发生成静态库添加到项目 ##
[Cocos2d-x在xcode下开发生成静态库添加到项目][3]

![image](res/静态库.png)

[1]:http://www.cocos.com/docs//doc/article/index?type=cocos2d-x&url=/doc/cocos-docs-master/manual/framework/html5/v3/reflection/zh.md
[2]:http://www.cocos.com/docs/article/index?type=cocos2d-x&url=/doc/cocos-docs-master/manual/framework/html5/v3/reflection-oc/zh.md 
[3]:http://blog.csdn.net/vivi_12/article/details/54668714