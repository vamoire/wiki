# Python

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