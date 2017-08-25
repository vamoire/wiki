//
//  TheConfig.cpp
//  AFBChild
//
//  Created by apple on 2017/8/25.
//
//

#include "TheConfig.hpp"
#include "cocos2d.h"
#include "Base.hpp"

USING_NS_CC;
using namespace mx;

static TheConfig* __sTheConfig = nullptr;
TheConfig* TheConfig::getInstance(){
    if (__sTheConfig == nullptr) {
        __sTheConfig = new TheConfig();
    }
    return __sTheConfig;
}

TheConfig::TheConfig()
:_configFile("")
,_defaultConfigFile("")
,_configUpdateUrl("")
,_updateOnAppEnterForeground(false)
//review
,_reviewEnable(false)
,_reivewiOSUrl("")
,_reivewAndroidUrl("")
,_reviewSucessTime(0)
,_reviewNeverShowIfSuccess(false)
//ad
,_adConfigUpdateUrl("")
{
    //配置文件目录
    std::string configRoot = getCacheRoot() + "config/";
    auto fu = FileUtils::getInstance();
    if (fu->isDirectoryExist(configRoot) == false) {
        fu->createDirectory(configRoot);
    }
    //配置文件
    _configFile = configRoot + "TheConfig.json";
    //默认配置文件
    _defaultConfigFile = fu->fullPathForFilename("TheConfig.json");
    //加载配置文件
    loadConfig();
    //后台切回前台更新配置文件
    Director::getInstance()->getEventDispatcher()->addCustomEventListener(Notification_Key_appEnterForeground, [&](EventCustom*){
        if (this->_updateOnAppEnterForeground) {
            this->updateConfig();
        }
    });
}
TheConfig::~TheConfig(){
    
}

//加载配置文件
void TheConfig::loadConfig(){
    auto fu = FileUtils::getInstance();
    std::string file = _defaultConfigFile;
    if (fu->isFileExist(_configFile)) {
        file = _configFile;
    }
    auto data = readJsonFile(file);
    //更新配置文件地址
    _configUpdateUrl = valueGetString(data, "UpdateUrl");
    _updateOnAppEnterForeground = valueGetBool(data, "UpdateOnAppEnterForeground");
    //评论
    _reviewEnable = valueGetBool(data, "Review", "enable");
    _reivewiOSUrl = valueGetString(data, "Review", "iOSUrl");
    _reivewAndroidUrl = valueGetString(data, "Review", "androidUrl");
    _reviewSucessTime = valueGetInt(data, "Review", "successTime");
    _reviewNeverShowIfSuccess = valueGetBool(data, "Review", "neverShowIfSuccess");
    //AD
    _adConfigUpdateUrl = valueGetString(data, "ADConfigUpdateUrl");
    //TODO: new config
}

//更新配置文件
void TheConfig::updateConfig(){
    if (_configUpdateUrl.length() == 0) {
        log("不存在配置文件更新地址");
        return;
    }
    
    request(_configUpdateUrl, [&](std::string str){
        if (str.length() > 0) {
            //update success
            FileUtils::getInstance()->writeStringToFile(str, _configFile);
            //reload data
            this->loadConfig();
        }
        else {
            //update fail
        }
    });
}

//评论
bool TheConfig::reviewEnable(){
    return _reviewEnable;
}

std::string TheConfig::getReviewUrl(){
    std::string reviewUrl = "";
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
    reviewUrl = _reivewiOSUrl;
#elif (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
    reviewUrl = _reivewAndroidUrl;
#endif
    return reviewUrl;
}

int TheConfig::getReviewSuccessTime(){
    return _reviewSucessTime;
}
bool TheConfig::neverShowIfReviewSuccess(){
    return _reviewNeverShowIfSuccess;
}

//AD
std::string TheConfig::getADConfigUrl(){
    return _adConfigUpdateUrl;
}
