using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音播放
/// </summary>
public class Sound : MonoBehaviour {

	/// <summary>
	/// 声音播放组件
	/// </summary>
	public AudioSource audioSource = null;
	/// <summary>
	/// 声音播放组件名称
	/// </summary>
	public string soundName = "";

	/// <summary>
	/// 创建声音播放组件
	/// </summary>
	/// <param name="name">声音播放组件名称</param>
	/// <param name="keep">不销毁</param>
	/// <returns>声音播放组件</returns>
	public static Sound Create(string name = "Sound", bool keep = false) {
		//GameObject
		GameObject audioObj = new GameObject(name);
		if (keep) {
			DontDestroyOnLoad(audioObj);
		}
		//AuidoSource
		var audioSource = audioObj.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		//Sound
		Sound sound = audioObj.AddComponent<Sound>();
		sound.audioSource = audioSource;
		sound.soundName = name;
		return sound;
	}

	static Dictionary<string, Sound> __soundDic = new Dictionary<string, Sound>();

	/// <summary>
	/// 获取声音播放组件
	/// </summary>
	/// <param name="name">声音播放组件名称</param>
	/// <param name="keep">不销毁</param>
	/// <returns>声音播放组件</returns>
	public static Sound Share(string name = "Sound", bool keep = false) {
		Sound sound = null;
		if (__soundDic.ContainsKey(name)) {
			sound = __soundDic[name];
		}
		else {
			sound = Sound.Create(name, keep);
			__soundDic.Add(name, sound);
		}
		return sound;
	}

	/// <summary>
	/// 播放音效
	/// </summary>
	/// <param name="name">音效名称</param>
	/// <param name="loop">循环</param>
	/// <returns>声音播放组件</returns>
	public static Sound Play(string name, bool loop = false) {
		Sound sound = Sound.Create(name);
		sound.PlaySound(name, loop);
		return sound;
	}

	/// <summary>
	/// 播放声音
	/// </summary>
	/// <param name="name">声音名称</param>
	public void PlaySound(string name, bool loop = false) {
		var clip = Resources.Load(name, typeof(AudioClip)) as AudioClip;
		if (clip) {
			audioSource.clip = clip;
			audioSource.loop = loop;
		}
		audioSource.Play();
	}

	/// <summary>
	/// 暂停声音
	/// </summary>
	public void PauseSound() {
		audioSource.Pause();
	}

	/// <summary>
	/// 播放按钮点击声音
	/// </summary>
	public static void Button() {
		Sound.Share("Button_Sound_Player", true).PlaySound("dianji");
	}


	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, AppConfig.SoundChangeNotification);
		SoundChangeNotification();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		if (soundName.Length > 0 && __soundDic.ContainsKey(soundName)) {
			__soundDic.Remove(soundName);
		}
		NotificationCenter.DefaultCenter().RemoveObserver(this, AppConfig.SoundChangeNotification);
	}

	void SoundChangeNotification() {
		bool on = AppConfig.share.sound;
		audioSource.volume = on ? 1 : 0;
	}
}
