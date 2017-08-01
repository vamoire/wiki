using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XiaJiaJia {

	public enum MessageCode{
		Unknow = -1,	//未知
		Login = 1,	//登录消息
		SendMessage = 2,//发送消息
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

		//登录消息
		public string loginID = "";

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
		private const string Key_Code = "code";
		private const string Key_FromID = "from";
		private const string Key_ToID = "to";
		private const string Key_Time = "time";
		private const string Key_LoginID = "l_id";
		private const string Key_Msg = "msg";
		private const string Key_PlayerPosition = "p_po";
		private const string Key_PlayerLocalScale = "p_sa";
		private const string Key_PlayerLife = "p_li";
		private const string Key_PlayerAttick = "p_at";
		private const string Key_PlayerArmor = "p_ar";
		private const string Key_AttickID = "a_id";
		private const string Key_AttickBegin = "a_be";
		private const string Key_AttickEnd = "a_en";
		private const string Key_AttickHurt = "a_hu";

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
			dic.Add (Key_Code, ((int)code).ToString ());
			dic.Add (Key_Time, time);
			dic.Add (Key_FromID, from);
			if (code == MessageCode.Login) {
				dic.Add (Key_LoginID, loginID);
			} else if (code == MessageCode.SendMessage) {
				dic.Add (Key_ToID, to);
				dic.Add (Key_Msg, message);
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
			//TODO: 临时解析登录消息
			if (text.IndexOf ("Hello ") != -1 && text.Length > 6) {
				code = MessageCode.Login;
				from = "server";
				loginID = text.Substring(6);
				loginID = loginID.Remove (loginID.Length - 1);
				return;
			}

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
}