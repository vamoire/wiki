#!/usr/bin/env python
# -*- coding: utf-8 -*-
import sys,os,subprocess,shutil

#获取脚本文件的当前路径
def cur_file_dir():
     #获取脚本路径
     path = sys.path[0]
     #判断为脚本文件还是py2exe编译后的文件，如果是脚本文件，则返回的是脚本的目录，如果是py2exe编译后的文件，则返回的是编译后的文件路径
     if os.path.isdir(path):
         return path
     elif os.path.isfile(path):
         return os.path.dirname(path)

#文件搜索
def searchFile(path, word):
    ret = ""
    for filename in os.listdir(path):
        fp = os.path.join(path, filename)
        if os.path.isfile(fp) and word in filename:
            ret = fp
            break
        elif os.path.isdir(fp):
            ret = search(fp, word)
            if ret != "" :
                break
    return ret

#目录搜索
def searchPath(path, word):
    ret = ''
    for filename in os.listdir(path):
        fp = os.path.join(path, filename)
        if os.path.isdir(fp) and word in filename:
            ret = fp
            break
        elif os.path.isdir(fp):
            ret = search(fp, word)
            if ret != '' :
                break
    return ret

#字符串lastIndex
def string_last_index(text, word):
    begin = 0
    npos = -1
    while True:
        temp = text.find(word,begin)
        if temp < 0 :
            #查找结束
            break
        else:
            npos = temp
            begin = npos + len(word)
    return npos

#删除目录
def delete_dir_files(top):
    for root, dirs, files in os.walk(top, topdown=False):
        for name in files:
            os.remove(os.path.join(root, name))
        for name in dirs:
            os.rmdir(os.path.join(root, name))
    os.rmdir(top)
#获取framework文件的短名称
def get_framework_short_name(path):
    begin = string_last_index(path,'/')
    end = path.index('.framework')
    return path[begin + 1 : end]

#脚本所在目录
root = cur_file_dir()
#默认导出根目录
out = "/Users/apple/Desktop"
#获取脚本指定的参数
if len(sys.argv) == 3 :
    root = sys.argv[1]
    out = sys.argv[2]
elif len(sys.argv) == 2:
    out = sys.argv[1]
#debug framework 地址
debug = root + '/Debug-iphonesimulator/'
debugFramework = searchPath(debug,'.framework')
#release framework 地址
release = root + "/Release-iphoneos/"
releaseFramework = searchPath(release,".framework")
#framework名称
frameworkName = get_framework_short_name(debugFramework)
#导出目录
outRoot = out + "/"
#导出的framework
outFramework = outRoot + frameworkName + ".framework"
#创建目录
if os.path.exists(outRoot) == False:
    os.makedirs(outRoot)
#删除旧文件
if (os.path.exists(outFramework)) :
    delete_dir_files(outFramework)
#复制debug framework到out目录
cmd = "cp -R " + debugFramework + " " + outFramework
# print cmd
subprocess.call(cmd,shell=True)
#合并deubg和release到out目录
cmd = "lipo -create " + debugFramework + "/" + frameworkName + " " + releaseFramework + "/" + frameworkName + " -output " + outRoot + frameworkName + ".framework/" + frameworkName
# print cmd
subprocess.call(cmd,shell=True)