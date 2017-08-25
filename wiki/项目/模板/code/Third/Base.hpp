//
//  Base.hpp
//  AFBChild
//
//  Created by apple on 2017/8/25.
//
//

#ifndef Base_hpp
#define Base_hpp

#include "cocos2d.h"

namespace mx {
    //通知KEY
#define Notification_Key_appEnterBackground "Notification_Key_applicationDidEnterBackground"
#define Notification_Key_appEnterForeground "Notification_Key_applicationWillEnterForeground"
    
    
    //获取缓存目录
    std::string getCacheRoot();
    
    //读取json字符串
    cocos2d::Value readJsonString(std::string jsonStr);
    
    //读取json文件
    cocos2d::Value readJsonFile(std::string file);
    
    //读取Value中的值
    //默认返回Value()
    cocos2d::Value valueGetValue(cocos2d::Value data, const char* key, ...);
    //默认返回""
    std::string valueGetString(cocos2d::Value data, const char* key, ...);
    //默认返回-1
    int valueGetInt(cocos2d::Value data, const char* key, ...);
    //默认返回false
    bool valueGetBool(cocos2d::Value data, const char* key, ...);
    
    //网络请求
    void* request(std::string url, std::function<void(std::string)>callback);
    
    //应用前后台切换发送通知
    void applicationDidEnterBackground();
    void applicationWillEnterForeground();
    
    //时间
    time_t getTimeNow();
    //时间字符串转换
    time_t string2time(std::string str, const char* format = "%Y-%m-%d %H:%M:%S");
    std::string time2string(time_t time, const char* format = "%Y-%m-%d %H:%M:%S");
}

#endif /* Base_hpp */
