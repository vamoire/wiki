//
//  AD_Vungle_iOS.m
//  MyGame3
//
//  Created by apple on 2017/8/9.
//
//

#import <Foundation/Foundation.h>
#include "AD_Vungle_iOS.h"
#include <VungleSDK/VungleSDK.h>
#include "AD_Config.hpp"

@interface Vungle_iOS : NSObject <VungleSDKDelegate> {
    void (^_closeBlock)(BOOL, NSString*);
}

-(void)setCloseCallback:(void(^)(BOOL, NSString*))block;

@end

@implementation Vungle_iOS

-(void)setCloseCallback:(void (^)(BOOL, NSString*))block {
    if (block) {
        _closeBlock = [block copy];
    }
}

/**
 * If implemented, this will get called when the SDK has an ad ready to be displayed. Also it will
 * get called with an argument `NO` for `isAdPlayable` when for some reason, there is
 * no ad available, for instance there is a corrupt ad or the OS wiped the cache.
 * Please note that receiving a `NO` here does not mean that you can't play an Ad: if you haven't
 * opted-out of our Exchange, you might be able to get a streaming ad if you call `play`.
 * @param isAdPlayable A boolean indicating if an ad is currently in a playable state
 * @param placementID The ID of a placement which is ready to be played
 */
- (void)vungleAdPlayabilityUpdate:(BOOL)isAdPlayable placementID:(nullable NSString *)placementID{
    NSLog(@"isAdPlayable = %d, placementID = %@", isAdPlayable, placementID);
}
/**
 * If implemented, this will get called when the SDK is about to show an ad. This point
 * might be a good time to pause your game, and turn off any sound you might be playing.
 * @param placementID The placement which is about to be shown.
 */
- (void)vungleWillShowAdForPlacementID:(nullable NSString *)placementID{
    
}

/**
 * If implemented, this method gets called when a Vungle Ad Unit is about to be completely dismissed.
 * At this point, it's recommended to resume your Game or App.
 */
- (void)vungleWillCloseAdWithViewInfo:(nonnull VungleViewInfo *)info placementID:(nonnull NSString *)placementID{
    if (_closeBlock) {
        _closeBlock(info.completedView.boolValue, placementID);
    }
}

/**
 * If implemented, this will get called when VungleSDK has finished initialization.
 * It's only at this point that one can call `playAd:options:placementID:error`
 * and `loadPlacementWithID:` without getting initialization errors
 */
- (void)vungleSDKDidInitialize{
    
}

/**
 * If implemented, this will get called if the VungleSDK fails to initialize.
 * The included NSError object should give some information as to the failure reason.
 * @note If initialization fails, you will need to restart the VungleSDK.
 */
- (void)vungleSDKFailedToInitializeWithError:(NSError *)error{
    
}


@end



AD_Vungle_iOS::AD_Vungle_iOS(): _appID("")
,_rootViewController(nullptr)
,_placementID({})
,_callback(nullptr)
{
    Vungle_iOS* ios = [[Vungle_iOS alloc] init];
    [ios setCloseCallback:^(BOOL completed, NSString *placement) {
        if (this->_callback) {
            this->_callback(completed);
        }
    }];
    [VungleSDK sharedSDK].delegate = ios;
    [[VungleSDK sharedSDK]setLoggingEnabled:YES];
}

//设置应用ID
void AD_Vungle_iOS::setAppID(std::string appID, std::vector<std::string> placementID){
    _appID = appID;
    NSLog(@"Vungle appID = %s", appID.c_str());
    _placementID = placementID;
    NSMutableArray* placementIDArray = [NSMutableArray array];
    for (std::string item : placementID) {
        [placementIDArray addObject:[NSString stringWithUTF8String:item.c_str()]];
    }
    NSError* error = nil;
    if ([[VungleSDK sharedSDK]startWithAppId:[NSString stringWithUTF8String:appID.c_str()] placements:placementIDArray error:&error]) {
        NSLog(@"ok");
    }
}

//设置iOS展示广告view
void AD_Vungle_iOS::setRootViewController(void * root){
    _rootViewController = root;
}

//请求广告
void AD_Vungle_iOS::requestAD(std::string placementID){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return;
    }
    if (![VungleSDK sharedSDK].isInitialized) {
        return;
    }
    if (placementID == "" && _placementID.size() > 0) {
        placementID = _placementID.at(0);
    }
    NSError* error = nil;
    [[VungleSDK sharedSDK] loadPlacementWithID:[NSString stringWithUTF8String:placementID.c_str()] error:&error];
    NSLog(@"请求Vungle广告 = %d", error == nil);
}

//广告是否准备好了
bool AD_Vungle_iOS::isReady(std::string placementID){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return false;
    }
    if (placementID == "" && _placementID.size() > 0) {
        placementID = _placementID.at(0);
    }
    bool isInitialized = [VungleSDK sharedSDK].isInitialized;
    bool cached = [[VungleSDK sharedSDK] isAdCachedForPlacementID:[NSString stringWithUTF8String:placementID.c_str()]];
    return isInitialized && cached;
}

//展示广告
bool AD_Vungle_iOS::showAD(std::function<void(bool)>callback, int placementIdx){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return false;
    }
    if (!isReady()) {
        return false;
    }
    _callback = callback;
    std::string placementID = "";
    if (_placementID.size() > 0) {
        placementID = _placementID.at(placementIdx);
    }
    else {
        return false;
    }
    
    NSError* error = nil;
    NSDictionary *options = @{VunglePlayAdOptionKeyOrientations: @(UIInterfaceOrientationMaskLandscape),
                              VunglePlayAdOptionKeyUser: @"Xia",//userGameID
                              VunglePlayAdOptionKeyIncentivizedAlertBodyText : @"If the video isn't completed you won't get your reward! Are you sure you want to close early?",
                              VunglePlayAdOptionKeyIncentivizedAlertCloseButtonText : @"Close",
                              VunglePlayAdOptionKeyIncentivizedAlertContinueButtonText : @"Keep Watching",
                              VunglePlayAdOptionKeyIncentivizedAlertTitleText : @"Careful!"};
    UIViewController* root = static_cast<UIViewController*>(_rootViewController);
    [[VungleSDK sharedSDK] playAd:root options:options placementID:[NSString stringWithUTF8String:placementID.c_str()] error:&error];
    return error == nil;
}


