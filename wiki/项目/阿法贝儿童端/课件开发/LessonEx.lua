local ExLesson = class("ExLesson", cc.LuaLessonLayer)

function ExLesson:createScene()
	local scene = cc.Scene:create()
	local layer = self:create()
	scene:addChild(layer)
	return scene
end

function ExLesson:ctor()
	-- 获取课件脚本所在的目录
	self.rootPath = self:getRootPath("game.lua")
    -- 进入场景动画是否完成
    self.isEnterTransitionFinish = false
    -- 背景音乐文件
    self.bgmFile = ""
    -- 游戏运行状态
    self.gameRunning = false
    -- 游戏结束
    self.gameOver = false
    -- 游戏数据存储文件
    self.gameDataFile = self.rootPath .. "game.dat"

    -- 生成随机种子
    math.randomseed(tostring(os.time()):reverse():sub(1, 6))

    -- 注册事件
    self:registerScriptHandler(function(event)
        -- event
        if event == "enterTransitionFinish" then
            self.isEnterTransitionFinish = true
            self:_enterTransitionFinish()
            self:enterTransitionFinish()
        elseif event == "exitTransitionStart" then
            self:exitTransitionStart()
            self:_exitTransitionStart()
        end
    end)

    -- 暂停恢复事件
    self:addPauseResumeEvent(self, function()
        if self.gameOver then
            return
        end
        self.gameRunning = false
        self:pause()
        cc.SimpleAudioEngine:getInstance():pauseAllEffects()
        -- 自定义游戏暂停事件
        self._targetsToResume = self:getActionManager():pauseAllRunningActions()
        self:pauseGameEvent()
    end, function()
        if self.gameOver then
            return
        end
        self.gameRunning = true
        -- 恢复游戏事件
        self:resume()
        cc.SimpleAudioEngine:getInstance():resumeAllEffects()
        if self._targetsToResume ~= nil then
            self:getActionManager():resumeTargets(self._targetsToResume)
        end
        self:resumeGameEvent()
    end)
end

-- 场景进入过场动画结束事件
function ExLesson:_enterTransitionFinish()
    -- 获取课件所在目录
    self.rootPath = self:getRootPath("game.lua")
    print("self.rootPath = " .. self.rootPath)
    -- 将课件所在目录及子目录添加到cocos搜索目录
    self:addSearchPath(self.rootPath)
    -- 播放背景音乐
    cc.SimpleAudioEngine:getInstance():playMusic(self.bgmFile, true)
end

function ExLesson:enterTransitionFinish()

end

-- 场景退出过场动画开始事件
function ExLesson:exitTransitionStart()

end

function ExLesson:_exitTransitionStart()
    -- 重置搜索路径
    self:resetSearchPath()
end

-- 游戏暂停事件
function ExLesson:pauseGameEvent()

end

-- 游戏继续事件
function ExLesson:resumeGameEvent()

end

-- 展示引导动画
function ExLesson:showHelpAni(target, callback, animationFile, osFile, delayTime, position)
    self:setSongEnabled(false)
    if position == nil then
        position = cc.p(0, 0)
    end
    -- 层
    local layerYD = cc.Layer:create()
    target:addChild(layerYD, 28)
    -- 屏蔽点击
    local ls = cc.EventListenerTouchOneByOne:create()
    ls:setSwallowTouches(true)
    ls:registerScriptHandler(function(touch, event) return true end, cc.Handler.EVENT_TOUCH_BEGAN)
    ls:registerScriptHandler(function(touch, event) end, cc.Handler.EVENT_TOUCH_MOVED)
    ls:registerScriptHandler(function(touch, event) end, cc.Handler.EVENT_TOUCH_ENDED)
    target:getEventDispatcher():addEventListenerWithSceneGraphPriority(ls, layerYD)
    -- -- 背景
    self:addSprite("UETest/Public/black.png", layerYD, 512, 384, 0)
    self:addSprite("UETest/Public/yindao_2.png", layerYD, 517, 378, 0)
    -- 动画
    --循环播放引导动画和os
    local ani 
    -- local eff 
    local ac = cc.RepeatForever:create(cc.Sequence:create(cc.CallFunc:create(function (sender)
            if ani==nil then
                ani = self:addArmature(animationFile, 0, layerYD, position.x, position.y, 1)
                ani:getAnimation():playWithIndex(0,-1,0)     
            else
                ani:getAnimation():playWithIndex(0,-1,0)
            end            
            cc.SimpleAudioEngine:getInstance():playEffect(osFile)
        end),cc.DelayTime:create(delayTime)))
    --这个时间用动画／声音中时间最长的时间
    target:runAction(ac)

    -- 边框
    self:addSprite("UETest/Public/yindao_1.png", layerYD, 517, 378, 2)
    -- 关闭按钮（点了关闭按钮要执行的开始游戏）
    local menu = self:addMenuImage("UETest/Lesson/OK_1.png", "UETest/Lesson/OK_2.png", function(sender)
            layerYD:removeFromParent()         
            self:stopAction(ac)         
            cc.SimpleAudioEngine:getInstance():stopAllEffects()
            self:setSongEnabled(true)
            if callback ~= nil then
                callback()
            end
    end, layerYD, 804, 208, 3) 
