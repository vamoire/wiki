using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFF.Tools;

/// <summary>
/// 点击按钮缩放
/// </summary>
public class ButtonScale : MonoBehaviour {

	/// <summary>
	/// 点击按钮后的缩放大小
	/// </summary>
	float press = 0.8f;

	// Use this for initialization
	void Start () {
		Flash();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Flash() {
		//button
		Button button = GetComponent<Button>();
		button.transition = Button.Transition.SpriteSwap;
		SpriteState spriteState = button.spriteState;
		Sprite sprite = button.gameObject.GetComponent<Image>().sprite;
		spriteState.pressedSprite = Tools.CreateMiniSprite(sprite, press);
		button.spriteState = spriteState;
	}
}
