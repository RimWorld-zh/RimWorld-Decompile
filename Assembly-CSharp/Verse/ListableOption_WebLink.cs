using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9F RID: 3743
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x04003A75 RID: 14965
		public Texture2D image;

		// Token: 0x04003A76 RID: 14966
		public string url;

		// Token: 0x04003A77 RID: 14967
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);

		// Token: 0x06005868 RID: 22632 RVA: 0x002D5051 File Offset: 0x002D3451
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x002D506F File Offset: 0x002D346F
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x002D5081 File Offset: 0x002D3481
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x002D5094 File Offset: 0x002D3494
		public override float DrawOption(Vector2 pos, float width)
		{
			float num = width - ListableOption_WebLink.Imagesize.x - 3f;
			float num2 = Text.CalcHeight(this.label, num);
			float num3 = Mathf.Max(this.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if (this.image != null)
			{
				Rect position = new Rect(pos.x, pos.y + num3 / 2f - ListableOption_WebLink.Imagesize.y / 2f, ListableOption_WebLink.Imagesize.x, ListableOption_WebLink.Imagesize.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Rect rect2 = new Rect(rect.xMax - num, pos.y, num, num2);
			Widgets.Label(rect2, this.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.action != null)
				{
					this.action();
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
