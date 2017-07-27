using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaJiaJia;

public class Player : MonoBehaviour {

	public enum PlayerStatus{
		Init,
		Life,
		Dead,
		Destroy
	};

	//信息文字
	private TextMesh InfoTextMesh = null;
	//名称
	public string Name = "";
	//血量
	public int Life = 0;
	//攻击
	public int Attack = 0;
	//防御
	public int Armor = 0;
	//状态
	public PlayerStatus Status = PlayerStatus.Init;

	// Use this for initialization
	void Start () {
		InfoTextMesh = transform.Find("InfoText").GetComponent<TextMesh> ();
	}

	// Update is called once per frame
	void Update () {
		
		//同步角色信息
		InfoTextMesh.text = Life.ToString();

		if (Status == PlayerStatus.Life) {
			var gc = GameController.getInstance ();
			//血量判断
			if (Life <= 0 && gc) {
				//挂掉了
				Status = PlayerStatus.Dead;
				var dic = gc.AllUserDic;
				if (dic != null && dic.ContainsKey (Name)) {
					dic.Remove (Name);
				}

				if (Name == gc.UserName) {
					//自己挂掉了
					GameController.ShowStartUI ();
				} else {
					//销毁对象
					Destroy (gameObject);
				}
			}
		}
	}
}