end

function ExLesson:showNewPassLayer(index)
    if self.gameOver then
        return
    end
    self.gameOver = true
    self.gameRunning = false
    local willReplaceScene = false
    if index == nil then
        index = 0
    end
    -- 显示成功页面  0成功 1一星 2两星 3三星 4失败
    local menuArr = self:showPassLayer(index)
    -- 重新注册按钮点击事件
    menuArr[2]:addTouchEventListener(function(sender, eventType)
        if eventType == ccui.TouchEventType.ended then
            -- 重新注册重新开始按钮点击事件
            self:playMenuEffect()
            if willReplaceScene then
                return
            end
            willReplaceScene = true
            self:runAction(cc.Sequence:create(cc.DelayTime:create(0.2),cc.CallFunc:create(function()
                self:reStartGameEvent()
            end)))
        end
    end)
    menuArr[3]:addTouchEventListener(function(sender, eventType)
        if eventType == ccui.TouchEventType.ended then
            -- 重新注册下一关按钮点击事件
            self:playMenuEffect()
            if willReplaceScene then
                return
            end
            willReplaceScene = true
            self:runAction(cc.Sequence:create(cc.DelayTime:create(0.2),cc.CallFunc:create(function()
                self:exsitGameEvent()
            end)))
        end
    end)
end

function ExLesson:showNewFailLayer()
    if self.gameOver then
        return
    end
    self.gameOver = true

    self.gameRunning = false

    local willReplaceScene = false
    -- 显示失败页面
    local menuArr = self:showFailLayer()
    -- 重新注册按钮点击事件
    menuArr[2]:addTouchEventListener(function(sender, eventType)
        if eventType == ccui.TouchEventType.ended then
            -- 重新注册重新开始按钮点击事件
            self:playMenuEffect()
            if willReplaceScene then
                return
            end
            willReplaceScene = true
            self:runAction(cc.Sequence:create(cc.DelayTime:create(0.2),cc.CallFunc:create(function()
                self:reStartGameEvent()
            end)))
        end
    end)
    menuArr[3]:addTouchEventListener(function(sender, eventType)
        if eventType == ccui.TouchEventType.ended then
            -- 重新注册下一关按钮点击事件
            self:playMenuEffect()
            if willReplaceScene then
                return
            end
            willReplaceScene = true
            self:runAction(cc.Sequence:create(cc.DelayTime:create(0.2),cc.CallFunc:create(function()
                self:exsitGameEvent()
            end)))
        end
    end)
end

-- 游戏结束重完事件
function ExLesson:reStartGameEvent()
    cc.Director:getInstance():replaceScene(self:createScene())
end

-- 游戏结束退出事件
function ExLesson:exsitGameEvent()
    self:exitLessonEvent()
end

-- 敬请期待
function ExLesson:showWaitNewGame()
    self:showLevelMessage("敬请期待")
    -- TODO: 播放敬请期待音效
    self:playEffect("")
end

