# CSLoader使用

## 1. 添加csb
<pre>
	auto winSize = Director::getInstance()->getVisibleSize();
	auto node = CSLoader::createNode("List/List.csb");
	node->setPosition(Director::getInstance()->getVisibleOrigin());
	node->setContentSize(winSize);
	Helper::doLayout(node);
	this->addChild(node);
</pre>

## 2. 查找对象
<pre>
    //back menu
    auto back = (Button*)findChild(node, "Back");
    back->addClickEventListener([=](Ref*){

    });
</pre>

## 3. 补充
cocos2dx后续版本将不对csb维护