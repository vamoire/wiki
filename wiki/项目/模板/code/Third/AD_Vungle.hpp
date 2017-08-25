//
//  AD_Vungle.hpp
//  MyGame3
//
//  Created by apple on 2017/8/9.
//
//

#ifndef AD_Vungle_hpp
#define AD_Vungle_hpp

#include <stdio.h>
#include <vector>
#include "string"
#include <functional>

//Vungle广告
class AD_Vungle {
public:
    static AD_Vungle* getInstance();
    //设置应用ID
    virtual void setAppID(std::string appID, std::vector<std::string> placementID);
    //设置iOS展示广告view
    virtual void setRootViewController(void * root);
    //请求广告
    virtual void requestAD(std::string placementID = "");
    //广告是否准备好了
    virtual bool isReady(std::string placementID = "");
    //展示广告
    virtual bool showAD(std::function<void()>closeCallback, std::string placementID = "");
    
    AD_Vungle();
    ~AD_Vungle();
};

#endif /* AD_Vungle_hpp */
