//
//  AD_Google_iOS.m
//  MyGame3
//
//  Created by apple on 2017/8/8.
//
//

#import <Foundation/Foundation.h>
#import "AD_Google_iOS.h"
#include <GoogleMobileAds/GoogleMobileAds.h>
#include "string"
#include "AD_Config.hpp"

#pragma mark - 类型转换

static GADInterstitial* toGADInterstitial(void* p) {
    GADInterstitial* gad = static_cast<GADInterstitial*>(p);
    return gad;
}

#pragma mark - AD_Google_iOS

@interface Google_iOS : NSObject <GADBannerViewDelegate, GADInterstitialDelegate, GADRewardBasedVideoAdDelegate> {
    void(^_closeCallback)();
    void(^_rewardCallback)();
    void(^_rewardCloseCallback)();
}

-(void)setCloseCallback:(void(^)())block;

-(void)setRewardCallback:(void(^)())block;

-(void)setRewardCloseCallback:(void(^)())block;

@end

@implementation Google_iOS

-(void)setCloseCallback:(void (^)())block {
    if (block) {
        _closeCallback = [block copy];
    }
}

-(void)setRewardCallback:(void (^)())block {
    if (block) {
        _rewardCallback = [block copy];
    }
}

-(void)setRewardCloseCallback:(void (^)())block {
    if (block) {
        _rewardCloseCallback = [block copy];
    }
}

#pragma mark - GADBannerViewDelegate

#pragma mark Ad Request Lifecycle Notifications

/// Tells the delegate that an ad request successfully received an ad. The delegate may want to add
/// the banner view to the view hierarchy if it hasn't been added yet.
- (void)adViewDidReceiveAd:(GADBannerView *)bannerView{
    
}

/// Tells the delegate that an ad request failed. The failure is normally due to network
/// connectivity or ad availablility (i.e., no fill).
- (void)adView:(GADBannerView *)bannerView didFailToReceiveAdWithError:(GADRequestError *)error{
    NSLog(@"%@", error.description);
}

#pragma mark Click-Time Lifecycle Notifications

/// Tells the delegate that a full screen view will be presented in response to the user clicking on
/// an ad. The delegate may want to pause animations and time sensitive interactions.
- (void)adViewWillPresentScreen:(GADBannerView *)bannerView{
    
}

/// Tells the delegate that the full screen view will be dismissed.
- (void)adViewWillDismissScreen:(GADBannerView *)bannerView{
    
}

/// Tells the delegate that the full screen view has been dismissed. The delegate should restart
/// anything paused while handling adViewWillPresentScreen:.
- (void)adViewDidDismissScreen:(GADBannerView *)bannerView{
    
}

/// Tells the delegate that the user click will open another app, backgrounding the current
/// application. The standard UIApplicationDelegate methods, like applicationDidEnterBackground:,
/// are called immediately before this method is called.
- (void)adViewWillLeaveApplication:(GADBannerView *)bannerView{
    
}


#pragma mark - GADInterstitialDelegate

#pragma mark Ad Request Lifecycle Notifications

/// Called when an interstitial ad request succeeded. Show it at the next transition point in your
/// application such as when transitioning between view controllers.
- (void)interstitialDidReceiveAd:(GADInterstitial *)ad{
    
}

/// Called when an interstitial ad request completed without an interstitial to
/// show. This is common since interstitials are shown sparingly to users.
- (void)interstitial:(GADInterstitial *)ad didFailToReceiveAdWithError:(GADRequestError *)error{
    NSLog(@"%@", error.description);
}

#pragma mark Display-Time Lifecycle Notifications

/// Called just before presenting an interstitial. After this method finishes the interstitial will
/// animate onto the screen. Use this opportunity to stop animations and save the state of your
/// application in case the user leaves while the interstitial is on screen (e.g. to visit the App
/// Store from a link on the interstitial).
- (void)interstitialWillPresentScreen:(GADInterstitial *)ad{
    
}

/// Called when |ad| fails to present.
- (void)interstitialDidFailToPresentScreen:(GADInterstitial *)ad{
    
}

/// Called before the interstitial is to be animated off the screen.
- (void)interstitialWillDismissScreen:(GADInterstitial *)ad{
    
}

