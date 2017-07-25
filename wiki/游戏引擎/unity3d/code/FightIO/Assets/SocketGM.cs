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
	//用户ID
	private string UserName = "";
	public Text OutText = null;
	public Text InText = null;

	private WebSocket ws = null;
	private string Message = "";
	public Dictionary<string, GameObject> AllUserDic = new Dictionary<string, GameObject>();
	private List<string> NetDataList = new List<string> ();

	// Use this for initialization
	void Start () {
		//用户ID
		if (UserName.Length == 0) {
			UserName = "user" + Universal.getPhysicalAddress();
			Debug.Log (UserName);
		}

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
		this.ws.ConnectAsync ();
		__socketGM = this;

		NotificationCenter.DefaultCenter ().PostNotification (new Notification (this, "SoketGMStartNotification"));
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
		NetInfo info = new NetInfo (data);
		Debug.Log ("NetInfo:" + info.ToNetString());
		if (info.isNull () || info.from == this.UserName || (info.to.Length > 0 && info.to != this.UserName)) {
			//消息为空 or 自己发送的消息 or 指定的接收者不是自己
			return;
		}
		//发送者
		string name = info.from;
		//消息类型判断
		if (info.code == MessageCode.SendMessage) {
			//文字消息
			Debug.Log(name + ":" + info.message);
		} else if (info.code == MessageCode.PlayerStatus) {
			//同步游戏对象消息
			if (!this.AllUserDic.ContainsKey (name)) {
				Debug.Log ("Add User: " + info.from);
				//add user
				GameObject obj = (GameObject)Instantiate (Resources.Load ("BodyS"));
				GameObject node = GameObject.Find ("Main Camera");
				obj.transform.parent = node.transform;
				this.AllUserDic.Add (name, obj);
			}
			GameObject item = this.AllUserDic [name];
			if (item != null) {
				item.transform.position = info.position;
				item.transform.localScale = info.localScale;
			}
		}
	}

	//发送同步游戏信息消息
	public void SendAsyncGameObject(GameObject obj) {
		NetInfo info = new NetInfo (this.UserName, obj);
		string msg = info.ToNetString ();
		Debug.Log ("msg:" + msg);
		this.ws.SendAsync(msg, delegate(bool b) {
			this.Message = msg;
		});
	}


	//发送消息
	public void SendMessage() {
		NetInfo info = new NetInfo (this.UserName, this.InText.text);
		string msg = info.ToNetString();
		this.Message = "Send: " + this.InText.text;
		this.ws.SendAsync(msg, delegate(bool b) {
			this.Message = "Send:" + b;
		});
	}
}