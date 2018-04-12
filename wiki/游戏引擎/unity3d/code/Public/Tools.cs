using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FFF.Tools
{
	/// <summary>
	/// 工具类
	/// </summary>
	public class Tools {

		/// <summary>
		/// 屏幕大小
		/// </summary>
		/// <returns></returns>
		public static Vector2 winSize {
			get {
				Vector2 ret = Vector2.zero;
				Camera ca = GameObject.FindObjectOfType<Camera>();
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

		/// <summary>
		/// 创建缩小的Sprite
		/// </summary>
		/// <param name="resSprite">原Sprite</param>
		/// <param name="scale">缩小比(0 ~ 1)</param>
		/// <returns>缩小后的Sprite</returns>
		public static Sprite CreateMiniSprite(Sprite resSprite, float scale) {
			//old texture
			Texture2D resTexture = resSprite.texture;
			//new texture
			Texture2D texture = new Texture2D((int)(resTexture.width / scale), (int)(resTexture.height / scale));
			//clear
			ClearTexture(texture);
			//old texture fill to new texture
			Vector2Int dis = new Vector2Int(texture.width - resTexture.width, texture.height - resTexture.height);
			texture.SetPixels(dis.x / 2, dis.y / 2, resTexture.width, resTexture.height, resTexture.GetPixels());
			//flush
			texture.Apply();
			//new Sprite
			Sprite miniSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			return miniSprite;
		}

		/// <summary>
		/// 清空纹理
		/// </summary>
		/// <param name="texture"></param>
		public static void ClearTexture(Texture2D texture) {
			ClearTexture(texture, Color.clear);
		}
		/// <summary>
		/// 清空纹理
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="color"></param>
		public static void ClearTexture(Texture2D texture, Color color) {
			for (int x = 0; x < texture.width; x++)
			{
				for (int y = 0; y < texture.height; y++)
				{
					texture.SetPixel(x, y, color);
				}
			}
			texture.Apply();
		}
		
		/// <summary>
		/// 纹理缩放
		/// </summary>
		/// <param name="source"></param>
		/// <param name="targetWidth"></param>
		/// <param name="targetHeight"></param>
		/// <returns></returns>
		public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
		{
			Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

			float incX = (1.0f / (float)targetWidth);
			float incY = (1.0f / (float)targetHeight);

			for (int i = 0; i < result.height; ++i)
			{
				for (int j = 0; j < result.width; ++j)
				{
					Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
					result.SetPixel(j, i, newColor);
				}
			}

			result.Apply();
			return result;
		}
		/// <summary>
		/// 缩放精灵
		/// </summary>
		/// <param name="one"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static Sprite ScaleSprite(Sprite one, float scale) {
			Texture2D oneTexture = one.texture;
			int width = (int)(oneTexture.width * scale);
			int height = (int)(oneTexture.height * scale);
			Texture2D texture = ScaleTexture(oneTexture, width, height);
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
			return sprite;
		}

		/// <summary>
		/// 合并精灵
		/// </summary>
		/// <param name="one"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static Sprite MergeSprite(Sprite one, Sprite other) {
			Texture2D oneTexture = one.texture;
			Texture2D otherTexture = other.texture;
			int width = Mathf.Max(oneTexture.width, otherTexture.width);
			int height = Mathf.Max(oneTexture.height, otherTexture.height);
			//new texture
			Texture2D texture = new Texture2D(width, height);
			//clear
			ClearTexture(texture);
			//one
			Vector2Int oneDis = new Vector2Int(width - oneTexture.width, height - oneTexture.height);
			texture.SetPixels(oneDis.x / 2, oneDis.y / 2, oneTexture.width, oneTexture.height, oneTexture.GetPixels());
			texture.Apply();
			//other
			Vector2Int otherDis = new Vector2Int(width - otherTexture.width, height - otherTexture.height);
			for(int x = 0; x < otherTexture.width; ++x) {
				for (int y = 0; y < otherTexture.height; y++)
				{
					Color c = otherTexture.GetPixel(x, y);
					if (c.a > 0.9) {
						Vector2Int p = new Vector2Int(otherDis.x / 2 + x, otherDis.y / 2 + y);
						texture.SetPixel(p.x, p.y, c);
					} 
				}
			}
			// texture.SetPixels(otherDis.x / 2, otherDis.y / 2, otherTexture.width, otherTexture.height, otherTexture.GetPixels());
			texture.Apply();
			//new sprite
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
			return sprite;
		}

		/// <summary>
		/// 设置GameObject所在Layer
		/// </summary>
		/// <param name="obj">GameObject对象</param>
		/// <param name="name">Layer名称</param>
		/// <param name="child">同时改变子对象</param>
		public static void SetGameObjectLayer(GameObject obj, string name, bool child = true) {
			int layer = LayerMask.NameToLayer(name);
			obj.layer = layer;
			if (child) {
				foreach (UnityEngine.Transform item in obj.GetComponentsInChildren<UnityEngine.Transform>())
				{
					item.gameObject.layer = layer;
				}
			}
		}

		static Button[] __buttonArr = null;
		/// <summary>
		/// 重新获取缓存按钮
		/// </summary>
		public static void ReloadCacheButtons() {
			__buttonArr = GameObject.FindObjectsOfType<Button>();
		}
		/// <summary>
		/// 清空缓存按钮
		/// </summary>
		public static void ClearCacheButtons() {
			__buttonArr = null;
		}
		/// <summary>
		/// 点p是否在缓存按钮范围内
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static bool CacheButtonIncludePoint(Vector3 p){
			bool inButton = false;
			if (__buttonArr == null) {
				return inButton;
			}
			foreach (Button item in __buttonArr)
			{
				if (item.gameObject.activeInHierarchy) {
					RectTransform rt = item.gameObject.GetComponent<RectTransform>();
					if(RectTransformIncludeVec3(rt, p)) {
						inButton = true;
						break;
					}
				}
			}
			return inButton;
		}

		/// <summary>
		/// 判断vector3是否在recttransform范围内
		/// </summary>
		/// <param name="rt"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		public static bool RectTransformIncludeVec3(RectTransform rt, Vector3 p){
			GameObject button = rt.gameObject;
			var rtp = rt.position;
			var rts = rt.sizeDelta;
			return p.x > rtp.x - rts.x / 2 && p.x < rtp.x + rts.x / 2 && p.y > rtp.y - rts.y / 2 && p.y < rtp.y + rts.y / 2;
		}
	}
}


