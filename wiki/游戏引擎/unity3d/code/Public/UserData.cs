using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据
/// </summary>
public class UserData : Config {
	static UserData __userData = null;
	public static UserData share {
		get {
			if (__userData == null) {
				__userData = Read<UserData>("userData.json");	
			}
			return __userData;
		}
	}
	/// <summary>
	/// 用户名称
	/// </summary>
	public string name;
	/// <summary>
	/// 用户头像序号
	/// </summary>
	public int headIcon;
}
