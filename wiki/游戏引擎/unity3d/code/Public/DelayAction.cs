using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFF
{
	public class DelayAction : MonoBehaviour {

		public class DelayItem {
			public DelayItem(){

			}
			public bool enable = false;
			public string name = "";
			public GameObject target = null;
			public float time = 0;
			public System.Action action = null;
		}

		List<DelayItem> list = new List<DelayItem>();

		static DelayAction __delayAction = null;
		public static DelayAction share {
			get {
				if (__delayAction == null) {
					GameObject obj = new GameObject("Delay Action");
					DontDestroyOnLoad(obj);
					__delayAction = obj.AddComponent<DelayAction>();
				}
				return __delayAction;
			}
		}

		/// <summary>
		/// 销毁
		/// </summary>
		public static void Destroy() {
			if (__delayAction) {
				Destroy(__delayAction.gameObject);
			}
		}

		public void Add(System.Action action, float time) {
			Add("", action, time, null);
		}

		public void Add(string name, System.Action action, float time, GameObject target = null) {
			DelayItem item = new DelayItem();
			item.enable = true;
			item.name = name;
			item.action = action;
			item.time = time;
			item.target = target;
			list.Add(item);
		}

		public void Remove(string name, GameObject target = null) {
			foreach (DelayItem item in list)
			{
				if (item.name == name && ((target == null) || (target == item.target))) {
					item.enable = false;
				}
			}
		}

		public void Remove(GameObject target) {
			foreach (DelayItem item in list)
			{
				if (item.target == target) {
					item.enable = false;
				}
			}
		}

		public void RemoveAll() {
			foreach (DelayItem item in list)
			{
				item.enable = false;
			}
		}

		// Use this for initialization
		void Start () {

		}

		void FixedUpdate()
		{
			float dt = Time.deltaTime;
			foreach (var item in list)
			{
				if (item.enable) {
					item.time -= dt;
					if (item.time <= 0) {
						item.enable = false;
						item.action();
					}
				}
			}
			list.RemoveAll((DelayItem item)=>{
				return item.enable == false;
			});
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}

