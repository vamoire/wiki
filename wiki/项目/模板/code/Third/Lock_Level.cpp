//
//  Lock_Level.cpp
//  MyGame3
//
//  Created by apple on 2017/8/14.
//
//

#include "Lock_Level.hpp"
#include "cocos2d.h"

static Lock_Level* __sLock_Level = nullptr;
Lock_Level* Lock_Level::getInstance(){
    if (__sLock_Level == nullptr) {
        __sLock_Level = new Lock_Level();
    }
    return __sLock_Level;
}

bool Lock_Level::isLocked(int level){
    //关卡0、1默认解锁
    if (level == 0 || level == 1) {
        return false;
    }
    //-1:已经解锁 0:未解锁 >0:免费试用次数
    int times = cocos2d::UserDefault::getInstance()->getIntegerForKey(getKey(level).c_str(), 0);
    return times == 0;
}
void Lock_Level::unlock(int level, int times){
    cocos2d::UserDefault::getInstance()->setIntegerForKey(getKey(level).c_str(), times);
    cocos2d::UserDefault::getInstance()->flush();
}

void Lock_Level::freeUseLevel(int level){
    //-1:已经解锁 0:未解锁 >0:免费试用次数
    //-1:已经解锁 0:未解锁 >0:免费试用次数
    int times = cocos2d::UserDefault::getInstance()->getIntegerForKey(getKey(level).c_str(), 0);
    if (times > 0) {
        this->unlock(level, times - 1);
    }
}

void Lock_Level::unlockAllLevel() {
    for (int i = 0; i < 10; ++i) {
        this->unlock(i, -1);
    }
}

Lock_Level::Lock_Level(){
    
}
Lock_Level::~Lock_Level(){
    
}


std::string Lock_Level::getKey(int level){
    std::stringstream ss;
    ss<<level;
    std::string key = "game_level_lock_" + ss.str();
    return key;
}


