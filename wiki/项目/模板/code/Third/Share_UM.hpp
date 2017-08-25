//
//  Share_UM.hpp
//  MyGame3
//
//  Created by apple on 2017/8/14.
//
//

#ifndef Share_UM_hpp
#define Share_UM_hpp

#include <stdio.h>
#include "string"
#include <functional>

class Share_UM {
    
public:
    static Share_UM* getInstance();
    
    void setShareInfo(std::string title, std::string message, std::string image, std::string url);
    virtual bool isWeChatEnabled();
    void shareToWeChatCircle(std::function<void(bool)>callback);
    virtual bool openShareEnabled();
    void openShare(std::function<void(bool)>callback);
    
    Share_UM();
    ~Share_UM();
    
private:
    std::string _title;
    std::string _message;
    std::string _image;
    std::string _url;
};

#endif /* Share_UM_hpp */
