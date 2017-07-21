# PHP

## 测试
[hello.php](code/hello.php)

## MAC开启PHP
1. 开启apache服务
2. 修改/etc/apache2/httpd.conf，取消下面代码的注释
<pre>
LoadModule php5_module libexec/apache2/libphp5.so
</pre>
3. 重启apache服务
<pre>
sudo apachectl restart
</pre>
4. 相关链接
[PHP教程][1]
[MAC下的PHP环境搭建][2]

[1]:http://www.w3school.com.cn/php/php_install.asp
[2]:http://blog.csdn.net/wj_november/article/details/51417491