using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;//获取mac地址

namespace XiaJiaJia {

	public enum MessageCode{
		Unknow = -1,	//未知
		SendMessage = 0,//发送消息
		PlayerStatus = 100,		//角色状态
		PlayerInfo = 200,		//角色信息
		PlayerAttack = 300,		//攻击
	};

	//网络消息数据结构
	public class NetInfo {
		//消息类型
		public MessageCode code = MessageCode.Unknow;
		//发送者
		public string from = "";
		//接收者
		public string to = "";
		//时间
		public string time = "";

		//文字消息
		//消息内容
		public string message = "";

		//角色状态
		//坐标
		public Vector3 position = Vector3.zero;
		//缩放
		public Vector3 localScale = Vector3.one;

		//角色信息
		//生命值
		public int player_life = 0;
		//攻击力
		public int player_attick = 0;
		//防御
		public int player_armor = 0;

		//攻击消息
		//攻击方式
		public int attick_type = 0;
		//伤害
		public int attick_hurt = 0;


		//消息封装中的key
		private static string Key_Code = "code";
		private static string Key_FromID = "from";
		private static string Key_ToID = "to";
		private static string Key_Time = "time";
		private static string Key_Msg = "msg";
		private static string Key_Position = "s_po";
		private static string Key_LocalScale = "s_sa";
		private static string Key_PlayerLife = "p_li";
		private static string Key_PlayerAttick = "p_at";
		private static string Key_PlayerArmor = "p_ar";
		private static string Key_AttickType = "a_ty";
		private static string Key_AttickHurt = "a_hu";


		//空消息判断
		public bool isNull(){
			return code == MessageCode.Unknow || from == "";
		}
		//消息封装
		public string ToNetString(){
			if (isNull ()) {
				return "";
			}
			Dictionary<string, string> dic = new Dictionary<string, string> ();
			dic.Add (Key_Code, ((int)code).ToString());
			dic.Add (Key_FromID, from);
			dic.Add (Key_ToID, to);
			dic.Add (Key_Time, time);
			if (code == MessageCode.SendMessage) {
				dic.Add (Key_Msg, message);
			} else if (code == MessageCode.PlayerStatus) {
				dic.Add (Key_Position, position.ToString ());
				dic.Add (Key_LocalScale, localScale.ToString ());
			} else if (code == MessageCode.PlayerInfo) {
				dic.Add (Key_PlayerLife, player_life.ToString ());
				dic.Add (Key_PlayerAttick, player_attick.ToString ());
				dic.Add (Key_PlayerArmor, player_armor.ToString ());
			} else if (code == MessageCode.PlayerAttack) {
				dic.Add (Key_AttickType, attick_type.ToString ());
				dic.Add (Key_AttickHurt, attick_hurt.ToString ());
			}

			JSONObject json = new JSONObject (dic);
			string text = json.ToString();
			return text;
		}
		//消息解析
		public NetInfo (string text) {
			//解析网络消息
			JSONObject json = new JSONObject(text);
			if (json.type != JSONObject.Type.OBJECT) {
				return;
			}
			Dictionary<string,string> dic = json.ToDictionary ();
			//判断消息类型和发送者
			if (!dic.ContainsKey (Key_Code) || !dic.ContainsKey (Key_FromID)) {
				return;
			}
			//消息类型
			code = (MessageCode)int.Parse (dic [Key_Code]);
			//发送者
			from = dic [Key_FromID];
			//接收者
			if (dic.ContainsKey (Key_ToID)) {
				to = dic [Key_ToID];
			}
			//时间
			if (dic.ContainsKey (Key_Time)) {
				time = dic [Key_Time];
			}

			//消息类型解析
			if (code == MessageCode.SendMessage) {
				//文字消息
				if (dic.ContainsKey (Key_Msg)) {
					message = dic [Key_Msg];
				}
			} else if (code == MessageCode.PlayerStatus) {
				//角色状态消息
				if (dic.ContainsKey (Key_Position)) {
					position = dic [Key_Position].ToVector3 ();
				} 
				if (dic.ContainsKey (Key_LocalScale)) {
					localScale = dic [Key_LocalScale].ToVector3 ();
				}
			} else if (code == MessageCode.PlayerInfo) {
				//角色信息
				if (dic.ContainsKey (Key_PlayerLife)) {
					player_life = int.Parse (dic [Key_PlayerLife]);
				} 
				if (dic.ContainsKey (Key_PlayerAttick)) {
					player_attick = int.Parse (dic [Key_PlayerAttick]);
				} 
				if (dic.ContainsKey (Key_PlayerArmor)) {
					player_armor = int.Parse (dic [Key_PlayerArmor]);
				} 
			} else if (code == MessageCode.PlayerAttack) {
				//攻击信息
				if (dic.ContainsKey (Key_AttickType)) {
					attick_type = int.Parse (dic [Key_AttickType]);
				} 
				if (dic.ContainsKey (Key_AttickHurt)) {
					attick_hurt = int.Parse (dic [Key_AttickHurt]);
				} 
			}
		}
		//构造角色状态消息
		public NetInfo (string name, GameObject obj) {
			code = MessageCode.PlayerStatus;
			from = name;
			position = obj.transform.position;
			localScale = obj.transform.localScale;
		}
		//构造文字消息
		public NetInfo (string name, string msg) {
			code = MessageCode.SendMessage;
			from = name;
			message = msg;
		}
		//构造角色信息
		public NetInfo(string name, int life, int attick, int armor) {
			code = MessageCode.PlayerInfo;
			from = name;
			player_life = life;
			player_attick = attick;
			player_armor = armor;
		}
		//构造攻击信息
		public NetInfo(string name, string to, int type, int hurt) {
			code = MessageCode.PlayerAttack;
			from = name;
			to = to;
			attick_type = type;
			attick_hurt = hurt;
		}
	}

	//通用方法
	public static class Universal {

		//已有类补充方法

		//string to vector3
		public static Vector3 ToVector3(this string text) {
			text = text.Replace ("(", "").Replace (")", "");
			string[] s = text.Split (',');
			if (s.Length == 3) {
				return new Vector3 (float.Parse (s [0]), float.Parse (s [1]), float.Parse (s [2]));
			}
			return Vector3.zero;
		}


		//普通方法

		//获取mac地址
		public static string getPhysicalAddress() {
			string physicalAddress = "";  
			NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();  
			foreach (NetworkInterface adaper in nice) {  
				if (adaper.Description == "en0") {  
					physicalAddress = adaper.GetPhysicalAddress ().ToString ();  
					break;  
				} 
				else {  
					physicalAddress = adaper.GetPhysicalAddress ().ToString ();  
					if (physicalAddress != "") {  
						break;  
					}
				} 
			}
			return physicalAddress;
		}

	}

	//通用点击事件
	public class UTouch {
		public static bool Began (){
			if (Input.GetMouseButtonDown (0)) {
				return true;
			} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				return true;
			}
			return false;
		}
		public static bool Moved (){
			if (Input.GetMouseButton (0)) {
				return true;
			} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				return true;
			}
			return false;
		}
		public static bool Ended (){
			if (Input.GetMouseButtonUp (0)) {
				return true;
			} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
				return true;
			}
			return false;
		}
		public static bool Canceled (){
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Canceled) {
				return true;
			}
			return false;
		}
		public static Vector2 Point() {
			if (Input.touchCount > 0) {
				return Input.GetTouch (0).position;
			} else {
				return Input.mousePosition;
			}
		}
	}
}
