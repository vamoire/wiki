//
//  MoreAppManager.hpp
//  AFBChild
//
//  Created by apple on 2017/8/28.
//
//

#ifndef MoreAppManager_hpp
#define MoreAppManager_hpp

#include "cocos2d.h"

//更多应用
class MoreAppManager {
public:
    static MoreAppManager* getInstance();
    MoreAppManager();
    ~MoreAppManager();
    
    //加载配置文件
    void loadConfig();
    //更新配置文件
    void updateConfig();
    
    //弹出单个应用推荐
    void showNewApp();
    
    //弹出所有应用列表
    void showAllAppPage();
    
};

#endif /* MoreAppManager_hpp */
