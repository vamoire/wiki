using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserIcon : Config {

	static UserIcon __userIcon = null;
	public static UserIcon share {
		get {
			if (__userIcon == null) {
				__userIcon = Read<UserIcon>("userIcon.json");
			}
			return __userIcon;
		}
	}

	public UserIcon(){

	}

	public Sprite GetSprite(int idx) {
		Sprite sprite = null;
		if (data != null) {
			foreach (UserIconItem item in data)
			{
				if (item.id == idx) {
					sprite = Resources.Load(item.name, typeof(Sprite)) as Sprite;
					break;
				}
			}
		}
		if (sprite == null) {
			sprite = Resources.Load("ZY_TX01hui", typeof(Sprite)) as Sprite;
		}
		return sprite;
	}

	public string version = "";
	public UserIconItem[] data = null;
}

public class UserIconItem {
	public UserIconItem() {

	}

	public int id = -1;
	public string name = "";
}
