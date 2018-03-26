using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePlus : MonoBehaviour {

	public Sprite en_image = null;

	// Use this for initialization
	void Start () {
		if (AppConfig.share.language == AppConfig.Language.EN && en_image) {
			Image image = GetComponent<Image>();
			if (image) {
				image.sprite = en_image;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
