using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.UI;
using XiaJiaJia;


public class SocketGM : MonoBehaviour {

	public const string LoginNotification = "LoginNotification";

	//单例
	private static SocketGM shareSocketGM = null;
	public static SocketGM Share () {
		if (shareSocketGM == null) {
			GameObject obj = new GameObject("Socket Manager");
			shareSocketGM = obj.AddComponent<SocketGM>();
			DontDestroyOnLoad(shareSocketGM);
		}
		return shareSocketGM;
	}

	//服务器地址
	public string ServerAddress = "ws://114.215.156.2:8181";
	//在线状态
	public bool Online = false;
	//当前的消息
	public string Message = "";
	//用户ID
	private string UserID = "";

	private WebSocket ws = null;
	private List<string> NetDataList = new List<string> ();

	//离线超时时间
	private float OutTime = 10f;
	//离线超时计时
	private float OutTimeDt = 0f;


	void Awake () {
		string[] pro = new string[0];
		this.ws = new WebSocket(ServerAddress, pro);
		this.ws.OnOpen += (object sender, EventArgs e) => {
			Debug.Log ("OnOpen" + e.ToString());
			this.Message = "OnOpen" + e.ToString();
		};
		this.ws.OnClose += (object sender, CloseEventArgs e) => {
			Debug.Log ("OnClose" + e.ToString());
			this.Message = "OnClose" + e.ToString();
		};
		this.ws.OnError += (object sender, ErrorEventArgs e) => {
			Debug.Log ("OnError" + e.Message);
			this.Message = "OnError" + e.ToString();
		};
		this.ws.OnMessage += (object sender, MessageEventArgs e) => {
			Debug.Log ("OnMessage:" + e.Data);
			this.Message = "OnMessage" + e.Data;
			//消息添加到网络数据处理列表
			this.NetDataList.Add(e.Data);
		};
	}

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate() {
		
		//在线状态更新
		if (this.NetDataList.Count > 0) {
			//收到服务器消息 

			//重置离线计时
			this.OutTimeDt = 0f;

			if (!this.Online) {
				//转为在线状态
				this.Online = true;
			}

		} else if (this.Online) {
			this.OutTimeDt += 0.02f;
			if (this.OutTimeDt >= this.OutTime) {
				//转为离线状态
				this.Online = false;
			}
		}

		//网络数据处理
		for (int i = 0; i < this.NetDataList.Count; ++i) {
			string data = this.NetDataList [i];
			this.NetDataExecute (data);
		}
		this.NetDataList.Clear ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDestroy() {
		if (this.ws != null) {
			this.ws.CloseAsync ();
			this.ws = null;
		}
	}


	//连接服务器
	public void StartConnectServer() {
		this.ws.ConnectAsync ();
	}

	//处理网络数据
	public void NetDataExecute(string data) {

		var gc = GameController.Share();

		NetInfo info = new NetInfo (data);

		//空消息 or 指定其他人接收的消息
		if (info.isNull () || (info.to.Length > 0 && info.to != this.UserID)) {
			return;
		}
		//发送者
		string name = info.from;

		//消息类型判断
		if (info.code == MessageCode.Login) {
			//登录消息
			UserID = info.loginID;
			Debug.Log ("用户:" + UserID + "登录");
			//登录通知
			NotificationCenter.DefaultCenter ().PostNotification (new Notification (this, LoginNotification, UserID));
		}
		else if (info.code == MessageCode.SendMessage) {
			//文字消息
			Debug.Log (name + ":" + info.message);
		} else if (info.code == MessageCode.PlayerInfo) {
			//同步 非自己 游戏对象消息
			GameObject item = null;
			bool create = false;
			if (info.from != this.UserID) {
				//如果不存在此角色 创建角色
				if (!gc.AllUserDic.ContainsKey (name)) {
					Debug.Log ("Add User: " + name);
					//add user
					GameObject obj = (GameObject)Instantiate (Resources.Load ("BodyS"));
					obj.name = name;
					GameObject node = GameObject.Find ("Map");
					obj.transform.parent = node.transform;
					gc.AllUserDic.Add (name, obj);

					create = true;
				}
				item = gc.AllUserDic [name];
			} else {
//				item = GameController.getInstance ().myPlayer;
			}

			//同步角色状态
			if (item != null) {
				item.transform.position = info.position;
				item.transform.localScale = info.localScale;

				//同步角色信息
				Player player = item.GetComponent<Player> ();
				player.Name = name;
				player.Life = info.player_life;
				player.Attack = info.player_attick;
				player.Armor = info.player_armor;

				if (create) {
					player.Status = Player.PlayerStatus.Life;
				}
			}
		} else if (info.code == MessageCode.AttackInfo) {
			//攻击消息
			GameObject obj = (GameObject)Instantiate (Resources.Load ("Attick"));
			obj.name = "Attick(" + info.from + ")";
			GameObject node = GameObject.Find ("Map");
			obj.transform.parent = node.transform;

			Attick attick = obj.GetComponent<Attick> ();
			attick.ID = info.attickID;
			attick.User = info.from;
			attick.Begin = info.attickBegin;
			attick.End = info.attickEnd;
			attick.Hurt = info.attickHurt;
			obj.transform.position = info.attickBegin;

			gc.AllAttickList.Add (obj);
		} else if (info.code == MessageCode.AttackDestroy) {
			//攻击销毁
			for (int i = 0; i < gc.AllAttickList.Count; ++i) {
				GameObject obj = gc.AllAttickList [i];
				if (obj != null) {
					Attick attickInfo = obj.GetComponent<Attick> ();
					if (attickInfo.User == info.from && attickInfo.ID == info.attickID) {
						//找到需要销毁的ID
						attickInfo.destroyAttick();
						break;
					}
				}
			}
		}

	}

	//发送同步角色状态消息
	public void SendAsyncGameObject(GameObject obj) {
		NetInfo info = new NetInfo (this.UserID, obj);
		string msg = info.ToNetString ();
//		Debug.Log ("msg:" + msg);
		if (msg == "") {
			return;
		}
		this.ws.SendAsync(msg, delegate(bool b) {
			this.Message = msg;
		});
	}


	//发送消息
	public void SendMsg(string message) {
		NetInfo info = new NetInfo (this.UserID, message);
		string msg = info.ToNetString();
		this.Message = "Send: " + msg;
		if (msg == "") {
			return;
		}
		this.ws.SendAsync(msg, delegate(bool b) {
			this.Message = "Send:" + b;
		});
	}

	//发送攻击消息
	public void SendAttick(GameObject obj, Vector3 end) {
		//获取攻击起点
		Vector3 begin = obj.transform.position;
		begin.y = begin.y - 0.8f;
		end.z = begin.z;
		Player player = obj.GetComponent<Player> ();
		NetInfo info = new NetInfo(this.UserID, begin, end, player.Attack);
		string msg = info.ToNetString ();
//		Debug.Log ("msg:" + msg);
		if (msg == "") {
			return;
		}
		this.ws.SendAsync(msg, delegate(bool b) {
			this.Message = msg;
		});
	}

	//发送攻击销毁
	public void SendAttickDestroy(string name, string id) {
		//获取攻击起点
		NetInfo info = new NetInfo(MessageCode.AttackDestroy, name, id);
		string msg = info.ToNetString ();
//		Debug.Log ("msg:" + msg);
		if (msg == "") {
			return;
		}
		this.ws.SendAsync(msg, delegate(bool b) {
			this.Message = msg;
		});
	}
}