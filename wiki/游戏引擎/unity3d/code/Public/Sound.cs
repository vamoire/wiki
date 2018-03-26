using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sound : MonoBehaviour {

	/// <summary>
	/// 声音播放组件
	/// </summary>
	public AudioSource audioSource = null;

	/// <summary>
	/// 创建声音播放组件
	/// </summary>
	/// <param name="name">声音播放组件名称</param>
	/// <returns>声音播放组件</returns>
	public static Sound Create(string name = "Sound") {
		//GameObject
		GameObject audioObj = new GameObject(name);
		//AuidoSource
		var audioSource = audioObj.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		//Sound
		Sound sound = audioObj.AddComponent<Sound>();
		sound.audioSource = audioSource;
		return sound;
	}

	/// <summary>
	/// 获取/创建声音播放组件
	/// </summary>
	/// <param name="name">声音播放组件名称</param>
	/// <returns>声音播放组件</returns>
	public static Sound GetOrCreate(string name = "Sound") {
		Sound sound = null;
		//get
		GameObject obj = GameObject.Find(name);
		if (obj != null) {
			sound = obj.GetComponent<Sound>();
		}
		else {
			//create
			sound = Sound.Create(name);
		}
		return sound;
	}

	/// <summary>
	/// 播放声音
	/// </summary>
	/// <param name="name">声音名称</param>
	public void Play(string name) {
		var clip = Resources.Load(name, typeof(AudioClip)) as AudioClip;
		if (clip) {
			audioSource.clip = clip;
		}
		audioSource.Play();
	}

	/// <summary>
	/// 播放按钮点击声音
	/// </summary>
	public static void Button() {
		Sound.GetOrCreate("Button_Sound_Player");//.Play("");
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
