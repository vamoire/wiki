//
//  TheConfig.hpp
//  AFBChild
//
//  Created by apple on 2017/8/25.
//
//

#ifndef TheConfig_hpp
#define TheConfig_hpp

#include <stdio.h>
#include "string"

class TheConfig {
    
public:
    
    static TheConfig* getInstance();
    
    TheConfig();
    ~TheConfig();
    
    //加载配置文件
    void loadConfig();
    //更新配置文件
    void updateConfig();
    
    //评论
    bool reviewEnable();
    std::string getReviewUrl();
    int getReviewSuccessTime();
    bool neverShowIfReviewSuccess();
    
    //AD
    std::string getADConfigUrl();
    
private:
    std::string _configFile;
    std::string _defaultConfigFile;
    std::string _configUpdateUrl;
    bool _updateOnAppEnterForeground;
    
    //评论
    bool _reviewEnable;
    std::string _reivewiOSUrl;
    std::string _reivewAndroidUrl;
    int _reviewSucessTime;
    bool _reviewNeverShowIfSuccess;
    
    //AD
    std::string _adConfigUpdateUrl;
    
};

#endif /* TheConfig_hpp */
