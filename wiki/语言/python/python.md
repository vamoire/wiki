# Python


## Json操作

一. dumps 和 dump

    dumps和dump   序列化方法
    dumps只完成了序列化为str，
    dump必须传文件描述符，将序列化的str保存到文件中

```
>>> import json
>>> json.dumps([])    # dumps可以格式化所有的基本数据类型为字符串
'[]'
>>> json.dumps(1)    # 数字
'1'
>>> json.dumps('1')   # 字符串
'"1"'
>>> dict = {"name":"Tom", "age":23}  
>>> json.dumps(dict)     # 字典
'{"name": "Tom", "age": 23}'
```

```
a = {"name":"Tom", "age":23}
with open("test.json", "w", encoding='utf-8') as f:
    # indent 超级好用，格式化保存字典，默认为None，小于0为零个空格
    f.write(json.dumps(a, indent=4))
    # json.dump(a,f,indent=4)   # 和上面的效果一样
```

二. loads 和 load 

    loads和load  反序列化方法
    loads 只完成了反序列化，
    load 只接收文件描述符，完成了读取文件和反序列化

```
>>> json.loads('{"name":"Tom", "age":23}')
{'age': 23, 'name': 'Tom'}
```

```
import json
with open("test.json", "r", encoding='utf-8') as f:
    aa = json.loads(f.read())
    f.seek(0)
    bb = json.load(f)    # 与 json.loads(f.read())
print(aa)
print(bb)

# 输出：
{'name': 'Tom', 'age': 23}
{'name': 'Tom', 'age': 23}
```

[对Json文件排序](code/SortScratchSpriteJson.py)

[python json模块 超级详解](https://www.cnblogs.com/tjuyuan/p/6795860.html)

[Python3 JSON 数据解析](http://www.runoob.com/python3/python3-json.html)

## Sort排序

指定比较对象进行排序
```
>>> student_tuples = [
        ('john', 'A', 15),
        ('jane', 'B', 12),
        ('dave', 'B', 10),
]
>>> sorted(student_tuples, key=lambda student: student[2])   # sort by age
[('dave', 'B', 10), ('jane', 'B', 12), ('john', 'A', 15)]
```

[对Json文件排序](code/SortScratchSpriteJson.py)

[python 列表排序方法sort、sorted技巧篇](http://www.cnblogs.com/whaben/p/6495702.html)

## 合并framework

[合并framework](code/call.py)

使用方法:将call.py复制到Products目录下执行


## 文件搜索
<pre>
import os
import sys

def search(path, word):
    for filename in os.listdir(path):
        fp = os.path.join(path, filename)
        if os.path.isfile(fp) and word in filename:
            print fp
        elif os.path.isdir(fp):
            search(fp, word)

search(sys.argv[1], sys.argv[2])

使用

python search.py directory_path keyword

2. 在制定目录及其子目录中查找文件内容含有关键字的文件

源码

#search.py
import os
import sys

def search(path, word):
    for filename in os.listdir(path):
        fp = os.path.join(path, filename)
        if os.path.isfile(fp):
            with open(fp) as f:
                for line in f:
                    if word in line:
                        print fp
                        break
        elif os.path.isdir(fp):
            search(fp, word)

search(sys.argv[1], sys.argv[2])

使用

python search.py directory_path keyword
</pre>