-- 显示窗口提示
function ExLesson:showLevelMessage(message)
    -- 层
    local layerYD = cc.Layer:create()
    self:addChild(layerYD, 28)
    -- 屏蔽点击
    local ls = cc.EventListenerTouchOneByOne:create()
    ls:setSwallowTouches(true)
    ls:registerScriptHandler(function(touch, event) return true end, cc.Handler.EVENT_TOUCH_BEGAN)
    ls:registerScriptHandler(function(touch, event) end, cc.Handler.EVENT_TOUCH_MOVED)
    ls:registerScriptHandler(function(touch, event) end, cc.Handler.EVENT_TOUCH_ENDED)
    self:getEventDispatcher():addEventListenerWithSceneGraphPriority(ls, layerYD)
    -- -- 背景
    self:addSprite("UETest/Public/black.png", layerYD, 512, 384, 0)
    self:addSprite("UETest/Public/yindao_2.png", layerYD, 517, 378, 0)
    --文字
    local testlabel = cc.LabelTTF:create(message, "Arial", 60)  
    testlabel:setPosition(cc.p(512,384))
    layerYD:addChild(testlabel, 3)
    -- -- 边框
    self:addSprite("UETest/Public/yindao_1.png", layerYD, 517, 378, 2)
    -- -- 关闭按钮（点了关闭按钮要执行的开始游戏）
    local menu = self:addMenuImage("UETest/Lesson/OK_1.png", "UETest/Lesson/OK_2.png", function(sender)
            layerYD:removeFromParent()         
            -- 游戏开始         
            self.gameRunning = true                 
            cc.SimpleAudioEngine:getInstance():stopAllEffects()         
    end, layerYD, 804, 208, 3)     
end


