//
//  MoreAppManager.cpp
//  AFBChild
//
//  Created by apple on 2017/8/28.
//
//

#include "MoreAppManager.hpp"

USING_NS_CC;

static MoreAppManager* __sMoreAppManager = nullptr;
MoreAppManager* MoreAppManager::getInstance(){
    if (__sMoreAppManager == nullptr) {
        __sMoreAppManager = new MoreAppManager();
    }
    return __sMoreAppManager;
}

MoreAppManager::MoreAppManager(){
    
}

MoreAppManager::~MoreAppManager(){
    
}

//加载配置文件
void MoreAppManager::loadConfig(){
    
}

//更新配置文件
void MoreAppManager::updateConfig(){
    
}

//弹出单个应用推荐
void MoreAppManager::showNewApp(){
    auto layer = Layer::create();
    
    
    
    auto scene = Director::getInstance()->getRunningScene();
    scene->addChild(layer);
}

//弹出所有应用列表
void MoreAppManager::showAllAppPage(){
    auto layer = Layer::create();
    
    
    
    auto scene = Director::getInstance()->getRunningScene();
    scene->addChild(layer);
}

