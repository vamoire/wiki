-- cc.FileUtils:getInstance():addSearchPath("src/")
-- local loadLessonEx = require "loadLessonEx"
local loadLessonEx = function(file)
	cc.FileUtils:getInstance():setPopupNotify(false)
	cc.FileUtils:getInstance():addSearchPath("src/")
	cc.FileUtils:getInstance():addSearchPath("res/")
	require "config"
	require "cocos.init"
	local tempLessonLayer = cc.LessonLayer:create()
	local rootPath = tempLessonLayer:getRootPath(file)
	print("root = " .. rootPath)
	tempLessonLayer:resetSearchPath()
	tempLessonLayer:addSearchPath(rootPath)
	local LessonEx = dofile(rootPath .. file)
	tempLessonLayer:resetSearchPath()
	return LessonEx
end

local LessonEx = loadLessonEx("LessonEx.lua")
local GameScene = class("GameScene", LessonEx)

local levelnum
local entryID
local istouch
local osos

function GameScene:ctor( ... )
    LessonEx.ctor(self)

    -- 背景音乐文件
    self.bgmFile = "Music/LessonBGM/music_03_explore.mp3"

    levelnum = 1
    entryID = 0
    osos = 0
    istouch = false
end

function GameScene:enterTransitionFinish()

	self:initData()
	-- 初始化游戏
	self:initGame()

	-- 引导动画
    self:showHelpAni(self, function()

    	-- 开始游戏
    	self:show321Go()
    end, "SSSZD_yd_ani", "LBZGL_YD.mp3", 10)
    --
end

function GameScene:exitTransitionStart()

end

function GameScene:initData()
	self.numname = {
		"LBZGL_SZ_1.png","LBZGL_SZ_2.png","LBZGL_SZ_3.png","LBZGL_SZ_4.png","LBZGL_SZ_5.png",
		"LBZGL_SZ_6.png","LBZGL_SZ_7.png","LBZGL_SZ_8.png","LBZGL_SZ_9.png","LBZGL_SZ_10.png"
	}
	self.timearr = {}

	self.wjname = {
		"1/LBZGL_WJ_1.png","2/LBZGL_WJ_1.png","3/LBZGL_WJ_1.png","4/LBZGL_WJ_1.png","5/LBZGL_WJ_1.png","6/LBZGL_WJ_1.png",
		"7/LBZGL_WJ_1.png","8/LBZGL_WJ_1.png","9/LBZGL_WJ_1.png","10/LBZGL_WJ_1.png","11/LBZGL_WJ_1.png","12/LBZGL_WJ_1.png",
		"13/LBZGL_WJ_1.png","14/LBZGL_WJ_1.png","15/LBZGL_WJ_1.png","16/LBZGL_WJ_1.png","17/LBZGL_WJ_1.png","18/LBZGL_WJ_1.png",
		"19/LBZGL_WJ_1.png","20/LBZGL_WJ_1.png","21/LBZGL_WJ_1.png","22/LBZGL_WJ_1.png","23/LBZGL_WJ_1.png","24/LBZGL_WJ_1.png"
	}
	self.wjpos = {
		cc.p(102.15,535.9),cc.p(265.8,535.9),cc.p(433.4,535.9),cc.p(601.05,535.9),cc.p(768,535.9),cc.p(931.4,535.9)
	}
	self.wjani = {}
	self.wjback = {}
	self.tdsp = {}
	self.tdpos = {
		{cc.p(460.35,-154.15),cc.p(570.85,-154.15)},
		{cc.p(402.9,-154.15),cc.p(513.4,-154.15),cc.p(618.95,-154.15)}
	}
end

--初始化游戏
function GameScene:initGame()
	-- 暂停按钮
	-- self:addFramePauseMenu()
	self.bgimgSP = GameScene:addSprite("LBZGL_BJ_1366.png", self, 512, 384, 1)
	self.bgimgSP01 = GameScene:addSprite("LBZGL_BJ_1366.png", self, 512, 384, 0)
	
	local shizhong = GameScene:addSprite("LBZGL_NZ.png", self, 677.35, 702.8, 1)
	
	local yijia = GameScene:addSprite("LBZGL_YJ.png", self, 512, 544.9, 1)
	self.lanbao = GameScene:addArmature("LBZGL_LB_ani", 0, self, 167.05, 187.4, 1)

	self.timearr[1] = GameScene:addSprite(self.numname[4], self, 781.65, 702.8, 1)
	self.timearr[2] = GameScene:addSprite(self.numname[1], self, 850.5, 702.8, 1)
	self.timearr[3] = GameScene:addSprite(self.numname[1], self, 919.35, 702.8, 1)
