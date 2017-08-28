//
//  AD_Google.hpp
//  MyGame3
//
//  Created by apple on 2017/8/8.
//
//

#ifndef AD_Google_hpp
#define AD_Google_hpp

#include <stdio.h>
#include <vector>
#include "string"
#include <functional>

//谷歌广告
class AD_Google {
public:
    static AD_Google* getInstance();
    //设置应用ID
    virtual void setAppID(std::string appID);
    //设置iOS展示广告view
    virtual void setRootViewController(void * root);
    //设置测试设备ID
    virtual void setTestDevices(std::string deviceID);
    
    
    //广告条
    
    //设置广告条ID
    virtual void setBannerID(std::string bannerID);
    //展示广告条
    virtual void showBannerAD();
    //关闭广告条
    virtual void hideBannerAD();
    
    
    //全屏广告
    
    //设置全屏广告ID
    virtual void setInterstitialID(std::string interstitialID);
    //请求全屏广告
    virtual void requestInterstitial();
    //全屏广告是否准备好
    virtual bool interstitialIsReady();
    //展示全屏广告
    virtual bool showInterstitialAD(std::function<void()>closeCallback);
    
    
    //奖励视频
    
    //设置奖励视频ID
    virtual void setRewardedVideoID(std::string rewardedvideoID);
    //请求奖励视频
    virtual void requestRewardedVideo();
    //奖励视频是否准备好
    virtual bool rewardedVideoIsReady();
    //展示奖励视屏
    virtual bool showRewardedVideoAD(std::function<void(bool)>callback);
    
    AD_Google();
    ~AD_Google();
};

#endif /* AD_Google_hpp */