/// Called just after dismissing an interstitial and it has animated off the screen.
- (void)interstitialDidDismissScreen:(GADInterstitial *)ad{
    if (_closeCallback) {
        _closeCallback();
    }
}

/// Called just before the application will background or terminate because the user clicked on an
/// ad that will launch another application (such as the App Store). The normal
/// UIApplicationDelegate methods, like applicationDidEnterBackground:, will be called immediately
/// before this.
- (void)interstitialWillLeaveApplication:(GADInterstitial *)ad{
    
}


#pragma mark - GADRewardBasedVideoAdDelegate

/// Tells the delegate that the reward based video ad has rewarded the user.
- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd
   didRewardUserWithReward:(GADAdReward *)reward{
    if (_rewardCallback) {
        _rewardCallback();
    }
}


/// Tells the delegate that the reward based video ad failed to load.
- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd
    didFailToLoadWithError:(NSError *)error{
    NSLog(@"%@", error.description);
}

/// Tells the delegate that a reward based video ad was received.
- (void)rewardBasedVideoAdDidReceiveAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd{
    
}

/// Tells the delegate that the reward based video ad opened.
- (void)rewardBasedVideoAdDidOpen:(GADRewardBasedVideoAd *)rewardBasedVideoAd{
    
}

/// Tells the delegate that the reward based video ad started playing.
- (void)rewardBasedVideoAdDidStartPlaying:(GADRewardBasedVideoAd *)rewardBasedVideoAd{
    
}

/// Tells the delegate that the reward based video ad closed.
- (void)rewardBasedVideoAdDidClose:(GADRewardBasedVideoAd *)rewardBasedVideoAd{
    if (_rewardCloseCallback) {
        _rewardCloseCallback();
    }
}

/// Tells the delegate that the reward based video ad will leave the application.
- (void)rewardBasedVideoAdWillLeaveApplication:(GADRewardBasedVideoAd *)rewardBasedVideoAd{
    
}


@end


#pragma mark - AD_Google_iOS

AD_Google_iOS::AD_Google_iOS() : _bannerID("")
,_bannerView({})
,_rootViewController(nullptr)
,_testDeviceID("")
,_interstitialID("")
,_interstitial(nullptr)
,_rewardedVideoID("")
,_closeCallback(nullptr)
,_rewardCallback(nullptr)
,_rewardCloseCallback(nullptr)
{
    Google_iOS* ios = [[Google_iOS alloc] init];
    [ios setRewardCallback:^(){
        if (this->_rewardCallback) {
            this->_rewardCallback();
        }
    }];
    [ios setRewardCloseCallback:^(){
        if (this->_rewardCloseCallback) {
            this->_rewardCloseCallback();
        }
    }];
    [GADRewardBasedVideoAd sharedInstance].delegate = ios;
}

//设置应用ID
void AD_Google_iOS::setAppID(std::string appID){
    NSString* str = [NSString stringWithUTF8String:appID.c_str()];
    [GADMobileAds configureWithApplicationID:str];
}
//设置广告条ID
void AD_Google_iOS::setBannerID(std::string bannerID){
    _bannerID = bannerID;
}
//设置iOS展示广告view
void AD_Google_iOS::setRootViewController(void * root){
    _rootViewController = root;
}
//设置测试设备ID
void AD_Google_iOS::setTestDevices(std::string deviceID){
    _testDeviceID = deviceID;
}
//展示广告条
void AD_Google_iOS::showBannerAD(){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return;
    }
    
    UIViewController* root = static_cast<UIViewController*>(_rootViewController);
    GADBannerView* bannerView = [[GADBannerView alloc] initWithAdSize:kGADAdSizeSmartBannerPortrait origin:CGPointZero];
    
    //底部居中
    CGSize winsize = [[UIScreen mainScreen] bounds].size;
    if (winsize.width < winsize.height) {
        winsize = CGSizeMake(winsize.height, winsize.width);
    }
    bannerView.center = CGPointMake(winsize.width / 2, winsize.height - bannerView.adSize.size.height / 2);
    
    [root.view addSubview:bannerView];
    NSLog(@"%s", _bannerID.c_str());
    bannerView.adUnitID = [NSString stringWithUTF8String:_bannerID.c_str()];
    bannerView.rootViewController = root;
    bannerView.delegate = [[Google_iOS alloc] init];
    bannerView.autoloadEnabled = YES;
    
    GADRequest* request = [GADRequest request];
    if (_testDeviceID.length() > 0) {
        request.testDevices = @[
                                [NSString stringWithUTF8String:_testDeviceID.c_str()]
                                ];
    }
    [bannerView loadRequest:request];
    
    _bannerView.push_back(bannerView);
}
//关闭广告条
void AD_Google_iOS::hideBannerAD(){
    for (void* item : _bannerView) {
        GADBannerView* view = (GADBannerView*)item;
        [view removeFromSuperview];
        [view release];
    }
    _bannerView.clear();
}