end

--游戏开始
function GameScene:startGame()
	-- 游戏开始
	self.gameRunning = true

	self.lanbao:getAnimation():playWithIndex(0)
	self.huaban = GameScene:addSprite("LBZGL_HB.png", self, 514.9, -186.45, 1)
	if levelnum>=1 and levelnum<=3 then
		self:setlevelfirst()
	elseif levelnum>=4 and levelnum<=6 then
		self:setlevelsecond()
	elseif levelnum>=7 and levelnum<=9 then
		self:setlevelthird()
	end

	local arr = self.wjani
	local barr = self.wjback
	local r = GameScene:getrandom(1, 6, 1, nil)
	table.insert(arr, arr[r[1]])
	table.remove(arr,r[1])
	table.insert(barr, barr[r[1]])
	table.remove(barr,r[1])
	for j=1,#arr-1 do
		arr[j]:runAction(cc.Sequence:create(cc.DelayTime:create(0.5*(j-1)),cc.CallFunc:create(function ( sender )
			cc.SimpleAudioEngine:getInstance():playEffect("LBZGL_chuxian.mp3")
			arr[j]:setTag(0)
			arr[j]:setVisible(true)
			arr[j]:getAnimation():playWithIndex(1)

			if j==#arr-1 then
				osos = cc.SimpleAudioEngine:getInstance():playEffect("LBZGL_KGL.mp3")
				self.huaban:runAction(cc.Sequence:create(cc.MoveBy:create(0.5, cc.p(0,360.25)),cc.CallFunc:create(function ( sender )
					istouch = true
					self:addTouchMove()
				end)))
				for m=1,#self.tdsp do
					self.tdsp[m]:runAction(cc.MoveBy:create(0.5, cc.p(0,360.25)))
				end
			end
		end)))
	end
		
end

function GameScene:setlevelfirst()
	local rand = GameScene:getrandom(1, 24, 2, nil)
	for i=1,6 do
		self.wjback[i] = GameScene:addSprite("quyu.png", self, self.wjpos[i].x, self.wjpos[i].y, 1)
		self.wjback[i]:setOpacity(0)
		if i%2==0 then
			self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(2)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[2]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
		else
			self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(1)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[1]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
		end
	end

	-- local rr = GameScene:getrandom(1, 6, 2, nil)
	for j=1,2 do
		self.tdsp[j] = GameScene:addSprite(self.wjname[rand[j]], self, self.tdpos[1][j].x, self.tdpos[1][j].y, 2)
		self.tdsp[j]:setTag(j)
		self.tdsp[j]:setScale(0.8)
	end
end

function GameScene:setlevelsecond()
	local rand = GameScene:getrandom(1, 24, 2, nil)
	for i=1,6 do
		self.wjback[i] = GameScene:addSprite("quyu.png", self, self.wjpos[i].x, self.wjpos[i].y, 1)
		self.wjback[i]:setOpacity(0)
		if i==1 or i==4 then
			self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(1)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[1]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
		else
			self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(2)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[2]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
		end
	end
	-- local rr = GameScene:getrandom(1, 6, 2, nil)
	for j=1,2 do
		self.tdsp[j] = GameScene:addSprite(self.wjname[rand[j]], self, self.tdpos[1][j].x, self.tdpos[1][j].y, 2)
		self.tdsp[j]:setTag(j)
		self.tdsp[j]:setScale(0.8)
	end
end

