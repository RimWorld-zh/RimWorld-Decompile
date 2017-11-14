using System;
using UnityEngine;

namespace Verse
{
	public class ListableOption_WebLink : ListableOption
	{
		public Texture2D image;

		public string url;

		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);

		public ListableOption_WebLink(string label, Texture2D image)
			: base(label, null, null)
		{
			base.minHeight = 24f;
			this.image = image;
		}

		public ListableOption_WebLink(string label, string url, Texture2D image)
			: this(label, image)
		{
			this.url = url;
		}

		public ListableOption_WebLink(string label, Action action, Texture2D image)
			: this(label, image)
		{
			base.action = action;
		}

		public override float DrawOption(Vector2 pos, float width)
		{
			Vector2 imagesize = ListableOption_WebLink.Imagesize;
			float num = (float)(width - imagesize.x - 3.0);
			float num2 = Text.CalcHeight(base.label, num);
			float num3 = Mathf.Max(base.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if ((UnityEngine.Object)this.image != (UnityEngine.Object)null)
			{
				float x = pos.x;
				double num4 = pos.y + num3 / 2.0;
				Vector2 imagesize2 = ListableOption_WebLink.Imagesize;
				double y = num4 - imagesize2.y / 2.0;
				Vector2 imagesize3 = ListableOption_WebLink.Imagesize;
				float x2 = imagesize3.x;
				Vector2 imagesize4 = ListableOption_WebLink.Imagesize;
				Rect position = new Rect(x, (float)y, x2, imagesize4.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Rect rect2 = new Rect(rect.xMax - num, pos.y, num, num2);
			Widgets.Label(rect2, base.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (base.action != null)
				{
					base.action();
				}
				else
				{
					Application.OpenURL(this.url);
				}
			}
			return num3;
		}
	}
}
