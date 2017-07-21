# c++

## 类型判断
<pre>
if (dynamic_cast< LessonLayer* >(this) == nullptr) {
    //this 不是 LessonLayer类及其子类
}
</pre>

## 字符串截取
<pre>
std::string str = "abcd";
std::string str2 = str.substr(1, 2);    //取str第1个字符开始的2个字符， str2==“bc”
</pre>

## 赋值定义
<pre>
Rect& operator= (const Rect& other);
Size& Size::operator= (const Size& other)
{
    setSize(other.width, other.height);
    return *this;
}
</pre>