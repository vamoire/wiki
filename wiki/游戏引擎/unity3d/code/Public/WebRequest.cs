using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace FFF.WebRequest
{
	public class WebRequest : MonoBehaviour {

		static WebRequest __webRequest = null;
		public static WebRequest share {
			get {
				if (__webRequest == null) {
					GameObject obj = new GameObject("Web Request");
					__webRequest = obj.AddComponent<WebRequest>();
					DontDestroyOnLoad(obj);
				}
				return __webRequest;
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		void OnDestroy()
		{
			StopAllCoroutines();
		}

		public Coroutine Request<T>(string url, System.Action<bool,T> action) {
			Coroutine cor = Request(url, (bool ret, string json)=>{
				T t = JsonConvert.DeserializeObject<T>(json);
				action(ret, t);
			});
			return cor;
		}

		public Coroutine Request(string url, System.Action<bool, string> action) {
			Coroutine cor = StartCoroutine(GetText(url, (bool ret, string text)=>{
				action(ret, text);
			}));
			return cor;
		}

		public void CancelRequest(Coroutine cor) {
			StopCoroutine(cor);
		}

		public void CancelAllRequest() {
			StopAllCoroutines();
		}

		IEnumerator GetText(string url, System.Action<bool,string> action) {
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.SendWebRequest();
	
			if(www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error);
				action(false, "");
			}
			else {
				// Show results as text
				Debug.Log(www.downloadHandler.text);
				action(true, www.downloadHandler.text);
				// Or retrieve results as binary data
				// byte[] results = www.downloadHandler.data;
			}
		}

		// void CreateUnityWebRequest() {
		// 	UnityWebRequest wr = new UnityWebRequest(); // Completely blank
		// 	UnityWebRequest wr2 = new UnityWebRequest("http://www.mysite.com"); // Target URL is set

		// 	// the following two are required to web requests to work
		// 	wr.url = "http://www.mysite.com";
		// 	wr.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided

		// 	wr.useHttpContinue = false;
		// 	wr.chunkedTransfer = false;
		// 	wr.redirectLimit = 0;  // disable redirects
		// 	wr.timeout = 60;       // don't make this small, web requests do take some time
		// }
	}
}


