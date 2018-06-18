using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9E RID: 3742
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x06005844 RID: 22596 RVA: 0x002D3315 File Offset: 0x002D1715
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x002D3333 File Offset: 0x002D1733
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x06005846 RID: 22598 RVA: 0x002D3345 File Offset: 0x002D1745
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x002D3358 File Offset: 0x002D1758
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

		// Token: 0x04003A65 RID: 14949
		public Texture2D image;

		// Token: 0x04003A66 RID: 14950
		public string url;

		// Token: 0x04003A67 RID: 14951
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}
