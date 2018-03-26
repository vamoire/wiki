using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileHelper {

	public static string StreamingAssetsPath (string path) {
#if UNITY_EDITOR
		string url = Application.streamingAssetsPath + "/" + path;
#elif UNITY_IPHONE
		string url = Application.dataPath + "/Raw/" + path;
#elif UNITY_ANDROID
		string url = Application.streamingAssetsPath + "/" + path;
#else
		string url = Application.streamingAssetsPath + "/" + path;
#endif
		return url;
	}

	public static string PersistentDataPath (string path) {
		return Application.persistentDataPath + "/" + path;
	}

	public static string ReadStringFromFile (string path) {
#if UNITY_EDITOR || UNITY_IOS
		return File.ReadAllText (path, Encoding.UTF8);
#elif UNITY_ANDROID
		WWW www = new WWW (path);
		while (!www.isDone) { }
		return www.text;
#else
		WWW www = new WWW (path);
		while (!www.isDone) { }
		return www.text;
#endif
	}

	public static void WriteStringToFile (string str, string path) {
		File.WriteAllText (path, str);
	}
}