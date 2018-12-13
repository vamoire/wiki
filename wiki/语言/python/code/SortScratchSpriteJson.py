#!/usr/bin/python
# -- coding:utf-8 --

# Scratch2.0离线编辑器保存的.sprite文件进行文件名排序
import json

# 读文件
with open("sprite.json","r") as f:
    # 从文件中读取Json对象
    obj = json.load(f)
    costumes = obj['costumes']

# 排序
newCostumes = sorted(costumes,key=lambda item: item['costumeName'])

# 更新
obj['costumes'] = newCostumes

# 写文件
with open("sprite.json","w") as f2:
    # 序列化写入文件
    json.dump(obj, f2, indent=4)