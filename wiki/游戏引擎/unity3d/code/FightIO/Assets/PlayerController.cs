using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaJiaJia;

public class PlayerController : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
		var gc = GameController.getInstance ();
		if (gc == null || gc.myPlayerInfo == null || gc.myPlayerInfo.Status != Player.PlayerStatus.Life) {
			return;
		}

		var socket = SocketGM.getInstance ();

		if (socket == null) {
			return;
		}

		//是否改变状态
		bool change = true;

		Vector3 p = this.transform.position;
		if (Input.GetKeyDown (KeyCode.W)) {
			this.transform.position = new Vector3 (p.x, p.y + 1, p.z);
		}
		else if (Input.GetKeyDown (KeyCode.S)) {
			this.transform.position = new Vector3 (p.x, p.y - 1, p.z);
		}
		else if (Input.GetKeyDown (KeyCode.A)) {
			this.transform.position = new Vector3 (p.x - 1, p.y, p.z);
		}
		else if (Input.GetKeyDown (KeyCode.D)) {
			this.transform.position = new Vector3 (p.x + 1, p.y, p.z);
		}
		else {
			change = false;
		}

		if (UTouch.Began ()) {
			//点击发射子弹
			Vector3 touchPoint = Camera.main.ScreenToWorldPoint (UTouch.Point ());

			socket.SendAttick (this.gameObject, touchPoint);

//			Debug.DrawLine (touchPoint, Vector3.zero);
//
//			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (UTouch.Point ()), Vector2.zero);
//			if (hit.collider != null) {
//				GameObject obj = hit.collider.gameObject;
//				if (obj == this.gameObject) {
//					obj.transform.localScale = new Vector3 (2.0f, 2.0f, obj.transform.localScale.y);
//					change = true;
//				}
//			}

		}

		if (change) {
			socket.SendAsyncGameObject (this.gameObject);
		}
	}
}
