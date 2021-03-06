//
//  ReviewManager.hpp
//  AFBChild
//
//  Created by apple on 2017/8/25.
//
//

#ifndef ReviewManager_hpp
#define ReviewManager_hpp

#include "cocos2d.h"

//评论管理
class ReviewManager {
    
public:
    
    static ReviewManager* getInstance();
    
    ReviewManager();
    ~ReviewManager();
    
    //是否允许评论
    bool reviewEnable();
    
    //是否已经成功评论过
    bool isReviewSuccess();
    //设置为已经评论成功过
    void setReviewSuccess(bool success);
    
    //跳转评论
    void openReview(std::function<void(bool)>callback);
    
private:
    
    time_t _reviewStartTime;
    std::function<void(bool)> _reviewCallback;
    
};


#endif /* ReviewManager_hpp */
