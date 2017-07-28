using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;//获取mac地址

namespace XiaJiaJia {

	public enum MessageCode{
		Unknow = -1,	//未知
		SendMessage = 0,//发送消息
		PlayerInfo = 100,	//角色信息
		AttackInfo = 200,	//攻击消息
		AttackDestroy = 300,//攻击销毁
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

		//角色信息
		//坐标
		public Vector3 position = Vector3.zero;
		//缩放
		public Vector3 localScale = Vector3.one;
		//生命值
		public int player_life = 0;
		//攻击力
		public int player_attick = 0;
		//防御
		public int player_armor = 0;

		//道具攻击
		//ID
		public string attickID = "";
		//起点
		public Vector3 attickBegin = Vector3.zero;
		//终点
		public Vector3 attickEnd = Vector3.zero;
		//伤害
		public int attickHurt = 0;


		//消息封装中的key
		private static string Key_Code = "code";
		private static string Key_FromID = "from";
		private static string Key_ToID = "to";
		private static string Key_Time = "time";
		private static string Key_Msg = "msg";
		private static string Key_PlayerPosition = "p_po";
		private static string Key_PlayerLocalScale = "p_sa";
		private static string Key_PlayerLife = "p_li";
		private static string Key_PlayerAttick = "p_at";
		private static string Key_PlayerArmor = "p_ar";
		private static string Key_AttickID = "a_id";
		private static string Key_AttickBegin = "a_be";
		private static string Key_AttickEnd = "a_en";
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
			dic.Add (Key_Msg, message);
			dic.Add (Key_Time, time);
			if (code == MessageCode.SendMessage) {
				dic.Add (Key_ToID, to);
			} else if (code == MessageCode.PlayerInfo) {
				dic.Add (Key_PlayerPosition, position.ToString ());
				dic.Add (Key_PlayerLocalScale, localScale.ToString ());
				dic.Add (Key_PlayerLife, player_life.ToString ());
				dic.Add (Key_PlayerAttick, player_attick.ToString ());
				dic.Add (Key_PlayerArmor, player_armor.ToString ());
			} else if (code == MessageCode.AttackInfo) {
				dic.Add (Key_AttickID, attickID);
				dic.Add (Key_AttickBegin, attickBegin.ToString ());
				dic.Add (Key_AttickEnd, attickEnd.ToString ());
				dic.Add (Key_AttickHurt, attickHurt.ToString ());
			} else if (code == MessageCode.AttackDestroy) {
				dic.Add (Key_AttickID, attickID);
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
			} else if (code == MessageCode.PlayerInfo) {
				//角色信息
				if (dic.ContainsKey (Key_PlayerPosition)) {
					position = dic [Key_PlayerPosition].ToVector3 ();
				} 
				if (dic.ContainsKey (Key_PlayerLocalScale)) {
					localScale = dic [Key_PlayerLocalScale].ToVector3 ();
				}
				if (dic.ContainsKey (Key_PlayerLife)) {
					player_life = int.Parse (dic [Key_PlayerLife]);
				} 
				if (dic.ContainsKey (Key_PlayerAttick)) {
					player_attick = int.Parse (dic [Key_PlayerAttick]);
				} 
				if (dic.ContainsKey (Key_PlayerArmor)) {
					player_armor = int.Parse (dic [Key_PlayerArmor]);
				} 
			} else if (code == MessageCode.AttackInfo) {
				//攻击消息
				if (dic.ContainsKey (Key_AttickID)) {
					attickID = dic [Key_AttickID];
				} 
				if (dic.ContainsKey (Key_AttickBegin)) {
					attickBegin = dic [Key_AttickBegin].ToVector3 ();
				} 
				if (dic.ContainsKey (Key_AttickEnd)) {
					attickEnd = dic [Key_AttickEnd].ToVector3 ();
				} 
				if (dic.ContainsKey (Key_AttickHurt)) {
					attickHurt = int.Parse (dic [Key_AttickHurt]);
				} 
			}else if (code == MessageCode.AttackDestroy) {
				if (dic.ContainsKey (Key_AttickID)) {
					attickID = dic [Key_AttickID];
				} 
			}
		}
		//构造角色状态消息
		public NetInfo (string name, GameObject obj) {
			code = MessageCode.PlayerInfo;
			from = name;
			position = obj.transform.position;
			localScale = obj.transform.localScale;
			//获取角色信息
			Player info = obj.GetComponent<Player>();
			player_life = info.Life;
			player_attick = info.Attack;
			player_armor = info.Armor;
		}
		//构造文字消息
		public NetInfo (string name, string msg) {
			code = MessageCode.SendMessage;
			from = name;
			message = msg;
		}
		//构造攻击消息
		public NetInfo (string name, Vector3 begin, Vector3 end, int hurt) {
			code = MessageCode.AttackInfo;
			from = name;
			attickID = System.DateTime.Now.ToString ();
			attickBegin = begin;
			attickEnd = end;
			attickHurt = hurt;
		}
		public NetInfo (MessageCode c, string name, string id) {
			code = MessageCode.AttackDestroy;
			from = name;
			attickID = id;
			attickID = System.DateTime.Now.ToString ();
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
		public static List<Vector2> Points() {
			List<Vector2> list = new List<Vector2>();
			if (Input.touchCount > 0) {
				for (int i = 0; i < Input.touchCount; ++i) {
					list.Add (Input.GetTouch (i).position);
				}
			} else {
				list.Add (Input.mousePosition);
			}
			return list;
		}
	}
}
