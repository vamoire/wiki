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
	/// 更新游戏最佳纪录
	/// </summary>
	/// <param name="mapType">地图类型</param>
	/// <param name="time">最佳纪录</param>
	public void UpdateBestTime(Game.MapType mapType, float time) {
		//bestFunc
		System.Func<float, float, float> bestFunc = (float old, float value)=>{
			float ret = old;
			if ((value > 0) && (old == 0 || value < old)) {
				ret = value;
			}
			return ret;
		};
		//update
		if (mapType == Game.MapType.M100) {
			gameData.bestTimeM100 = bestFunc(gameData.bestTimeM100, time);
		}
		else if (mapType == Game.MapType.M400) {
			gameData.bestTimeM400 = bestFunc(gameData.bestTimeM400, time);
		}
		else if (mapType == Game.MapType.M800) {
			gameData.bestTimeM800 = bestFunc(gameData.bestTimeM800, time);
		}
		//flush
		Flush();
	}

	/// <summary>
	/// 获得最佳纪录
	/// </summary>
	/// <param name="mapType">地图类型</param>
	/// <returns></returns>
	public float GetBestTime(Game.MapType mapType) {
		float value = 0;
		if (mapType == Game.MapType.M100) {
			value = gameData.bestTimeM100;
		}
		else if (mapType == Game.MapType.M400) {
			value = gameData.bestTimeM400;
		}
		else if (mapType == Game.MapType.M800) {
			value = gameData.bestTimeM800;
		}
		return value;
	}

	/// <summary>
	/// 用户名称
	/// </summary>
	public string name;
	/// <summary>
	/// 用户头像序号
	/// </summary>
	public int headIcon;
	/// <summary>
	/// 游戏数据
	/// </summary>
	public GameData gameData;


	/// <summary>
	/// 用户游戏数据
	/// </summary>
	public class GameData {
		/// <summary>
		/// 100米最佳纪录
		/// </summary>
		public float bestTimeM100;
		/// <summary>
		/// 400米最佳纪录
		/// </summary>
		public float bestTimeM400;
		/// <summary>
		/// 800米最佳纪录
		/// </summary>
		public float bestTimeM800;

	}
	
}
