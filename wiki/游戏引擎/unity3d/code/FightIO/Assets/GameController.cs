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
	//角色名称
	public string UserName = "";


	private NotificationCenter notiCenter = null;


	static GameController __game_controller = null;
	static public GameController getInstance() {
		return __game_controller;
	}

	void Awake() {
		//默认名称
		UserName = "user" + Universal.getPhysicalAddress();
		Debug.Log (UserName);
		__game_controller = this;
	}

	void OnEnable() {
		notiCenter = NotificationCenter.DefaultCenter ();
		notiCenter.AddObserver (this, "NotifiEvent");
		//		NotificationCenter.DefaultCenter ().PostNotification (new Notification (this, "NotifiEvent"));
	}

	void NotifiEvent(Notification notification) {

	}

	// Use this for initialization
	void Start () {

	}

	void OnDisable() {
		notiCenter.RemoveObserver (this, "NotifiEvent");
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
		var socket = SocketGM.getInstance ();
		if (socket) {

			myPlayer = GameObject.Find ("BodyL(Clone)");

			if (myPlayer != null) {
				Destroy (myPlayer);
				myPlayer = null;
				myPlayerInfo = null;
			}
			if (myPlayer == null) {

				if (HideStartUI ()) {
					
					//添加自身角色
					//add user
					myPlayer = (GameObject)Instantiate (Resources.Load ("BodyL"));
					GameObject node = GameObject.Find ("Map");
					myPlayer.transform.parent = node.transform;

					//设置角色信息
					myPlayerInfo = myPlayer.GetComponent<Player> ();
					myPlayerInfo.Name = UserName;
					myPlayerInfo.Life = 100;
					myPlayerInfo.Attack = 80;
					myPlayerInfo.Armor = 10;
					myPlayerInfo.Status = Player.PlayerStatus.Life;

					//第一次同步角色信息
					socket.SendAsyncGameObject (myPlayer);

				}
			}

		}
	}
}
