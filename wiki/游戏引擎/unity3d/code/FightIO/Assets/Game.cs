using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaJiaJia;

public class Game : MonoBehaviour {

	private SocketGM socketGM = null;
	private NotificationCenter notiCenter = null;

	// Use this for initialization
	void Start () {
		this.socketGM = SocketGM.getInstance ();
	}

	void OnEnable() {
		notiCenter = NotificationCenter.DefaultCenter ();
		notiCenter.AddObserver(this, "SoketGMStartNotification");
	}

	void OnDisable() {
		notiCenter.RemoveObserver (this, "SoketGMStartNotification");
	}

	void SoketGMStartNotification(Notification notification) {
		this.socketGM = SocketGM.getInstance ();
	}

	// Update is called once per frame
	void Update () {

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
			Debug.DrawLine (Camera.main.ScreenToWorldPoint (UTouch.Point ()), Vector3.zero);
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (UTouch.Point ()), Vector2.zero);
			if (hit.collider != null) {
				GameObject obj = hit.collider.gameObject;
				if (obj == this.gameObject) {
					obj.transform.localScale = new Vector3 (2.0f, 2.0f, obj.transform.localScale.y);
					change = true;
				}
			}
		}

		if (change && this.socketGM != null) {
			this.socketGM.SendAsyncGameObject (this.gameObject);
		}
	}
}
