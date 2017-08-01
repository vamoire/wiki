using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaJiaJia;

public class GameController : MonoBehaviour {

	//自己的游戏角色
	public GameObject myPlayer = null;
	public Player myPlayerInfo = null;
	//所有其他角色
	public Dictionary<string, GameObject> AllUserDic = new Dictionary<string, GameObject>();
	//所有攻击道具
	public List<GameObject> AllAttickList = new List<GameObject> ();
	//角色ID
	public string UserID = "";


	//单例
	private static GameController __game_controller = null;
	public static GameController Share () {
		if (__game_controller == null) {
			GameObject obj = new GameObject("Game Manager");
			__game_controller = obj.AddComponent<GameController>();
			DontDestroyOnLoad(__game_controller);
		}
		return __game_controller;
	}

	void Awake() {
		
	}

	void OnEnable() {
		//添加通知监听
		var notiCenter = NotificationCenter.DefaultCenter ();
		//登录
		notiCenter.AddObserver (this, SocketGM.LoginNotification);
	}

	//登录通知事件
	void LoginNotification(Notification notification) {
		UserID = notification.data.ToString ();

		var socket = SocketGM.Share ();
		if (socket.Online) {
			//地图
			GameObject map = GameObject.Find ("Map");
			//销毁所有游戏角色和游戏道具
			List<GameObject> destroyList = new List<GameObject> ();
			for (int i = 0; i < map.transform.childCount; ++i) {
				var child = map.transform.GetChild (i).gameObject;
				PlayerController pc = child.GetComponent<PlayerController> ();
				Attick attick = child.GetComponent<Attick> ();
				if (pc != null || attick != null) {
					destroyList.Add (child);
				}
			}
			foreach (GameObject item in destroyList) {
				Destroy (item);
			}

			myPlayer = null;
			myPlayerInfo = null;

			if (HideStartUI ()) {

				//添加自身角色
				//add user
				myPlayer = (GameObject)Instantiate (Resources.Load ("BodyL"));
				myPlayer.name = UserID;
				myPlayer.transform.parent = map.transform;
				myPlayer.transform.position = new Vector3 (Random.Range(-15, 15), Random.Range(-10, 10), myPlayer.transform.position.z);

				//设置角色信息
				myPlayerInfo = myPlayer.GetComponent<Player> ();
				myPlayerInfo.Name = UserID;
				myPlayerInfo.Life = 100;
				myPlayerInfo.Attack = 80;
				myPlayerInfo.Armor = 10;
				myPlayerInfo.Status = Player.PlayerStatus.Life;

				//第一次同步角色信息
				socket.SendAsyncGameObject (myPlayer);

			}

		}
	}

	// Use this for initialization
	void Start () {

	}

	void OnDisable() {
		//移除通知监听
		var notiCenter = NotificationCenter.DefaultCenter ();
		notiCenter.RemoveObserver (this, SocketGM.LoginNotification);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//显示开始页面
	static public bool ShowStartUI() {
		var canvas = GameObject.Find("Canvas");
		if (canvas != null) {
			var can = canvas.GetComponent<Canvas> ();
			can.enabled = true;
			return true;
		}
		return false;
	}

	//隐藏开始页面
	static public bool HideStartUI() {
		var canvas = GameObject.Find("Canvas");
		if (canvas != null) {
			var can = canvas.GetComponent<Canvas> ();
			can.enabled = false;
			return true;
		}
		return false;
	}

	//开始游戏
	public void StartGame() {
		var socket = SocketGM.Share ();
		socket.StartConnectServer ();
	}
}
