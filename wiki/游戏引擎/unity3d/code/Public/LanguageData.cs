using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多语言数据管理
/// </summary>
public class LanguageData {
	//instance
	private static LanguageData __languageData = null;
	/// <summary>
	/// 多语言单例
	/// </summary>
	/// <returns>多语言单例</returns>
	public static LanguageData share{
		get{
			if (__languageData == null) {
				__languageData = new LanguageData();
			}
			return __languageData;
		}
	}

	/// <summary>
	/// 获取当前多语言值
	/// </summary>
	/// <param name="key">多语言键</param>
	/// <returns>多语言值</returns>
	public static string Key(string key) {
		return LanguageData.share.GetKey(key);
	}

	public LanguageData() {
		LoadData();
	}

	public void LoadData() {
		//加载配置文件
		TextAsset asset = Resources.Load("language", typeof(TextAsset)) as TextAsset;
		string[] lineData = asset.text.Split(new char[]{'\n'});
		data = new string[lineData.Length][];
		for (int i = 0; i < lineData.Length; i++)
		{
			data[i] = lineData[i].Split(new char[]{','});
		}
	}


	/// <summary>
	/// 获取多语言Key对应的当前语言文字
	/// </summary>
	/// <param name="name">多语言Key</param>
	/// <returns>当前语言文字</returns>
	public string GetKey(string name) {
		string value = name;
		int idx = (int)AppConfig.share.language + 1;
		for(int i = 0; i < data.Length && name.Length > 0; i++) {
			//key
			string key = data[i][0];
			if (key == name) {
				//find
				value = data[i][idx];
				break;
			}
		}
		return value;
	}

	//多语言数据
	string[][]data;
}