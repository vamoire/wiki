using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 应用设置信息
/// </summary>
public class AppConfig : Config {

	static AppConfig __appConfig = null;
	public static AppConfig share {
		get {
			if (__appConfig == null) {
				__appConfig = Read<AppConfig>("appConfig.json");	
			}
			return __appConfig;
		}
	}

	/// <summary>
	/// 语言枚举
	/// </summary>
	public enum Language {
		CN = 0,
		EN
	}

	/// <summary>
	/// 当前语言
	/// </summary>
	public Language language = Language.EN;

	/// <summary>
	/// 游戏背景音乐
	/// </summary>
	public bool music = true;
	/// <summary>
	/// 游戏音效
	/// </summary>
	public bool sound = true;

}
