using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

	public GameObject Target = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Target == null) {
			Target = GameController.Share ().myPlayer;
		}
		if (Target != null) {
			transform.position = new Vector3 (Target.transform.position.x, Target.transform.position.y, transform.position.z);
		}
	}
}
