//
//  Share_UM_iOS.hpp
//  MyGame3
//
//  Created by apple on 2017/8/15.
//
//

#ifndef Share_UM_iOS_hpp
#define Share_UM_iOS_hpp

#include "Share_UM.hpp"

class Share_UM_iOS : public Share_UM {
    
public:
    virtual bool isWeChatEnabled() override;
    virtual bool openShareEnabled() override;
};

#endif /* Share_UM_iOS_hpp */
