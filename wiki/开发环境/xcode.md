# XCode

## 代码行数统计

```
find . -name "*.m" -or -name "*.h" -or -name "*.xib" -or -name "*.c" |xargs grep -v "^$"|wc -l 
```

```
find . -name "*.cpp" -or -name "*.h" -or -name "*.hpp" -or -name "*.mm" |xargs grep -v "^$"|wc -l 
```

[代码行数统计](http://blog.csdn.net/nathan1987_/article/details/49757769)

## 搜索代码中的注释 正则
1. 打开”Find Navigator”
2. 切换搜索模式到 “Find > Regular Expression”
3. 输入\/\/[^\n]*

## 搜索代码中的中文字符串 正则
1. 打开”Find Navigator”
2. 切换搜索模式到 “Find > Regular Expression”
3. 输入@"[^"]*[\u4E00-\u9FA5]+[^"\n]*?" (swift请去掉”@” 输入@"[^"]*[\u4E00-\u9FA5]+[^"\n]*?" 就好了)