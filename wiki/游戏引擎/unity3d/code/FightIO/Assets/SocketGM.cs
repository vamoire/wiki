using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.UI;
using XiaJiaJia;


public class SocketGM : MonoBehaviour {
	static public SocketGM __socketGM = null;
	static public SocketGM getInstance() {
		return __socketGM;
	}
	//服务器地址
	public string ServerAddress = "ws://114.215.156.2:8181";

	public Text OutText = null;
	public Text InText = null;

	private WebSocket ws = null;
	private string Message = "";
	private List<string> NetDataList = new List<string> ();
	//用户ID
	private string UserName = "";

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
//			Debug.Log ("OnMessage:" + e.Data);
			this.Message = "OnMessage" + e.Data;
			//消息添加到网络数据处理列表
			this.NetDataList.Add(e.Data);
		};
		this.ws.ConnectAsync ();

		__socketGM = this;
	}

	// Use this for initialization
	void Start () {
		UserName = GameController.getInstance ().UserName;
	}

	void FixedUpdate() {
		this.OutText.text = this.Message;

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

	//处理网络数据
	public void NetDataExecute(string data) {

		var gc = GameController.getInstance();
		if (gc == null) {
			return;
		}

		NetInfo info = new NetInfo (data);
//		Debug.Log ("NetInfo:" + info.ToNetString());
		//空消息 or 指定其他人接收的消息
		if (info.isNull () || (info.to.Length > 0 && info.to != this.UserName)) {
			//消息为空 or 自己发送的消息 or 指定的接收者不是自己
			return;
		}
		//发送者
		string name = info.from;
		//消息类型判断
		if (info.code == MessageCode.SendMessage) {
			//文字消息
			Debug.Log (name + ":" + info.message);
		} else if (info.code == MessageCode.PlayerInfo) {
			//同步 非自己 游戏对象消息
			GameObject item = null;
			bool create = false;
			if (info.from != this.UserName) {
				//如果不存在此角色 创建角色
				if (!gc.AllUserDic.ContainsKey (name)) {
					Debug.Log ("Add User: " + name);
					//add user
					GameObject obj = (GameObject)Instantiate (Resources.Load ("BodyS"));
					GameObject node = GameObject.Find ("Map");
					obj.transform.parent = node.transform;
					gc.AllUserDic.Add (name, obj);

					create = true;
				}
				item = gc.AllUserDic [name];
			} else {
				item = GameController.getInstance ().myPlayer;
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
			//攻击撤销
			for (int i = 0; i < gc.AllAttickList.Count; ++i) {
				GameObject obj = gc.AllAttickList [i];
				if (obj != null) {
					Attick attickInfo = obj.GetComponent<Attick> ();
					if (attickInfo.User == info.from && attickInfo.ID == info.attickID) {
						//找到需要撤销的ID
						attickInfo.destroyAttick();
						break;
					}
				}
			}
		}

	}

	//发送同步角色状态消息
	public void SendAsyncGameObject(GameObject obj) {
		NetInfo info = new NetInfo (this.UserName, obj);
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
	public void SendMessage() {
		NetInfo info = new NetInfo (this.UserName, this.InText.text);
		string msg = info.ToNetString();
		this.Message = "Send: " + this.InText.text;
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
		end.z = begin.z;
		Player player = obj.GetComponent<Player> ();
		NetInfo info = new NetInfo(this.UserName, begin, end, player.Attack);
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