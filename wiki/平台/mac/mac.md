# MAC

## SVN服务器搭建
[SVN服务器搭建](https://www.cnblogs.com/czq1989/p/4913692.html)

## 安装bundle文件
需要安装VMware Workstation for Linux on Ubuntu，文件是Bundle格式，由于我是新手不知怎样安装，在网上找了不少资料，竟然不能执行。
　　解决：
　　1, sudo su
　　要先取得root权限，网上的资料都没说这个，结果就是因为这个弄了好久都不成功。
　　2, sudo chmod +x VMware-Workstation-Full-7.1.3-324285.x86_64.bundle
　　授予运行权限
　　3，sudo ./VMware-Workstation-Full-7.1.3-324285.x86_64.bundle
　　这样就可以有个图形界面安装了。

## 图片剪裁
打开图片选择区域Command＋K

## 目录权限
<pre>
sudo chown -R user:/Library/WebServer/Documents/
</pre>

## 屏幕录制
mac自带quicktimeplayer录制屏幕
电脑声音录制 
1. 安装soundflower 
2. launchpad -> audio midi setup
3. 创建混合音频输出create multi-output device (build-in output + soundflower 2ch)
4. 系统sound 输出设置为混合音频输出 输入设置为soundflower 2ch
5. [链接][1]

[1]:http://bbs.feng.com/forum.php?mod=viewthread&tid=9950118&archiveid=1