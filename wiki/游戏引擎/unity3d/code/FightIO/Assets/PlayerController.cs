using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiaJiaJia;

public class PlayerController : MonoBehaviour {

	private CnControls.SimpleJoystick StickScript = null;

	private SocketGM socket = SocketGM.Share ();

	void Start () {
		
	}

	void Update () {
		var gc = GameController.Share ();
		if (gc.myPlayerInfo == null || gc.myPlayerInfo.Status != Player.PlayerStatus.Life) {
			return;
		}

		// Just use CnInputManager. instead of Input. and you're good to go
		var inputVector = new Vector3(CnControls.CnInputManager.GetAxis("Horizontal"), CnControls.CnInputManager.GetAxis("Vertical"));
		// If we have some input
		if (inputVector.sqrMagnitude > 0.001f)
		{
			Vector3 p = this.transform.position;
			this.transform.position = p + inputVector * 0.1f;
			socket.SendAsyncGameObject (this.gameObject);
		}

		if (StickScript == null) {
			var stick = GameObject.Find ("Joystick");
			if (stick != null) {
				StickScript = stick.GetComponent<CnControls.SimpleJoystick> ();
			}
		}

		List<Vector2> list = new List<Vector2> ();
		if (Input.GetMouseButtonDown (0)) {
			list.Add (Input.mousePosition);
		} else if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					list.Add (Input.GetTouch (i).position);
				}
			}
		}
		
		if (list.Count > 0 && StickScript != null) {
			//点击发射子弹
			Rect stickRect = StickScript.TouchZone.rect;
			for (int i = 0; i < list.Count; ++i) {
				Vector2 p = list [i];
				if (!stickRect.Contains (new Vector3 (p.x, p.y))) {
					Vector3 touchPoint = Camera.main.ScreenToWorldPoint (p);
					socket.SendAttick (this.gameObject, touchPoint);
					break;
				}
			}

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
	}
}