-- 获取从b到e的n个随机数，排除ex中的元素
function ExLesson:getrandom(b, e, n, ex)
    local ret = {}
    local src = {}
    for i = 0, e - b do
        src[i + 1] = b + i
    end
    if ex == nil then
        ex = {}
    end
    -- 排除随机元素
    for i=1, #ex do
        for j=1,#src do
            if ex[i] == src[j] then
                table.remove(src, j)
            end
        end
    end

    for i=1,n do
        if #src == 0 then
            break
        end
        local idx = math.random(1,#src)
        ret[#ret + 1] = src[idx]
        table.remove(src, idx)
    end

    return ret
end

-- 数组元素重新随机
function ExLesson:arrRandom(arr)
    local newArr = {}
    if arr ~= nil then
        local temp = {}
        for i=1,#arr do
            temp[i] = arr[i]
        end
        local num = #temp
        for i=1,num do
            local r = math.random(1, #temp)
            newArr[i] = temp[r]
            table.remove(temp, r)
        end
    end
    return newArr
end

-- 播放音效
function ExLesson:playEffect(file,temp)
    if temp==nil then
        temp=false
    end
    return cc.SimpleAudioEngine:getInstance():playEffect(file,temp)
end
function ExLesson:stopEffect(target)
    --
    if target then
        cc.SimpleAudioEngine:getInstance():stopEffect(target)
    end
end

-- 按钮通用点击音效
function ExLesson:playMenuEffect()
    return self:playEffect("eff/Menu.mp3")
end

function ExLesson:addSprite(name, target, x, y, z)
    local sp = cc.Sprite:create(name)
    if sp ~= nil then
        sp:setPosition(cc.p(x, y))
        target:addChild(sp, z)
    end
    return sp
end

function ExLesson:addArmature(name, idx, target, x, y, z)
    local file = name .. ".ExportJson"
    if not cc.FileUtils:getInstance():isFileExist(file) then
        print("armture " .. name .. " not exsit")
        return nil
    end
    ccs.ArmatureDataManager:getInstance():removeArmatureFileInfo(file)
    ccs.ArmatureDataManager:getInstance():addArmatureFileInfo(file)
    local ani = ccs.Armature:create(name)
    ani:setPosition(cc.p(x, y))
    target:addChild(ani, z)
    ani:getAnimation():playWithIndex(idx)
    return ani
end

function ExLesson:addMenuImage(normal, selected, fun, target, x, y, z)
    local item = cc.MenuItemImage:create(normal, selected)
    item:registerScriptTapHandler(function(sender)
        self:playMenuEffect()
        fun(sender)
    end)
    item:setPosition(cc.p(x,y))
    local menu = cc.Menu:create(item)
    menu:setPosition(cc.p(0,0))
    target:addChild(menu, z)
    return menu
end

-- 键值读写
function ExLesson:readGameDataKey(key, default)
    local data = self:readGameData()
    return data[key] or default
end

function ExLesson:writeGameDataKey(key, value)
    local data = self:readGameData()
    data[key] = value
    self:writeGameData(data)
end

-- 文件读写
function ExLesson:readGameData()
    local data = cc.FileUtils:getInstance():getValueMapFromFile(self.gameDataFile)
    if data == nil then
        data = {}
    end
    return data
end

function ExLesson:writeGameData(data)
    cc.FileUtils:getInstance():writeValueMapToFile(data, self.gameDataFile)
end

-- 暂停恢复事件
function ExLesson:addPauseResumeEvent(target, pauseCb, resumeCb)
    local notifications = {
        "Lesson_Notification_PauseMenuEvent",
        "Lesson_Notification_ResumeMenuEvent",
        "Lesson_Notification_SongPauseEvent",
        "Lesson_Notification_SongResumeEvent"
    }

    local listeners = {}
    local dispatcher = cc.Director:getInstance():getEventDispatcher()
    local isPause = {false, false}
    local layer = cc.Layer:create()
    target:addChild(layer)

    local enterAddListener = function()
        for i,notification in ipairs(notifications) do
            local listener = cc.EventListenerCustom:create(notification, function()
                local os = isPause[1] or isPause[2]
                if i == 1 then
                    isPause[1] = true
                elseif i == 2 then
                    isPause[1] = false
                elseif i == 3 then
                    isPause[2] = true
                elseif i == 4 then
                    isPause[2] = false
                end
                local ns = isPause[1] or isPause[2]
                if os ~= ns then
                    -- 状态改变
                    if ns then
                        -- 暂停
                        if pauseCb ~= nil then
                            pauseCb()
                        end
                    else
                        -- 恢复
                        if resumeCb ~= nil then
                            resumeCb()
                        end
                    end
                end
            end)
            listeners[#listeners + 1] = listener
            dispatcher:addEventListenerWithFixedPriority(listener, 1)
        end
    end

    local temp = self.isEnterTransitionFinish
    if temp then
        enterAddListener()
    end

    layer:registerScriptHandler(function(event)
        -- event
        print("event"..event)
        if event == "enterTransitionFinish" then
            if not temp then
                enterAddListener()
            end
        elseif event == "exitTransitionStart" then
            for i,listener in ipairs(listeners) do
                dispatcher:removeEventListener(listener)
            end
        end
    end)
    return layer
end

-- 两个Point或者Node之间的角度计算 
function ExLesson:pointAngle( startPoint, endPoint )
    if type(startPoint) == "userdata" then
        startPoint = cc.p(startPoint:getPosition())
    end
    if type(endPoint) == "userdata" then
        endPoint = cc.p(endPoint:getPosition())
    end
    local angle = cc.pToAngleSelf(cc.pSub(endPoint, startPoint))
    local ro = angle / 3.1415926 * 180
    return ro
end

-- 从数组中移除元素
function ExLesson:tableRemove(array, item)
    for i,v in ipairs(array) do
        if v == item then
            table.remove(array, i)
            break
        end
    end
end











-- 功能模块

-- 点击参数筛选
function ExLesson:touchListenerArgsSort( ... )
    -- 参数
    local layer, began, moved, ended, cancelled, swallow
    -- 参数分类筛选
    local funs = {}
    for k,v in ipairs({...}) do
        if type(v) == "table" or type(v) == "userdata" then
            layer = v
        elseif type(v) == "boolean" then
            swallow = v
        elseif type(v) == "function" then
            funs[#funs + 1] = v
        end
    end

    -- 默认点击事件
    local emptyB = function(t, e)
        return true
    end
    local empty = function(t, e)
    end
    
    -- 参数默认值
    layer = layer or self
    began = funs[1] or emptyB
    moved = funs[2] or empty
    ended = funs[3] or empty
    cancelled = funs[4] or ended
    if type(swallow) ~= "boolean" then
        swallow = true
    end
    return layer, began, moved, ended, cancelled, swallow
end

-- 添加点击事件
function ExLesson:addTouchListener( ... )
    -- 参数
    local layer, began, moved, ended, cancelled, swallow = self:touchListenerArgsSort(...)
    -- 点击事件
    local ls = cc.EventListenerTouchOneByOne:create()
    ls:setSwallowTouches(swallow)
    ls:registerScriptHandler(function(touch, event)
        local ret = began(touch, event, ls)
        if type(ret) ~= "boolean" then
            ret = true
        end
        return ret 
    end, cc.Handler.EVENT_TOUCH_BEGAN)
    ls:registerScriptHandler(function(touch, event) 
        moved(touch, event, ls)
    end, cc.Handler.EVENT_TOUCH_MOVED)
    ls:registerScriptHandler(function(touch, event) 
        ended(touch, event, ls)
    end, cc.Handler.EVENT_TOUCH_ENDED)
    ls:registerScriptHandler(function(touch, event) 
        cancelled(touch, event, ls)
    end, cc.Handler.EVENT_TOUCH_CANCELLED)
    self:getEventDispatcher():addEventListenerWithSceneGraphPriority(ls, layer)
    -- 点击到精灵数组中的精灵
    ls.spriteTouchInRect = function(sprites, touch)
        local ret = nil
        if type(sprites) == "userdata" then
            sprites = {sprites}
        end
        local wp = touch:getLocation()
        for k,v in ipairs(sprites) do
            local p = v:getParent():convertToNodeSpace(wp)
            if cc.rectContainsPoint(v:getBoundingBox(), p) then
                ret = v
                break
            end
        end
        return ret
    end
    -- 拖动精灵跟随移动
    ls.spriteFollow = function(sprite, touch)
        if sprite ~= nil then
            local dt = touch:getDelta()
            local np = cc.pAdd(cc.p(sprite:getPosition()), dt)
            sprite:setPosition(np)
        end
    end
    -- 记录拖动精灵的位置
    ls.spriteRecord = function(sprite)
        if sprite ~= nil then
            sprite.bp = cc.p(sprite:getPosition())
        end
    end
    -- 重置拖动精灵的位置
    ls.spriteReset = function(sprite)
        if sprite ~= nil and sprite.bp ~= nil then
            -- 回到原位
            sprite:setPosition(sprite.bp)
        end
    end
    return ls
end

-- demo
-- local layer = cc.Layer:create()
-- self:addChild(layer, 29)
-- self:addTouchListener(layer, function(t, e)
--     -- began
--     print("b")
--     return true
-- end, function(t, e)
--     print("m")
-- end, function(t, e)
--     print("e")
-- end, true)

-- 顺序延迟执行事件
function ExLesson:delayDispatch (...)
    local target = ...
    if type(target) == "number" or type(target) == "function" then
        target = self
    end
    local actions = {}
    for k,v in ipairs({...}) do
        local action = nil
        if type(v) == "number" then
            action = cc.DelayTime:create(v) 
        elseif type(v) == "function" then
            action = cc.CallFunc:create(v)
        end
        if action ~= nil then
            actions[#actions + 1] = action
        end
    end
    target:runAction(cc.Sequence:create(actions))
end

-- demo
-- 使用指定目标执行动作
-- local node = cc.Node:create()
-- self:addChild(node)
-- self:delayDispatch(node, 
--     1, function() print("1") end,
--     1, function() print("2") node:stopAllActions() end,
--     1, function() print("3") end)


-- 添加路径并跟随移动
function ExLesson:initFollowLineMove(pointDis, pointMaxCount, onCheckPoint, onAddPoint, onRemovePoint, followNode, followDt, onFollowBegin, onFollowEnd)
    -- 游戏状态
    local gamePause = false
    -- 层
    local layer = cc.Layer:create()
    -- 路径点数组
    layer.pointArr = {}
    -- 路径点最大数量
    layer.pointMaxCount = pointMaxCount
    -- 路径点之间间距
    layer.pointDis = pointDis
    -- 点是否有效回调
    layer.onCheckPoint = onCheckPoint
    -- 添加点回调
    layer.onAddPoint = onAddPoint
    -- 移除点回调
    layer.onRemovePoint = onRemovePoint
    -- 移动状态
    layer.isMoving = false
    -- 跟随移动对象
    layer.followNode = followNode
    -- 跟随移动两个点之前的间隔时间
    layer.followDt = followDt
    -- 开始跟随移动回调
    layer.onFollowBegin = onFollowBegin
    -- 结束跟随移动回调
    layer.onFollowEnd = onFollowEnd
    -- 跟随点移动
    layer.followMove = function()
        if gamePause then
            return
        end
        local node = layer.followNode
        local dt = layer.followDt
        if #layer.pointArr == 0 then
            print("point array is empty")
            return
        end
        layer.isMoving = true
        local actions = {}
        actions[#actions + 1] = cc.CallFunc:create(function()
            layer.onFollowBegin()
        end)
        for i=1,#layer.pointArr do
            local p = layer.pointArr[i]
            if i == 1 then
                node:setPosition(p)
                layer.onRemovePoint(p, i)
            else
                local move = cc.MoveTo:create(dt, p)
                local event = cc.CallFunc:create(function()
                    layer.onRemovePoint(p, i)
                end)
                actions[#actions + 1] = move
                actions[#actions + 1] = event
            end
        end
        actions[#actions + 1] = cc.CallFunc:create(function()
            layer.pointArr = {}
            layer.isMoving = false
            layer.onFollowEnd()
        end)
        node:runAction(cc.Sequence:create(actions))
    end
    local function addPoint2Arr(p)
        if gamePause then
            return false
        end
        local ret = false
        local function addIfRight(point)
            -- 验证点有效
            if #layer.pointArr < layer.pointMaxCount and layer.onCheckPoint(point) then
                layer.pointArr[#layer.pointArr + 1] = point
                layer.onAddPoint(point, #layer.pointArr)
                ret = true
            end
        end
        if #layer.pointArr == 0 then
            addIfRight(p)
        else
            -- 验证距离
            local lp = layer.pointArr[#layer.pointArr]
            local dis = cc.pGetDistance(lp, p)
            if dis >= layer.pointDis and dis < layer.pointDis * 1.2 then
                addIfRight(p)
            end
        end
        return ret
    end
    
    local ls = cc.EventListenerTouchOneByOne:create()
    ls:setSwallowTouches(false)
    ls:registerScriptHandler(function(touch, event) 
        -- 正在移动判断
        if layer.isMoving then
            return false
        end
        -- 点击点
        local p = layer:convertToNodeSpace(touch:getLocation())
        -- 点击在移动节点上判断
        local node = layer.followNode
        local np = node:getParent():convertToNodeSpace(touch:getLocation())
        if cc.rectContainsPoint(node:getBoundingBox(), np) == false then
            return
        end
        -- 添加点结果
        local ret = addPoint2Arr(p)
        return ret
    end, cc.Handler.EVENT_TOUCH_BEGAN)
    local function touchEvent(touch, event)
        -- local start = layer:convertToNodeSpace(touch:getPreviousLocation()) 
        local start = layer.pointArr[#layer.pointArr]
        local ended = layer:convertToNodeSpace(touch:getLocation())
        local distance = cc.pGetDistance(start, ended)
        if distance > 1 then
            local brushes = {}
            local d = distance
            local i = 0
            for i = 0, d - 1 do
                local difx = ended.x - start.x
                local dify = ended.y - start.y
                local delta = i / distance
                local tempP = cc.p(start.x + (difx * delta), start.y + (dify * delta))
                addPoint2Arr(tempP)
            end
        end
    end
    ls:registerScriptHandler(touchEvent, cc.Handler.EVENT_TOUCH_MOVED)
    ls:registerScriptHandler(function(touch, event)
        touchEvent(touch, event)
        -- 开始移动
        layer.followMove()
    end, cc.Handler.EVENT_TOUCH_ENDED)
    self:getEventDispatcher():addEventListenerWithSceneGraphPriority(ls, layer)

    self:addPauseResumeEvent(layer, function()
        gamePause = true
        if layer.isMoving then
            layer.followNode:pause()
        end
    end, function()
        gamePause = false
        if layer.isMoving then
            layer.followNode:resume()
        end
    end)
    self:addChild(layer, 20)
    return layer, layer.followMove
end

-- -- 暂停恢复事件
-- function ExLesson:addPauseResumeEvent(target, pauseCb, resumeCb)
--     local notifications = {
--         "Lesson_Notification_PauseMenuEvent",
--         "Lesson_Notification_ResumeMenuEvent",
--         "Lesson_Notification_SongPauseEvent",
--         "Lesson_Notification_SongResumeEvent"
--     }
--     local listeners = {}
--     local dispatcher = cc.Director:getInstance():getEventDispatcher()
--     local isPause = {false, false}
--     local layer = cc.Layer:create()
--     target:addChild(layer)
--     layer:registerScriptHandler(function(event)
--         -- event
--         if event == "enterTransitionFinish" then
--             for i,notification in ipairs(notifications) do
--                 local listener = cc.EventListenerCustom:create(notification, function()
--                     local os = isPause[1] or isPause[2]
--                     if i == 1 then
--                         isPause[1] = true
--                     elseif i == 2 then
--                         isPause[1] = false
--                     elseif i == 3 then
--                         isPause[2] = true
--                     elseif i == 4 then
--                         isPause[2] = false
--                     end
--                     local ns = isPause[1] or isPause[2]
--                     if os ~= ns then
--                         -- 状态改变
--                         if ns then
--                             -- 暂停
--                             if pauseCb ~= nil then
--                                 pauseCb()
--                             end
--                         else
--                             -- 恢复
--                             if resumeCb ~= nil then
--                                 resumeCb()
--                             end
--                         end
--                     end
--                 end)
--                 listeners[#listeners + 1] = listener
--                 dispatcher:addEventListenerWithFixedPriority(listener, 1)
--             end
--         elseif event == "exitTransitionStart" then
--             for i,listener in ipairs(listeners) do
--                 dispatcher:removeEventListener(listener)
--             end
--         end
--     end)
--     return layer
-- end

-- demo
-- local moveSp = cc.Sprite:create("DMM_orange.png")
-- if moveSp ~= nil then
--     moveSp:setPosition(cc.p(512,384))
--     self:addChild(moveSp, 20)
-- end
-- local spArr = {}
-- local layer, moveFun = self:initFollowLineMove(50.0, 200, function(p)
--     -- 返回是否为有效的点
--     if p.x > 512 then
--         return false
--     end
--     return true
-- end, function(point, idx)
--     -- 添加路径点
--     -- print("p = " .. point.x .. "," .. point.y)
--     local sp = cc.Sprite:create("DMM_orange.png")
--     if sp ~= nil then
--         sp:setPosition(point)
--         self:addChild(sp, 10)
--         spArr[idx] = sp
--     end
-- end, function(point, idx)
--     -- 移除路径点
--     spArr[idx]:removeFromParent()
-- end, moveSp, 0.5, function()
--     -- 开始移动
--     print("move begin")
-- end, function()
--     -- 结束移动
--     print("move end")
-- end)


-- 初始化进度条
function ExLesson:initProgressUI(target, x, y, z, bgImage, proImage, fgImage, blinkImage, moveImage, blinkVoice, maxTime, blinkTime, blinkCb, endCb, direction)
    -- 进度条方向 0:倒计时 1:计时
    direction = direction or 0
    -- 开始进度和结束进度
    local startPercent = 100
    local endPercent = 0
    if direction == 1 then
        startPercent = 0
        endPercent = 100
    end
    -- 容器层
    local layer = cc.Layer:create()
    layer:setPosition(x, y)
    -- 背景
    local bgSp = ExLesson:addSprite(bgImage, layer, 0, 0, 0)
    -- 进度
    local progress = cc.ProgressTimer:create(cc.Sprite:create(proImage))
    progress:setType(cc.PROGRESS_TIMER_TYPE_BAR)
    progress:setMidpoint(cc.p(0, 1))
    progress:setBarChangeRate(cc.p(1, 0))
    progress:setPosition(cc.p(0, 0))

    progress:setPercentage(startPercent)
    layer:addChild(progress,1)
    -- 前景
    local fgSp = ExLesson:addSprite(fgImage, layer, 0, 0, 2)
    -- 闪烁动画
    local blinkSp = ExLesson:addSprite(blinkImage, layer, 0, 0, 3)
    blinkSp.showTime, blinkSp.hideTime = 0.5, 0.2
    if blinkSp ~= nil then
        blinkSp:setVisible(false)
        -- 设置闪烁间隔时间方法
        blinkSp.setBlinkTime = function(showTime, hideTime)
            blinkSp.showTime, blinkSp.hideTime = showTime, hideTime
        end
    end
    -- 跟随进度移动精灵
    local moveSp = ExLesson:addSprite(moveImage, layer, 0, 0, 4)
    local pRect = progress:getBoundingBox()
    local moveSpUpdate = function(dt)
        local np = cc.p(pRect.x + pRect.width * progress:getPercentage() / 100.0, pRect.y + pRect.height * 0.5)
        moveSp:setPosition(np)
    end
    local schId = nil
    moveSpUpdate(0)
    if moveSp ~= nil then
        layer:registerScriptHandler(function(event)
            -- event
            local scheduler = cc.Director:getInstance():getScheduler()
            if event == "enter" then
                schId = scheduler:scheduleScriptFunc(moveSpUpdate, 0, false)
            elseif event == "exit" then
                if schId ~= nil then
                    scheduler:unscheduleScriptEntry(schId)
                end
            end
        end)
    end
    target:addChild(layer, z)
    -- 开始
    layer.startProgress = function()
        if blinkSp ~= nil and blinkTime > 0 and maxTime > blinkTime then
            -- 存在闪烁动画
            local percent = 100.0 * blinkTime / maxTime
            if direction == 1 then
                percent = 100.0 * (maxTime - blinkTime) / maxTime
            end
            progress:runAction(cc.Sequence:create(cc.ProgressTo:create(maxTime - blinkTime, percent), cc.CallFunc:create(function()
                -- 开始闪烁
                blinkSp:setVisible(true)
                if blinkCb ~= nil then
                    blinkCb()
                end
                -- 循环闪烁动画
                blinkSp:runAction(cc.RepeatForever:create(cc.Sequence:create(
                    cc.Show:create(), cc.CallFunc:create(function()
                        if blinkVoice ~= nil then
                            cc.SimpleAudioEngine:getInstance():playEffect(blinkVoice)
                        end
                    end), cc.DelayTime:create(blinkSp.showTime),
                    cc.Hide:create(), cc.DelayTime:create(blinkSp.hideTime))))
            end), cc.ProgressTo:create(blinkTime, endPercent), cc.CallFunc:create(function()
                -- 进度结束
                if endCb ~= nil then
                    endCb()
                end
                -- 停止闪烁
                blinkSp:stopAllActions()
                blinkSp:setVisible(false)
            end)))
        else
            -- 不存在闪烁动画
            progress:runAction(cc.Sequence:create(cc.ProgressTo:create(maxTime, endPercent), cc.CallFunc:create(function()
                -- 进度结束
                if endCb ~= nil then
                    endCb()
                end
            end)))
        end
    end
    -- 暂停
    layer.pauseProgress = function()
        progress:pause()
        if blinkSp ~= nil then 
            blinkSp:pause()
        end
    end
    -- 恢复
    layer.resumeProgress = function()
        progress:resume()
        if blinkSp ~= nil then 
            blinkSp:resume()
        end
    end

    -- 暂停恢复事件
    self:addPauseResumeEvent(layer, function()
        layer.pauseProgress()
    end, function()
        layer.resumeProgress()
    end)

    return layer, progress, bgSp, fgSp, blinkSp, moveSp
end

-- demo
-- -- 初始化进度
-- local layer, progress, bgSp, fgSp, blinkSp, moveSp = GameScene:initProgressUI(self, 512, 384, 100, "01_UI/time_2.png", "01_UI/time_1.png", "01_UI/time_3.png", "01_UI/time_4.png", "touchback.png", "", 6, 1, function()
--     print("blink")
-- end, function()
--     print("over")
-- end, 0)
-- -- UI微调
-- bgSp:setPosition(cc.p(0, -5))
-- -- 开始进度
-- layer:startProgress()
-- -- 进度暂停恢复测试
-- self:runAction(cc.Sequence:create(cc.DelayTime:create(4), cc.CallFunc:create(function()
--     layer:pauseProgress()
-- end), cc.DelayTime:create(4), cc.CallFunc:create(function()
--     layer:resumeProgress()
-- end)))



return ExLesson
