using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景音乐
/// </summary>
public class BGM : MonoBehaviour {

	public AudioSource audioSource = null;

	static BGM __backgroundMusic = null;
	public static BGM share {
		get{
			if (__backgroundMusic == null) {
				//GameObject
				GameObject audioObj = new GameObject("BGM");
				DontDestroyOnLoad(audioObj);

				//AuidoSource
				var audioSource = audioObj.AddComponent<AudioSource>();
				audioSource.playOnAwake = true;
				audioSource.loop = true;

				//bgm
				var bgm = audioObj.AddComponent<BGM>();
				bgm.audioSource = audioSource;

				__backgroundMusic = bgm;
			}
			return __backgroundMusic;
		}
	}

	public void Play(string name) {
		if (audioSource.clip == null || audioSource.isPlaying == false || audioSource.clip.name != name) {
			var clip = Resources.Load(name, typeof(AudioClip)) as AudioClip;
			if (clip) {
				audioSource.clip = clip;
			}
			audioSource.Play();
		}
	}

	public void Stop() {
		audioSource.Stop();
	}

	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, AppConfig.BGMChangeNotification);
		BGMChangeNotification();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		NotificationCenter.DefaultCenter().RemoveObserver(this, AppConfig.BGMChangeNotification);
	}

	void BGMChangeNotification() {
		bool on = AppConfig.share.music;
		audioSource.volume = on ? 1 : 0;
	}
}
