# Web开发

## JS

## 显示隐藏元素
- 隐藏后保留占用的空间
```
//隐藏
document.getElementById('jionUs').style.visibility = 'hidden';
//显示
document.getElementById('jionUs').style.visibility = 'visible';
```

- 隐藏后释放占用的空间
```
// 显示
document.getElementById('jionUs').style.display = '';
// 隐藏
document.getElementById('jionUs').style.display = 'none';
```

## 重定向
- 
```
window.location = "http://www.baidu.com";
```

- 
```
document.location = "http://www.baidu.com";
```

## 延迟执行
```
setTimeout("autoFillIFrameHeight()", 310)
```

## bootstrap

## 下拉菜单
```
<span class="dropdown">
    <button style="width: 160px" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        面议
        <span class="caret" style="margin-left: 5px"></span>
    </button>
    <ul class="dropdown-menu dropdown-menu-left">
        <li>
            <a href="#">面议</a>
        </li>
    </ul>
</span>
```