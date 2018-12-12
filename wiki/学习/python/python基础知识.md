# python基础知识

## 输出

```
print("hello world!")
```

## 数学运算

- 整除 //

```
10 // 3
```

- 求幂 **

```
2 ** 3
```

## 变量

- 使用变量前必须赋值，python变量没有默认值

```
x = 3
```

## 输入

```
x = input("plz enter:")
```

## 引用 导入模块

```
import math

math.floor(1.8)
maht.ceil(1.2)
```

- 在调用函数时不指定前缀 from module import function

```
from math import sqrt

sqrt(9)
```

- 使用变量名替代函数

```
foo = math.sqrt

foo(4)
```

## 回到未来 __future__

- 对于当前不支持，但未来将成为标准组成部分的功能，可以从这个模块进行导入

```
from __future__ import division
```