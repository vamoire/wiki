# XCode

## 搜索代码中的中文字符串
1. 打开”Find Navigator”
2. 切换搜索模式到 “Find > Regular Expression”
3. 输入@"[^"]*[\u4E00-\u9FA5]+[^"\n]*?" (swift请去掉”@” 输入@"[^"]*[\u4E00-\u9FA5]+[^"\n]*?" 就好了)