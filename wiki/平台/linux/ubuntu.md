# Ubuntu

## git常用命令
[git常用命令](https://blog.csdn.net/tomatozaitian/article/details/73515849)

## libpcre2-8-0
[libpcre2-8-0](https://packages.ubuntu.com/xenial/libpcre2-8-0)

## 改源
[ubuntu 把软件源修改为国内源和更新](https://www.cnblogs.com/flyinggod/p/7979108.html)


## VMware安装虚拟机Ubuntu提示piix4_smbus:Host SMBus错误解决办法

错误： ubuntu开机出现错误提示：piix4_smbus 0000:00:007.3: Host SMBus controller not enabled

原因： ubuntu装入i2c_piix4模块所致，因为系统找不到这个模块，所以报错

处理方法：

1、查明装入模块的确切名字，显示输出的结果是模块的确切名字：i2c_piix4

2、将该模块列入不装入名单。编辑文件sudo vim /etc/modprobe.d/blacklist.conf，在末尾加入blacklist i2c-piix4

3、重启reboot

## 搭建git服务器

[搭建git服务器](http://www.cnblogs.com/dee0912/p/5815267.html)