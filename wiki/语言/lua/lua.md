# Lua

## 移除table中的元素
```
 for i , v in ipairs(t) do
   if v.id%3 == 0 then
     table.remove(t ,i)
   end
 end
```