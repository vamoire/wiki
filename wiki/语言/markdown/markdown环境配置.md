# Markdown 环境配置 #

## 1. 安装 ##
- 创建目录
- 下载[MDWiki][4]
- 解压MDWiki到目录
- 将mdwiki.html设置为web server默认页
- 浏览器打开

## 2. 导航配置 ##
- 在目录创建navigation.md文件
- 导航格式如下
<pre>
># Brand name
>
>[Menu Item 1]()
>
>  * # SubMenu Heading 1
>  * [SubMenu Item 1](subitem1.md)
>  * [SubMenu Item 2](subitem2.md)
>  - - - -
>  * # SubMenu Heading 2
>  * [SubMenu Item 3](subitem3.md)
>  - - - -
>  * # SubMenu Heading 3
>  * [SubMenu Item 3](subitem3.md)
>
>[Menu Item 2](item2.md)
>- - - -
>[Menu Item 3](item3.md)
>
>[gimmick:themechooser](主题)
</pre>

## 3. 项目配置 ##
- 在目录中添加config.json
<pre>
{
    "title": "title",
    "useSideMenu": true,
    "lineBreaks": "gfm",
    "additionalFooterText": "All content and images &copy; by John Doe",
    "anchorCharacter": "#"
}
</pre>

## 4.侧边栏 ##
- [侧边栏目录][5]
- [侧边栏目录][6]

## 5. Window Web Server
- [配置IIS][3]

## 6. Mac Web Server
- 启动
<pre>
sudo apachectl start
</pre>

- 根目录
/Library/WebServer/Documents/

- 目录权限
<pre>
sudo chown -R user:/Library/WebServer/Documents/
</pre>

- 链接
[Mac启动Webserver服务][1]
[Mac修改权限][2]

[1]:http://www.360doc.com/content/15/0712/23/12146850_484530531.shtml
[2]:https://stackoverflow.com/questions/15980675/chown-illegal-group-name-mac-os-x
[3]:http://jingyan.baidu.com/article/ed2a5d1f128ff609f6be17fa.html
[4]:https://github.com/Dynalon/mdwiki/releases
[5]:http://www.mamicode.com/info-detail-1744454.html
[6]:(https://zohead.com/archives/wikitten-mdwiki/