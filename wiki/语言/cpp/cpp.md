# c++

## std::vector<> find
<pre>
    if (std::find(toDestroy.begin(), toDestroy.end(), b)
        == toDestroy.end()) {
        toDestroy.push_back(b);
    }
</pre>

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

## 字符串erase

erase函数的原型如下：
（1）string& erase ( size_t pos = 0, size_t n = npos );
（2）iterator erase ( iterator position );
（3）iterator erase ( iterator first, iterator last );
也就是说有三种用法：
（1）erase(pos,n); 删除从pos开始的n个字符，比如erase(0,1)就是删除第一个字符
（2）erase(position);删除position处的一个字符(position是个string类型的迭代器)
（3）erase(first,last);删除从first到last之间的字符（first和last都是迭代器）

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