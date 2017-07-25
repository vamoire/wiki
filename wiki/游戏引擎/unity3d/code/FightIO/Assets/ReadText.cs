using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var fileAddress = Path.Combine (Application.persistentDataPath, "test.txt");
		var fileInfo = new FileInfo (fileAddress);
		StreamWriter w;
		if (!fileInfo.Exists) {
			w = File.CreateText (fileAddress);
		}
		else {
			w = new StreamWriter (fileAddress);
		}
		w.WriteLine ("123");
		w.Close ();

		if (fileInfo.Exists) {
			StreamReader r = new StreamReader (fileAddress);
			var s = r.ReadToEnd ();
			Debug.Log (s);
			r.Close ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
