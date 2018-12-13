#!/usr/bin/python
# -- coding:utf-8 --

import zipfile, json, os, sys

#输入目录
rootdir = raw_input("输入需要排序的sprite2文件所在目录:\n").strip()

#列出文件夹下所有的目录与文件
list = os.listdir(rootdir)

count = 0

for i in range(0,len(list)):

    path = os.path.join(rootdir,list[i])

    if os.path.isfile(path) and path.endswith('.sprite2'):

        with zipfile.ZipFile(path, 'a') as azip:

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

            print("已修改:" + path)
            count = count + 1

print("已修改" + str(count) + "个文件")