function GameScene:setlevelthird()
	local rand = GameScene:getrandom(1, 24, 3, nil)
	for i=1,6 do
		self.wjback[i] = GameScene:addSprite("quyu.png", self, self.wjpos[i].x, self.wjpos[i].y, 1)
		self.wjback[i]:setOpacity(0)
		if i==1 or i==4 then
			self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(1)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[1]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
        elseif i==2 or i==5 then
        	self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(2)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[2]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
		else
			self.wjani[i] = GameScene:addArmature("LBZGL_WJ_ani", 0, self, self.wjpos[i].x, self.wjpos[i].y, 1)
			self.wjani[i]:setTag(3)
			self.wjani[i]:setVisible(false)
			local sprite = cc.Sprite:create(self.wjname[rand[3]])
            local bone = self.wjani[i]:getBone("LBZGL_WJ_1")
            bone:addDisplay(sprite,0)
		end
	end
	-- local rr = GameScene:getrandom(1, 6, 3, nil)
	for j=1,3 do
		self.tdsp[j] = GameScene:addSprite(self.wjname[rand[j]], self, self.tdpos[2][j].x, self.tdpos[2][j].y, 2)
		self.tdsp[j]:setTag(j)
		self.tdsp[j]:setScale(0.8)
	end
end

function GameScene:setTimeSch()
	local time = 3000
    local isdidi = true
    
    entryID = cc.Director:getInstance():getScheduler():scheduleScriptFunc(function ( sender )
        if self.gameRunning then
            time  = time - 1
            local num = math.modf(time/10)
            if num>0 then
            	self.timearr[1]:setTexture(self.numname[math.modf(num/100)+1])
	            self.timearr[2]:setTexture(self.numname[(math.modf(num/10)%10)+1])
	            self.timearr[3]:setTexture(self.numname[(num%10)+1])
	        else
	        	self.timearr[1]:setTexture(self.numname[1])
	            self.timearr[2]:setTexture(self.numname[1])
	            self.timearr[3]:setTexture(self.numname[1])
	            istouch = false
	            cc.Director:getInstance():getScheduler():unscheduleScriptEntry(entryID)
	            self.bgimgSP01:runAction(cc.Sequence:create(cc.DelayTime:create(1.5),cc.CallFunc:create(function ( sender )
	            	for i=1,#self.wjani do
		            	self.wjani[i]:stopAllActions()
		            	self.wjani[i]:setVisible(false)
		            end
		            self.bgimgSP:stopAllActions()
		            cc.SimpleAudioEngine:getInstance():stopEffect(osos)
	            	if levelnum<=2 then
	            		self:showNewFailLayer()
	            	elseif levelnum>2 and levelnum<=5 then
	            		self:showNewPassLayer(1)
	            	elseif levelnum>5 and levelnum<=9 then
	            		self:showNewPassLayer(2)
	            	elseif levelnum==10 then
	            		self:showNewPassLayer(3)
	            	end
	            end)))
            end
	            

        end
    end,0.1,false)
end

