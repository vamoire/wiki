# MAC

## 缩放和移动窗口
```
在 OS X 中缩放和拖动窗口几乎是我们每天都可能做的动作，除了将鼠标移动到窗口角落和边界上进行缩放以外，你还知道 OS X Lion 中的几个窗口缩放和移动的新特性吗？这里介绍三个非常有意思且更加有用的小技巧。

首先，打开一个窗口，例如 Finder 或 Safari，并把鼠标指针移动到窗口左侧的边缘。它会变成一个左右双向箭头。在 OS X Lion 中无论将指针移到窗口的上、下、左、右以及四个角落上时都会呈现出类似的双向箭头（有些不支持缩放的窗口例外），也就进入了窗口缩放的状态。

现在，按住 Option 键，并左右拖拽进行缩放。你会看到和普通的窗口缩放略有不同：当你拖拽时，窗口的左右两侧会同时收拢和扩大。也就是说，当按住 Option 键在左侧进行缩放时，右侧也会随之进行相同幅度的缩放。同样的道理，上、下和四个角落里按住 Option 进行缩放都会是这种联动的方式。

接下来，试试 Shift 键。按住它，并拖拽窗口的左侧边界，你会看到窗口的四个边都在动作，这和在 Photoshop 里处理图片时有点类似，当前的窗口会保持纵横比例地进行缩放。

如果这不足以让你感到意外，请继续尝试：按住 Command 键，并缩放被遮挡住的窗口，也就是不在最前面的窗口。真的可以缩放后台的窗口吗？不信就试试看吧。

第四条技巧更有意思。当你把指针移到窗口的上、下、左、右边界上时，它会呈现双向箭头的样式。假设现在把指针放在窗口的左边界上，现在直接上下拖动一下，你就可以移动窗口了，而不再是缩放。同样，当指针停在窗口的上、下边界时，横向拖动一下也能够移动窗口。这样即使不用标题栏你也可以轻松移动窗口。
```

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