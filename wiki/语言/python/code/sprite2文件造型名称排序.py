#!/usr/bin/python
# -- coding:utf-8 --

import zipfile, json

name = raw_input("输入需要排序的sprite2文件地址:\n").strip()

with zipfile.ZipFile(name, 'a') as azip:

    jsonStr = azip.read('sprite.json')
    obj = json.loads(jsonStr, encoding='utf-8')
    costumes = obj['costumes']

    # 排序
    newCostumes = sorted(costumes,key=lambda item: item['costumeName'])

    # 更新
    obj['costumes'] = newCostumes

    # 序列化
    data = json.dumps(obj, ensure_ascii=False, indent=4)
    data = data.encode('utf-8')

    azip.writestr('sprite.json', data, compress_type=zipfile.ZIP_DEFLATED)

print("完成")