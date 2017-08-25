//
//  Share_UM_iOS.cpp
//  MyGame3
//
//  Created by apple on 2017/8/15.
//
//

#include "Share_UM_iOS.h"
#include "CCUMSocialSDK.h"
#include <UMSocialCore/UMSocialManager.h>

bool Share_UM_iOS::isWeChatEnabled() {
    bool enabled = false;
    NSArray* array = [[UMSocialManager defaultManager] platformTypeArray];
    for (NSNumber* obj in array) {
        int i = obj.intValue;
        if (i == umeng::social::Platform::WEIXIN_CIRCLE) {
            enabled = true;
            break;
        }
    }
    return enabled;
}

bool Share_UM_iOS::openShareEnabled(){
    bool enabled = false;
    NSArray* array = [[UMSocialManager defaultManager] platformTypeArray];
    for (NSNumber* obj in array) {
        int i = obj.intValue;
        if (i == umeng::social::Platform::WEIXIN_CIRCLE || i == umeng::social::Platform::WEIXIN || i == umeng::social::Platform::SINA) {
            enabled = true;
            break;
        }
    }
    return enabled;
}
