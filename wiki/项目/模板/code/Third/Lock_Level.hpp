//
//  Lock_Level.hpp
//  MyGame3
//
//  Created by apple on 2017/8/14.
//
//

#ifndef Lock_Level_hpp
#define Lock_Level_hpp

#include <stdio.h>
#include "string"

class Lock_Level {
    
public:
    static Lock_Level* getInstance();
    
    bool isLocked(int level);
    //设置免费试用次数
    void unlock(int level, int times);
    //免费试用一次
    void freeUseLevel(int level);
    void unlockAllLevel();
    
    Lock_Level();
    ~Lock_Level();
    
private:
    std::string getKey(int level);
};

#endif /* Lock_Level_hpp */
