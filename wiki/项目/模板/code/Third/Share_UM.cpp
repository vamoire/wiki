//
//  Share_UM.cpp
//  MyGame3
//
//  Created by apple on 2017/8/14.
//
//

#include "Share_UM.hpp"

#include "CCUMSocialSDK.h"
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
#include "Share_UM_iOS.h"
#endif

static Share_UM* __sShare_UM = nullptr;
Share_UM* Share_UM::getInstance(){
    if (__sShare_UM == nullptr) {
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
        __sShare_UM = new Share_UM_iOS();
#else
        __sShare_UM = new Share_UM();
#endif
    }
    return __sShare_UM;
}

void Share_UM::setShareInfo(std::string title, std::string message, std::string image, std::string url){
    _title = title;
    _message = message;
    _image = image;
    _url = url;
}

bool Share_UM::isWeChatEnabled() {
    return true;
}

static std::function<void(bool)>__sShareallback = nullptr;
void Share_UM::shareToWeChatCircle(std::function<void(bool)>callback){
    __sShareallback = callback;
    auto sdk = umeng::social::CCUMSocialSDK::create();
#ifdef DEBUG
    sdk->setLogEnable(true);
#endif
    sdk->directShare(umeng::social::Platform::WEIXIN_CIRCLE, _message.c_str(), _title.c_str(), _url.c_str(), _image.c_str(), [](int platform, int stCode, const string& errorMsg){
        log("%d, %d, %s", platform, stCode, errorMsg.c_str());
        if (__sShareallback == nullptr) {
            return;
        }
        if ( stCode == 100 )
        {
            log("开始分享");
        }else if (stCode == 200) {
            log("分享成功");
            __sShareallback(true);
        }else {
            log("分享失败");
            __sShareallback(false);
        }
    });
}

bool Share_UM::openShareEnabled(){
    return false;
}

void Share_UM::openShare(std::function<void(bool)>callback){
    __sShareallback = callback;
    auto sdk = umeng::social::CCUMSocialSDK::create();
#ifdef DEBUG
    sdk->setLogEnable(true);
#endif
    vector<int>* platfroms = new vector<int>();
    platfroms->push_back(umeng::social::Platform::WEIXIN);
    platfroms->push_back(umeng::social::Platform::WEIXIN_CIRCLE);
    platfroms->push_back(umeng::social::Platform::SINA);
    sdk->openShare(platfroms, _message.c_str(), _title.c_str(), _image.c_str(), _url.c_str(), [](int platform, int stCode, const string& errorMsg){
        log("%d, %d, %s", platform, stCode, errorMsg.c_str());
        if (__sShareallback == nullptr) {
            return;
        }
        if ( stCode == 100 )
        {
            log("开始分享");
        }else if (stCode == 200) {
            log("分享成功");
            __sShareallback(true);
        }else {
            log("分享失败");
            __sShareallback(false);
        }
    });
}

Share_UM::Share_UM():_title("游戏大冒险︱连爸妈都爱玩的宝宝观察力游戏")
,_message("如果游戏可以达到一些教育的目的，为什么不让宝宝来试试呢？ http://api.cnafb.com/webservice/v2.0/apps/share_afb_game.php")
,_image("https://api.cnafb.com/webservice/v2.1/shareImage/Icon-300.png")
,_url("http://api.cnafb.com/webservice/v2.0/apps/share_afb_game.php")
{
    
}
Share_UM::~Share_UM(){
    
}
