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

function GameScene:ctor( ... )
    LessonEx.ctor(self)

    -- 背景音乐文件
    self.bgmFile = "bg_music.mp3"

end

function GameScene:enterTransitionFinish()

	-- 初始化游戏
	self:initGame()

	-- 引导动画
    self:showHelpAni(self, function()

    	-- 开始游戏
    	self:startGame()
    end, "BEBS_caozuoyindao_ani", "BEBS_caozuoyindao.mp3", 10)
    --
end

function GameScene:exitTransitionStart()

end

--初始化游戏
function GameScene:initGame()
	-- 暂停按钮
	-- self:addFramePauseMenu()
	
end

--游戏开始
function GameScene:startGame()
	-- 游戏开始
	self.gameRunning = true

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