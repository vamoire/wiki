//
//  AD_Google.cpp
//  MyGame3
//
//  Created by apple on 2017/8/8.
//
//

#include "AD_Google.hpp"
#include "cocos2d.h"

#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
#include "AD_Google_iOS.h"
#endif

static AD_Google* __s_AD_Google = nullptr;
AD_Google* AD_Google::getInstance(){
    if (__s_AD_Google == nullptr) {
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
        __s_AD_Google = new AD_Google_iOS();
#else
        __s_AD_Google = new AD_Google();
#endif
    }
    return __s_AD_Google;
}

//设置应用ID
void AD_Google::setAppID(std::string appID){
    
}
//设置广告条ID
void AD_Google::setBannerID(std::string bannerID){
    
}
//设置iOS展示广告view
void AD_Google::setRootViewController(void * root){
    
}
//设置测试设备ID
void AD_Google::setTestDevices(std::string deviceID){
    
}
//展示广告条
void AD_Google::showBannerAD(){
    
}
//关闭广告条
void AD_Google::hideBannerAD(){
    
}


//设置全屏广告ID
void AD_Google::setInterstitialID(std::string interstitialID){}
//请求全屏广告
void AD_Google::requestInterstitial(){}
//全屏广告是否准备好
bool AD_Google::interstitialIsReady(){return false;}
//展示全屏广告
bool AD_Google::showInterstitialAD(std::function<void()>closeCallback){return false;}



//设置奖励视频ID
void AD_Google::setRewardedVideoID(std::string rewardedvideoID){}
//请求奖励视频
void AD_Google::requestRewardedVideo(){}
//奖励视频是否准备好
bool AD_Google::rewardedVideoIsReady(){return false;}
//展示奖励视屏
bool AD_Google::showRewardedVideoAD(std::function<void()>rewardCallback, std::function<void()>rewardCloseCallback){return false;}


AD_Google::AD_Google(){
    
}
AD_Google::~AD_Google(){
    
}
