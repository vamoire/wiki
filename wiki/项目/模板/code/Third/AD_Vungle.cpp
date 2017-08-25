//
//  AD_Vungle.cpp
//  MyGame3
//
//  Created by apple on 2017/8/9.
//
//

#include "AD_Vungle.hpp"
#include "AD_Vungle_iOS.h"

static AD_Vungle* __s_AD_Vungle = nullptr;
AD_Vungle* AD_Vungle::getInstance(){
    if (__s_AD_Vungle == nullptr) {
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
        __s_AD_Vungle = new AD_Vungle_iOS();
#else
        __s_AD_Vungle = new AD_Vungle();
#endif
    }
    return __s_AD_Vungle;
}
//设置应用ID
void AD_Vungle::setAppID(std::string appID, std::vector<std::string> placementID){
    
}

//设置iOS展示广告view
void AD_Vungle::setRootViewController(void * root){
    
}

//请求广告
void AD_Vungle::requestAD(std::string placementID){
    
}

//广告是否准备好了
bool AD_Vungle::isReady(std::string placementID){
    return false;
}

//展示广告
bool AD_Vungle::showAD(std::function<void()>closeCallback, std::string placementID){
    return false;
}

AD_Vungle::AD_Vungle()
{
    
}
AD_Vungle::~AD_Vungle(){}