//设置全屏广告ID
void AD_Google_iOS::setInterstitialID(std::string interstitialID){
    _interstitialID = interstitialID;
    GADInterstitial* gad = toGADInterstitial(_interstitial);
    if (gad != nullptr) {
        [gad release];
    }
    gad = [[GADInterstitial alloc]initWithAdUnitID:[NSString stringWithUTF8String:_interstitialID.c_str()]];
    Google_iOS* ios = [[Google_iOS alloc] init];
    [ios setCloseCallback:^{
        if (this->_closeCallback) {
            this->_closeCallback();
        }
    }];
    gad.delegate = ios;
    _interstitial = gad;
}
//请求全屏广告
void AD_Google_iOS::requestInterstitial(){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return;
    }
    GADInterstitial* gad = toGADInterstitial(_interstitial);
    if (gad != nullptr && gad.hasBeenUsed) {
        //重新初始化
        setInterstitialID(_interstitialID);
        gad = toGADInterstitial(_interstitial);
    }
    if (gad != nullptr) {
        GADRequest *request = [GADRequest request];
        if (_testDeviceID.length() > 0) {
            request.testDevices = @[
                                    [NSString stringWithUTF8String:_testDeviceID.c_str()]
                                    ];
        }
        [gad loadRequest:request];
    }
}
//全屏广告是否准备好
bool AD_Google_iOS::interstitialIsReady(){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return false;
    }
    GADInterstitial* gad = toGADInterstitial(_interstitial);
    if (gad != nullptr) {
        return gad.isReady && !gad.hasBeenUsed;
    }
    return false;
}
//展示全屏广告
bool AD_Google_iOS::showInterstitialAD(std::function<void()>closeCallback){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return false;
    }
    GADInterstitial* gad = toGADInterstitial(_interstitial);
    if (gad != nullptr && gad.isReady) {
        _closeCallback = closeCallback;
        UIViewController* root = static_cast<UIViewController*>(_rootViewController);
        [gad presentFromRootViewController:root];
        return true;
    }
    return false;
}





//设置奖励视频ID
void AD_Google_iOS::setRewardedVideoID(std::string rewardedvideoID){
    _rewardedVideoID = rewardedvideoID;
}

//请求奖励视频
void AD_Google_iOS::requestRewardedVideo(){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return;
    }
    GADRequest *request = [GADRequest request];
    if (_testDeviceID.length() > 0) {
        request.testDevices = @[
                                [NSString stringWithUTF8String:_testDeviceID.c_str()]
                                ];
    }
    [[GADRewardBasedVideoAd sharedInstance] loadRequest:request
                                           withAdUnitID:[NSString stringWithUTF8String:_rewardedVideoID.c_str()]];
}

//奖励视频是否准备好
bool AD_Google_iOS::rewardedVideoIsReady(){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return false;
    }
    return [[GADRewardBasedVideoAd sharedInstance] isReady];
}

//展示奖励视屏
bool AD_Google_iOS::showRewardedVideoAD(std::function<void()>rewardCallback, std::function<void()>rewardCloseCallback){
    //广告开关
    if (!AD_Config::getInstance()->isOpen()) {
        return false;
    }
    if ([[GADRewardBasedVideoAd sharedInstance] isReady]) {
        _rewardCallback = rewardCallback;
        _rewardCloseCallback = rewardCloseCallback;
        UIViewController* root = static_cast<UIViewController*>(_rootViewController);
        [[GADRewardBasedVideoAd sharedInstance] presentFromRootViewController:root];
        return true;
    }
    return false;
}


