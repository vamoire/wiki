using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkErrorAlert : MonoBehaviour {

	/// <summary>
	/// 显示网络异常提示
	/// </summary>
	/// <returns></returns>
	public static NetworkErrorAlert Show() {
		GameObject obj = Instantiate(Resources.Load("NetworkError")) as GameObject;
		NetworkErrorAlert networkErrorAlert = obj.GetComponent<NetworkErrorAlert>();
		return networkErrorAlert;
	}

	[SerializeField]
	GameObject UI_CN = null;
	[SerializeField]
	GameObject UI_EN = null;

	Animator animator = null;

	// Use this for initialization
	void Start () {
		if (AppConfig.share.language == AppConfig.Language.CN) {
			UI_CN.SetActive(true);
			UI_EN.SetActive(false);
		}
		else {
			UI_CN.SetActive(false);
			UI_CN.SetActive(true);
		}

		animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ButtonEvent_OK() {
		animator.SetBool("Close", true);
		Destroy(gameObject, 0.15f);
	}

}