function GameScene:addTouchMove()
	self.newlayer = cc.Layer:create()
    self:addChild(self.newlayer, 1)
    local dragSp
    local dragAni
    local orgx
    local orgy
    local orgz
    -- istouch = true
    local dj = 0
    local listener = cc.EventListenerTouchOneByOne:create()
    listener:registerScriptHandler(function ( touch,event )
        if self.gameRunning==false then
            return false
        end
        local touchpoint = self:convertTouchToNodeSpace(touch)
        for i=1,#self.tdsp do
            dragSp = self.tdsp[i]
            if (cc.rectContainsPoint(dragSp:getBoundingBox(),touchpoint)) and dragSp:isVisible() and istouch then
                istouch = false
                cc.SimpleAudioEngine:getInstance():stopEffect(dj)
                dj = cc.SimpleAudioEngine:getInstance():playEffect("LBZGL_dianji.mp3")
                orgx = dragSp:getPositionX()
                orgy = dragSp:getPositionY()
                orgz = dragSp:getLocalZOrder()
                dragSp:setLocalZOrder(5)
                return true
            end
        end
        return false
    end,cc.Handler.EVENT_TOUCH_BEGAN)
    listener:registerScriptHandler(function ( touch,event )
        if self.gameRunning == false then
            return
        end
        local touchpoint = self:convertTouchToNodeSpace(touch)
        dragSp:setPosition(touchpoint)
    end,cc.Handler.EVENT_TOUCH_MOVED)
    listener:registerScriptHandler(function ( touch,event )
        if self.gameRunning==false then
            return false
        end
        local touchpoint = self:convertTouchToNodeSpace(touch)
        local p = self:convertToNodeSpace(touch:getLocation())
        local istip = true
        for j=1,#self.wjani do
        	if (cc.rectContainsPoint(self.wjback[j]:getBoundingBox(),touchpoint)) then
        		istip = false
        		if self.wjani[j]:getTag()==dragSp:getTag() then
        			self.bgimgSP:runAction(cc.Sequence:create(cc.CallFunc:create(function ( sender )
        				self.wjani[j]:setVisible(true)
	        			self.wjani[j]:getAnimation():playWithIndex(2)
	        			levelnum = levelnum + 1
	        			cc.SimpleAudioEngine:getInstance():playEffect("LBZGL_right.mp3")
	        			local r = GameScene:getrandom(1, 2, 1, nil)
	        			self.lanbao:getAnimation():playWithIndex(r[1])
	        			dragSp:setVisible(false)
	        			dragSp:setPosition(cc.p(orgx,orgy))
        			end),cc.DelayTime:create(0.4),cc.CallFunc:create(function ( sender )
        				for n=1,#self.wjani do
        					self.wjani[n]:getAnimation():playWithIndex(3)
        				end
        			end),cc.DelayTime:create(0.4),cc.CallFunc:create(function ( sender )
        				cc.SimpleAudioEngine:getInstance():playEffect("LBZGL_hua.mp3")
        				for n=1,#self.wjani do
        					self.wjani[n]:getAnimation():playWithIndex(4)
        					self.wjani[n]:runAction(cc.Sequence:create(cc.MoveBy:create(0.3, cc.p(0,-650.05)),cc.DelayTime:create(0.2),cc.CallFunc:create(function ( sender )
        						if n==#self.wjani then
        							if levelnum~=10 then

        								self.wjani[n]:stopAllActions()
	        							for p=1,#self.wjani do
	        								self.wjani[p]:removeFromParent()
	        								self.wjback[p]:removeFromParent()
	        							end
	        							for q=1,#self.tdsp do
	        								self.tdsp[q]:removeFromParent()
	        							end
	        							self.wjani = {}
	        							self.wjback = {}
	        							self.tdsp = {}
	        							self.huaban:removeFromParent()
	        							self.huaban = nil

        								self:startGame()
        							end
        						else
        							self.wjani[n]:stopAllActions()
        						end
        					end),cc.DelayTime:create(0.5),cc.CallFunc:create(function ( sender )
        						self:showNewPassLayer(3)
        					end)))
        				end
        			end)))
        		else
        			cc.SimpleAudioEngine:getInstance():playEffect("LBZGL_wrong.mp3")
        			local r = GameScene:getrandom(3, 4, 1, nil)
        			self.lanbao:getAnimation():playWithIndex(r[1])
        			dragSp:runAction(cc.Sequence:create(cc.EaseIn:create(cc.MoveTo:create(0.5, cc.p(orgx,orgy)), 0.2),cc.CallFunc:create(function ( sender )
        				istouch = true
        				self.lanbao:getAnimation():playWithIndex(0)
        			end)))
        		end
        	end
        end
        if istip then
        	istouch = true
	    	dragSp:setPosition(cc.p(orgx,orgy))
        end
        dragSp:setLocalZOrder(orgz)
    end,cc.Handler.EVENT_TOUCH_ENDED)
    listener:registerScriptHandler(function ( touch,event )

    end,cc.Handler.EVENT_TOUCH_CANCELLED)

    local eventDispatcher = self:getEventDispatcher()
    eventDispatcher:addEventListenerWithSceneGraphPriority(listener, self.newlayer)
end

--321go层
function GameScene:show321Go()
    self:runAction(cc.Sequence:create(cc.CallFunc:create(function ( sender )
        cc.SimpleAudioEngine:getInstance():stopAllEffects()
        cc.SimpleAudioEngine:getInstance():playEffect("OS_321.mp3")
        self.gogo = GameScene:addArmature("UI_ani_jishiqi", 0, self, 0, 0, 11)
    end),cc.DelayTime:create(5),cc.CallFunc:create(function ( sender )
        self.gogo:removeFromParent()
        self.gogo = nil
        self:setSongEnabled(true)
        self:startGame()
        self:setTimeSch()
        local pauseMenu = self:addFramePauseMenu()
    end)))
end

-- 游戏暂停事件
function GameScene:pauseGameEvent()

end

-- 游戏继续事件
function GameScene:resumeGameEvent()

end

--按钮点击音效
function GameScene:playMenuEffect()
	--
	return self:playEffect("eff/Menu.mp3")
end

-- 跳转场景
cc.Director:getInstance():replaceScene(GameScene:createScene())