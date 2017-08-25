//
//  Base.cpp
//  AFBChild
//
//  Created by apple on 2017/8/25.
//
//

#include "Base.hpp"

#include "json/rapidjson.h"
#include "json/document.h"
#include "network/HttpClient.h"

USING_NS_CC;
using namespace cocos2d::network;

namespace mx {
    
    //获取缓存目录
    std::string getCacheRoot() {
        std::string cachePath = cocos2d::FileUtils::getInstance()->getWritablePath();
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
        cachePath = cachePath.erase(cachePath.size() - 10) + "Library/Caches/";
#elif (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
        cachePath = cachePath.erase(cachePath.size() - 6) + "Library/Caches/";
#endif
        return cachePath;
    }
    
    //json::value转为cocos2d::value
    cocos2d::Value valueFromJsonValue(rapidjson::Value &json) {
        Value ret;
        do {
            if (json.IsNull()) {
                ret = Value();
                break;
            }
            if (json.IsObject()) {
                ValueMap vm;
                for (auto i = json.MemberBegin(); i != json.MemberEnd(); ++i) {
                    auto key = i->name.GetString();
                    vm[key] = valueFromJsonValue(i->value);
                }
                ret = Value(vm);
            }
            else if (json.IsArray()) {
                ValueVector vv;
                for (auto i = 0; i < json.Capacity(); ++i) {
                    vv.push_back(valueFromJsonValue(json[i]));
                }
                ret = Value(vv);
            }
            else if (json.IsString()) {
                ret = Value(json.GetString());
            }
            else if (json.IsInt()) {
                ret = Value(json.GetInt());
            }
            else if (json.IsBool()) {
                ret = Value(json.GetBool());
            }
            else if (json.IsDouble()) {
                ret = Value(json.GetDouble());
            }
            else {
                ret = Value();
            }
        } while (0);
        return ret;
    }
    
    //读取json字符串
    cocos2d::Value readJsonString(std::string jsonStr){
        Value ret = Value();
        rapidjson::Document doc;
        do {
            doc.Parse<0>(jsonStr.c_str());
            if (doc.HasParseError()) {
                log("string 转 json 失败");
                break;
            }
            ret = valueFromJsonValue(doc);
        } while (0);
        return ret;
    }
    
    //读取json文件
    cocos2d::Value readJsonFile(std::string file){
        std::string str = FileUtils::getInstance()->getStringFromFile(file);
        return readJsonString(str);
    }
    
    
    
    //读取多层Value中的值
    cocos2d::Value valueGetArgs(cocos2d::Value data, const char* key, va_list args) {
        cocos2d::Value d = Value(data);
        if (d.getType() == Value::Type::MAP) {
            ValueMap vm = d.asValueMap();
            d = vm[key];
        }
        else {
            return Value();
        }
        
        do {
            if (d.getType() == Value::Type::MAP) {
                ValueMap vm = d.asValueMap();
                auto k = va_arg(args, const char*);
                if (k) {
                    d = vm[k];
                }
                else {
                    break;
                }
            }
            else if (d.getType() == Value::Type::VECTOR) {
                ValueVector vv = d.asValueVector();
                auto k = va_arg(args, int);
                if (k >= 0 && k < (int)vv.size()) {
                    d = vv.at(k);
                }
                else {
                    break;
                }
            }
            else {
                break;
            }
        } while (true);
        return d;
    }
    
    cocos2d::Value valueGetValue(cocos2d::Value data, const char* key, ...){
        //参数列表变量
        va_list args;
        //初始化
        va_start(args, key);
        //获取
        cocos2d::Value d = valueGetArgs(data, key, args);
        //结束
        va_end(args);
        return d;
    }
    std::string valueGetString(cocos2d::Value data, const char* key, ...){
        //参数列表变量
        va_list args;
        va_start(args, key);
        cocos2d::Value d = valueGetArgs(data, key, args);
        va_end(args);
        //结果
        std::string ret = "";
        if (d.getType() == Value::Type::STRING) {
            ret = d.asString();
        }
        return ret;
    }
    
    int valueGetInt(cocos2d::Value data, const char* key, ...){
        va_list args;
        va_start(args, key);
        cocos2d::Value d = valueGetArgs(data, key, args);
        va_end(args);
        //结果
        int ret = -1;
        if (d.getType() == Value::Type::INTEGER) {
            ret = d.asInt();
        }
        return ret;
    }
    bool valueGetBool(cocos2d::Value data, const char* key, ...){
        va_list args;
        va_start(args, key);
        cocos2d::Value d = valueGetArgs(data, key, args);
        va_end(args);
        //结果
        bool ret = false;
        if (d.getType() == Value::Type::BOOLEAN) {
            ret = d.asBool();
        }
        return ret;
    }
    
    //网络请求
    void* request(std::string url, std::function<void(std::string)>callback){
        HttpRequest* request = new (std::nothrow) HttpRequest();
        request->setUrl(url);
        request->setRequestType(HttpRequest::Type::GET);
        request->setResponseCallback([=](HttpClient *sender, HttpResponse *response){
            std::string ret = "";
            do{
                if (!response) {
                    log("response = null");
                    break;
                }
                if (strcmp(response->getErrorBuffer(), "") != 0) {
                    log("%s", response->getErrorBuffer());
                }
                if (response->getResponseCode() != 200) {
                    log("code = %ld", response->getResponseCode());
                    break;
                }
                
                //data
                auto dv = response->getResponseData();
                
                if (dv->size() == 0) {
                    log("data size = %lu", dv->size());
                    break;
                }
                
                for (int i = 0; i < dv->size(); ++i) {
                    ret += (*dv)[i];
                }
            }
            while (false);
            callback(ret);
        });
        
//        if (type == HttpRequest::Type::POST) {
//            std::string postData = "";
//            for (auto i = data.begin(); i != data.end(); ++i) {
//                auto key = i->first;
//                auto value = i->second;
//                postData = postData + "&" + key + "=" + post_encode(value);
//            }
//            if (postData.length() > 0) {
//                postData.erase(postData.begin());
//            }
//            request->setRequestData(postData.c_str(), strlen(postData.c_str()));
//        }

//        HttpClient::getInstance()->sendImmediate(request);
        HttpClient::getInstance()->send(request);
        request->release();
        return request;
    }
    
    //应用前后台切换发送通知
    void applicationDidEnterBackground(){
        Director::getInstance()->getEventDispatcher()->dispatchCustomEvent(Notification_Key_appEnterBackground);
    }
    
    void applicationWillEnterForeground(){
        Director::getInstance()->getEventDispatcher()->dispatchCustomEvent(Notification_Key_appEnterForeground);
    }
    
    //时间
    time_t getTimeNow(){
        //当前时间
        time_t     now;
        // Get current time
        std::time(&now);
        return now;
    }
    
    //时间字符串转换
    time_t string2time(std::string str, const char* format){
        time_t     time;
        struct tm  ts;
        strptime(str.c_str(), format, &ts);
        time = mktime(&ts);
        return time;
    }
    std::string time2string(time_t time, const char* format){
        struct tm  ts;
        char       buf[80];
        ts = *localtime(&time);
        strftime(buf, sizeof(buf), format, &ts);
        return std::string(buf);
    }
}
