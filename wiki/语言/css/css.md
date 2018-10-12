# css样式

## 背景图片
### 背景图片自适应
- 保持宽高比 完全填充
```
{
    background-image: url('image.jpg');
    background-size: cover;
}
```

- 宽高拉伸
```
{
    background-image: url('image.jpg');
    background-size: 100% 100%;
    -moz-background-size: 100% 100%;
    background-repeat: no-repeat";
}
```

- 保持图片宽高比 图片完全展示
```
{
    background-image: url('image.jpg');
    background-size: contain;
}
```

- [更多](https://www.cnblogs.com/intelwisd/p/7852118.html)

## 定位
- 绝对位置
position:absolute；position:relative绝对定位使用通常是父级定义position:relative定位，子级定义position:absolute绝对定位属性，并且子级使用left或right和top或bottom进行绝对定位。
```
<div style="position: relative">
    <label style="position: absolute;left: 0px; top: 0px">text</label>
</div>
```

- 居中
```
{
    margin: 0 auto;
}
```

## input-placeholder
```
<style type="text/css">
    input::-webkit-input-placeholder{
        color: red;
    }
    input::-moz-placeholder{
        color: red;
    }
    input:-moz-placeholder{
        color: red;
    }
    input:-ms-input-placeholder{
        color: red;
    }
</style>
```

## textarea
- 彻底禁用拖动
```
<textarea id='memo' style="height: 50px;width: 70px;resize: none;"></textarea>
```

- 组合使用css中的min-height min-width max-height max-width
```
<textarea id='memo' style="min-height: 50px;min-width: 70px;max-height: 50px; max-width: 70px;" ></textarea>
```

## 样式优先级
- 样式后加!important
```
vertical-align: middle!important;
```


## 选择器
- [选择器](http://www.w3school.com.cn/cssref/css_selectors.asp)