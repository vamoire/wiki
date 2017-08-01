using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attick : MonoBehaviour {

	public string ID = "";
	public string User = "";
	public Vector3 Begin = Vector3.zero;
	public Vector3 End = Vector3.zero;
	public int Hurt = 0;
	public float Speed = 2f;
	//射程
	public float Len = 20;

	//计时
	private float time = 0;
	//销毁时间
	private float desTime = 0;
	private bool isDes = false;

	// Use this for initialization
	void Start () {
		//终点重新计算
		End = Begin + (End - Begin) / Vector3.Distance(Begin,End) * Len;

		//持续时间
		desTime = Vector3.Distance (Begin, End) / Speed;
		if (GameController.Share ().UserID == User && User != "") {
			//自己发射的子弹
			var render = gameObject.GetComponent<SpriteRenderer>();
			Texture2D texture2d = (Texture2D)Resources.Load ("Neptune");
			Sprite sp = Sprite.Create (texture2d, render.sprite.textureRect, new Vector2 (0.5f, 0.5f));
			render.sprite = sp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		//销毁判断
		if (isDes) {
			return;
		}

		if (time >= desTime) {
			
			this.destroyAttick();

			return;
		}

		//TODO:角度刷新

		//位置刷新
		transform.position = Begin + (End - Begin) / desTime * time;

		time += 0.02f;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (isDes) {
			return;
		}

		//碰撞到自己
		if (GameController.Share ().UserID != User && User != "") {
			//不是自己发射的子弹
			var player = GameController.Share().myPlayer;
			if (player != null && GameController.Share().myPlayerInfo.Status == Player.PlayerStatus.Life) {
				if (player.GetComponent<Collider2D> () == collider) {
					//别人的子弹射到自己

					var info = player.GetComponent<Player> ();
				if (Hurt > info.Armor) {
					info.Life = info.Life - (Hurt - info.Armor);
				}
					//发送伤害消息
					SocketGM.Share().SendAsyncGameObject(player);

					//发送子弹销毁消息
					SocketGM.Share().SendAttickDestroy(User,ID);

					//销毁子弹
					this.destroyAttick();
				}
			}
		}
	}

	public void destroyAttick() {
		//销毁子弹
		isDes = true;

		var all = GameController.Share ().AllAttickList;
		if (all != null) {
			all.Remove (gameObject);
		}

		Destroy (gameObject);
	}
}
