//
//  AD_Config.hpp
//  MyGame3
//
//  Created by apple on 2017/8/10.
//
//

#ifndef AD_Config_hpp
#define AD_Config_hpp

#include <stdio.h>
#include <functional>

class AD_Config {
    
public:
    
    static AD_Config* getInstance();
    
    void setRootViewController(void* root);
    
    void loadConfig();
    
    void updateConfig();
    
    //奖励视频广告
    void requestRewardAD();
    bool rewardIsReady();
    void showRewardAD(std::function<void(bool)>callback);
    
    //获取当前奖励视频种类
    bool isUseGoogleReward();
    
    //获取广告开关
    bool isOpen();
    //设置广告开关
    void setOpen(bool open);
    
    //获取分享解锁的次数
    int getShareUnlockTimes();
    //获取评论解锁的次数
    int getReviewUnlockTimes();
    //获取广告解锁的次数
    int getRewardUnlockTimes();
    //获取显示评论解锁的频率
    int getReviewUnlockHz();
    
    AD_Config();
    ~AD_Config();
    
private:
    bool _useGoogleReward;
    bool _adOpen;
    
    int _shareUnlockTimes;
    int _reviewUnlockTimes;
    int _rewardUnlockTimes;
    int _reviewUnlockHz;
};

#endif /* AD_Config_hpp */
