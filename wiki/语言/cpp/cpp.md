# c++


## 泛型
<pre>
    //模板声明
    template <typename T>
    
    //函数模板
    T add(T a, int b) {
        return a + b;
    }

    //根据传入的参数类型返回对应的结果
    auto x = add(1, 2);
    //x = int
    auto y = add(1.0, 1.0);
    //y = double
</pre>



## 参数 ...

<pre>
Menu * Menu::create(MenuItem* item, ...)
{
    va_list args;
    va_start(args,item);
    
    Vector<MenuItem*> items;
    if( item )
    {
        //取第一个参数
        items.pushBack(item);
        //取第二个参数
        MenuItem *i = va_arg(args, MenuItem*);
        while(i)
        {
            items.pushBack(i);
            //取后续的参数
            i = va_arg(args, MenuItem*);
        }
    }

    Menu *ret = Menu::createWithArray(items);
    
    va_end(args);
    
    return ret;
}
</pre>


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