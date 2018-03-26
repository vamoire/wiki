using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFF.Tools
{
	/// <summary>
	/// 工具类
	/// </summary>
	public class Tools : MonoBehaviour {

		/// <summary>
		/// 屏幕大小
		/// </summary>
		/// <returns></returns>
		public static Vector2 winSize {
			get {
				Vector2 ret = Vector2.zero;
				Camera ca = FindObjectOfType<Camera>();
				if (ca != null) {
					ret = Tools.GetWinSize(ca.orthographicSize);
				}
				return ret;
			}
		}

		/// <summary>
		/// 获取指定相机orthographicSize的屏幕大小
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Vector2 GetWinSize(float size) {
			float h = size;
			float w = h / Screen.height * Screen.width;
			return new Vector2(w * 2, h * 2);
		}

		public static Sprite CreateMiniSprite(Sprite resSprite, float scale) {
			//old texture
			Texture2D resTexture = resSprite.texture;
			//new texture
			Texture2D texture = new Texture2D((int)(resTexture.width / scale), (int)(resTexture.height / scale));
			//clear
			Color bgColor = new Color(0, 0, 0, 0);
			for (int x = 0; x < texture.width; x++)
			{
				for (int y = 0; y < texture.height; y++)
				{
					texture.SetPixel(x, y, bgColor);
				}
			}
			//old texture fill to new texture
			Vector2Int dis = new Vector2Int(texture.width - resTexture.width, texture.height - resTexture.height);
			texture.SetPixels(dis.x / 2, dis.y / 2, resTexture.width, resTexture.height, resTexture.GetPixels());
			//flush
			texture.Apply();
			//new Sprite
			Sprite miniSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			return miniSprite;
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}


