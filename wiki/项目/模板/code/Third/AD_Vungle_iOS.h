//
//  AD_Vungle_iOS.h
//  MyGame3
//
//  Created by apple on 2017/8/9.
//
//

#ifndef AD_Vungle_iOS_h
#define AD_Vungle_iOS_h

#include "AD_Vungle.hpp"

class AD_Vungle_iOS : public AD_Vungle {
public:
    //设置应用ID
    virtual void setAppID(std::string appID, std::vector<std::string> placementID) override;
    //设置iOS展示广告view
    virtual void setRootViewController(void * root) override;
    //请求广告
    virtual void requestAD(std::string placementID = "") override;
    //广告是否准备好了
    virtual bool isReady(std::string placementID = "") override;
    //展示广告
    virtual bool showAD(std::function<void()>closeCallback, std::string placementID = "") override;
    
    AD_Vungle_iOS();
private:
    std::string _appID;
    void* _rootViewController;
    std::vector<std::string> _placementID;
    std::function<void()>_closeCallback;
};


#endif /* AD_Vungle_iOS_h */
