using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("Network Manager").GetComponent<NetworkManagerHUD>().showGUI = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 10.0f;
		var y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

//		transform.Rotate(0, x, 0);
		transform.Translate(x, y, 0);
	}

	public override void OnStartLocalPlayer() {
//		GetComponent<SpriteRenderer> ().material.color = Color.blue;
		GetComponent<SpriteRenderer> ().color = Color.blue;
	}
}
