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
```
{
    position: absolute;
    left: 0px;
    top: 0px;
}
```

- 居中
```
{
    margin: 0 auto;
}
```

## 选择器
- [选择器](http://www.w3school.com.cn/cssref/css_selectors.asp)