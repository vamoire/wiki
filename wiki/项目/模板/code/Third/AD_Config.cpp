//
//  AD_Config.cpp
//  MyGame3
//
//  Created by apple on 2017/8/10.
//
//

#include "AD_Config.hpp"
#include "AD_Google.hpp"
#include "AD_Vungle.hpp"

#include "cocos2d.h"
#include "Base.hpp"
#include "TheConfig.hpp"

USING_NS_CC;

static AD_Config* __sAD_Config = nullptr;
AD_Config* AD_Config::getInstance(){
    if (__sAD_Config == nullptr) {
        __sAD_Config = new AD_Config();
    }
    return __sAD_Config;
}

void AD_Config::setRootViewController(void* root){
    AD_Google::getInstance()->setRootViewController(root);
    AD_Vungle::getInstance()->setRootViewController(root);
}

void AD_Config::loadConfig(){
    cocos2d::FileUtils* fu = cocos2d::FileUtils::getInstance();
    std::string file = fu->getWritablePath() + "ADConfig";
    if (!fu->isFileExist(file)) {
        //default
        file = fu->fullPathForFilename("ADConfig.json");
    }
    Value v = mx::readJsonFile(file);
    std::string googleApp = mx::valueGetString(v, "google", "app");
    std::string googleBanner = mx::valueGetString(v, "google", "banner");
    std::string googleInterstitial = mx::valueGetString(v, "google", "interstitial");
    std::string googleReward = mx::valueGetString(v, "google", "reward");
    //google
    AD_Google* gad = AD_Google::getInstance();
    gad->setAppID(googleApp);
    gad->setBannerID(googleBanner);
    gad->setInterstitialID(googleInterstitial);
    gad->setRewardedVideoID(googleReward);
    //vungle
    std::string vungleApp = mx::valueGetString(v, "vungle", "app");
    Value vungleReward = mx::valueGetValue(v, "vungle", "reward");
    if (vungleReward.getType() == Value::Type::VECTOR) {
        ValueVector arr = vungleReward.asValueVector();
        std::vector<std::string> vc;
        for (Value item : arr) {
            if (item.getType() == Value::Type::STRING) {
                vc.push_back(item.asString());
            }
        }
        if (vc.size() > 0) {
            //vungle
            AD_Vungle* vad = AD_Vungle::getInstance();
            vad->setAppID(vungleApp, vc);
        }
    }
    //other
    int p = mx::valueGetInt(v, "googleReward");
    int r = rand()%100;
    _useGoogleReward = (r <= p);
    
    _shareUnlockTimes = mx::valueGetInt(v, "shareUnlock");
    _reviewUnlockTimes = mx::valueGetInt(v, "reviewUnlock");
    _rewardUnlockTimes = mx::valueGetInt(v, "rewardUnlock");
    _reviewUnlockHz = mx::valueGetInt(v, "reviewUnlockHz");
    
}

void AD_Config::updateConfig(){
    //TODO:请求服务器配置文件
    std::string url = TheConfig::getInstance()->getADConfigUrl();
    mx::request(url, [=](std::string str){
        if (str != "") {
            cocos2d::FileUtils* fu = cocos2d::FileUtils::getInstance();
            std::string file = fu->getWritablePath() + "/ADConfig";
            FileUtils::getInstance()->writeStringToFile(str, file);
            loadConfig();
        }
    });
    mx::valueGetString(Value(), "", 0, "", nullptr);
}

//获取当前奖励视频种类
bool AD_Config::isUseGoogleReward(){
    return _useGoogleReward;
}


#define AD_Config_Open "Key_AD_Config_isOpen"

//获取广告开关
bool AD_Config::isOpen(){
    return _adOpen;
}

//设置广告开关
void AD_Config::setOpen(bool open){
    cocos2d::UserDefault::getInstance()->setBoolForKey(AD_Config_Open, open);
    cocos2d::UserDefault::getInstance()->flush();
    _adOpen = open;
}

//获取分享解锁的次数
int AD_Config::getShareUnlockTimes(){
    return _shareUnlockTimes;
}
//获取评论解锁的次数
int AD_Config::getReviewUnlockTimes(){
    return _reviewUnlockTimes;
}
//获取广告解锁的次数
int AD_Config::getRewardUnlockTimes(){
    return _rewardUnlockTimes;
}
//获取显示评论解锁的频率
int AD_Config::getReviewUnlockHz(){
    return _reviewUnlockHz;
}

AD_Config::AD_Config():_useGoogleReward(true)
,_adOpen(true)
,_shareUnlockTimes(3)
,_reviewUnlockTimes(3)
,_rewardUnlockTimes(3)
,_reviewUnlockHz(0)
{
#ifdef DEBUG
    AD_Google::getInstance()->setTestDevices("b6c9f06dea560adb09f6e23302f6396d");
#endif
    //加载配置
    loadConfig();
    
    //获取广告开关
    _adOpen = cocos2d::UserDefault::getInstance()->getBoolForKey(AD_Config_Open, true);
    
    //更新广告配置
    updateConfig();
}
AD_Config::~AD_Config(){
    
}
