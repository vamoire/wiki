//
//  AD_Google_iOS.h
//  MyGame3
//
//  Created by apple on 2017/8/8.
//
//

#ifndef AD_Google_iOS_h
#define AD_Google_iOS_h

#include "AD_Google.hpp"

class AD_Google_iOS : public AD_Google{
public:
    
    AD_Google_iOS();
    
    //设置应用ID
    virtual void setAppID(std::string appID) override;
    //设置iOS展示广告view
    virtual void setRootViewController(void * root) override;
    //设置测试设备ID
    virtual void setTestDevices(std::string deviceID) override;
    
    
    //广告条
    
    //设置广告条ID
    virtual void setBannerID(std::string bannerID) override;
    //展示广告条
    virtual void showBannerAD() override;
    //关闭广告条
    virtual void hideBannerAD() override;
    
    
    
    //全屏广告
    
    //设置全屏广告ID
    virtual void setInterstitialID(std::string interstitialID) override;
    //请求全屏广告
    virtual void requestInterstitial() override;
    //全屏广告是否准备好
    virtual bool interstitialIsReady() override;
    //展示全屏广告
    virtual bool showInterstitialAD(std::function<void()>closeCallback) override;
    
    
    //奖励视频
    
    //设置奖励视频ID
    virtual void setRewardedVideoID(std::string rewardedvideoID) override;
    //请求奖励视频
    virtual void requestRewardedVideo() override;
    //奖励视频是否准备好
    virtual bool rewardedVideoIsReady() override;
    //展示奖励视屏
    virtual bool showRewardedVideoAD(std::function<void()>rewardCallback, std::function<void()>rewardCloseCallback) override;
    
private:
    std::string _bannerID;
    std::vector<void*> _bannerView;
    void* _rootViewController;
    std::string _testDeviceID;
    std::string _interstitialID;
    void* _interstitial;
    std::string _rewardedVideoID;
    std::function<void()>_closeCallback;
    std::function<void()>_rewardCallback;
    std::function<void()>_rewardCloseCallback;
};



#endif /* AD_Google_iOS_h */
