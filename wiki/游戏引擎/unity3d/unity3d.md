# unity3d

## 目录权限
iOS:
Application.dataPath :                    Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data
Application.streamingAssetsPath : Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw
Application.persistentDataPath :    Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Documents
Application.temporaryCachePath : Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches

Android:
Application.dataPath :  /data/app/xxx.xxx.xxx.apk
Application.streamingAssetsPath :  jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
Application.persistentDataPath :  /data/data/xxx.xxx.xxx/files
Application.temporaryCachePath :  /data/data/xxx.xxx.xxx/cache

Windows Web Player:
Application.dataPath :  file:///D:/MyGame/WebPlayer (即导包后保存的文件夹，html文件所在文件夹)
Application.streamingAssetsPath : 
Application.persistentDataPath : 
Application.temporaryCachePath : 

---------------------------------------------------------------------------------------------------
各目录权限：

根目录：StreamingAssets文件夹
<pre>
#if UNITY_EDITOR
string filepath = Application.dataPath +"/StreamingAssets"+"/my.xml";
#elif UNITY_IPHONE
 string filepath = Application.dataPath +"/Raw"+"/my.xml";
#elif UNITY_ANDROID
 string filepath = "jar:file://" + Application.dataPath + "!/assets/"+"/my.xml;
#endif
</pre>
根目录：Resources 文件夹
可以使用Resources.Load("名字"); 把文件夹中的对象加载出来

根目录：StreamingAssets 文件夹
可以使用Application.dataPath进行读操作
Application.dataPath： 只可读不可写，放置一些资源数据

Application.persistentDataPath
iOS与Android平台都可以使用这个目录下进行读写操作，可以存放各种配置文件进行修改之类的。
在PC上的地址是：C:\Users\用户名 \AppData\LocalLow\DefaultCompany\test