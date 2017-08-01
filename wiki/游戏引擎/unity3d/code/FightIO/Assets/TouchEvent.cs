using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;//获取mac地址

namespace XiaJiaJia {
	//通用方法
	public static class Universal {

		//已有类补充方法

		//string to vector3
		public static Vector3 ToVector3(this string text) {
			text = text.Replace ("(", "").Replace (")", "");
			string[] s = text.Split (',');
			if (s.Length == 3) {
				return new Vector3 (float.Parse (s [0]), float.Parse (s [1]), float.Parse (s [2]));
			}
			return Vector3.zero;
		}


		//普通方法

		//获取mac地址
		public static string getPhysicalAddress() {
			string physicalAddress = "";  
			NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();  
			foreach (NetworkInterface adaper in nice) {  
				if (adaper.Description == "en0") {  
					physicalAddress = adaper.GetPhysicalAddress ().ToString ();  
					break;  
				} 
				else {  
					physicalAddress = adaper.GetPhysicalAddress ().ToString ();  
					if (physicalAddress != "") {  
						break;  
					}
				} 
			}
			return physicalAddress;
		}

	}

	//通用点击事件
	public class UTouch {
		public static bool Began (){
			if (Input.GetMouseButtonDown (0)) {
				return true;
			} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				return true;
			}
			return false;
		}
		public static bool Moved (){
			if (Input.GetMouseButton (0)) {
				return true;
			} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				return true;
			}
			return false;
		}
		public static bool Ended (){
			if (Input.GetMouseButtonUp (0)) {
				return true;
			} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
				return true;
			}
			return false;
		}
		public static bool Canceled (){
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Canceled) {
				return true;
			}
			return false;
		}
		public static Vector2 Point() {
			if (Input.touchCount > 0) {
				return Input.GetTouch (0).position;
			} else {
				return Input.mousePosition;
			}
		}
		public static List<Vector2> Points() {
			List<Vector2> list = new List<Vector2>();
			if (Input.touchCount > 0) {
				for (int i = 0; i < Input.touchCount; ++i) {
					list.Add (Input.GetTouch (i).position);
				}
			} else {
				list.Add (Input.mousePosition);
			}
			return list;
		}
	}
}
