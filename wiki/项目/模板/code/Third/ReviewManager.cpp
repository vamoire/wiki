//
//  ReviewManager.cpp
//  AFBChild
//
//  Created by apple on 2017/8/25.
//
//

#include "ReviewManager.hpp"
#include "Base.hpp"
#include "TheConfig.hpp"

#define ReviewManager_Key_isReviewSuccess "ReviewManager_Key_isReviewSuccess"

USING_NS_CC;

static ReviewManager* __sReviewManager = nullptr;
ReviewManager* ReviewManager::getInstance(){
    if (__sReviewManager == nullptr) {
        __sReviewManager = new ReviewManager();
    }
    return __sReviewManager;
}

ReviewManager::ReviewManager(){
    //切回应用通知
    Director::getInstance()->getEventDispatcher()->addCustomEventListener(Notification_Key_appEnterForeground, [&](EventCustom*){
        //验证时间是否正确
        bool success = true;
        int successTime = TheConfig::getInstance()->getReviewSuccessTime();
        if (successTime > 0) {
            //需要验证
            time_t now = mx::getTimeNow();
            success = (now - _reviewStartTime) >= successTime;
        }
        
        if (_reviewCallback) {
            _reviewCallback(success);
        }
    });
}
ReviewManager::~ReviewManager(){
    
}

//是否允许评论
bool ReviewManager::reviewEnable(){
    return TheConfig::getInstance()->reviewEnable();
}

//是否已经成功评论过
bool ReviewManager::isReviewSuccess(){
    bool ret = UserDefault::getInstance()->getBoolForKey(ReviewManager_Key_isReviewSuccess, false);
    return ret;
}

//设置为已经评论成功过
void ReviewManager::setReviewSuccess(bool success){
    UserDefault::getInstance()->setBoolForKey(ReviewManager_Key_isReviewSuccess, success);
    UserDefault::getInstance()->flush();
}

//跳转评论
void ReviewManager::openReview(std::function<void(bool)>callback){
    _reviewCallback = callback;
    //记录当前时间
    _reviewStartTime = mx::getTimeNow();
    std::string url = TheConfig::getInstance()->getReviewUrl();
    Application::getInstance()->openURL(url);
}
