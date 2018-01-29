using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class FileHelper {
	
	public static string StreamingAssetsPath(string path) {
		#if UNITY_IPHONE
			string ret = Application.dataPath + "/Raw/" + path;
		#else
			string ret = Application.streamingAssetsPath + "/" + path;
		#endif
		return ret;
	}

	public static string PersistentDataPath(string path) {
		return Application.streamingAssetsPath + "/" + path;
	}

	public static string ReadStringFromFile(string path) {
		#if UNITY_ANDROID
			WWW www = new WWW(path);
			while (!www.isDone) { }
			return www.text;
		#else
			return File.ReadAllText (path, Encoding.UTF8);
		#endif
	}

	public static void WriteStringToFile(string str, string path) {
		File.WriteAllText(path, str, Encoding.UTF8);
	}
}
