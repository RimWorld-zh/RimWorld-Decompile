using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5E RID: 3678
	public abstract class FeedbackItem
	{
		// Token: 0x04003970 RID: 14704
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04003971 RID: 14705
		private int uniqueID;

		// Token: 0x04003972 RID: 14706
		public float TimeLeft = 2f;

		// Token: 0x04003973 RID: 14707
		protected Vector2 CurScreenPos;

		// Token: 0x04003974 RID: 14708
		private static int freeUniqueID;

		// Token: 0x060056BA RID: 22202 RVA: 0x002CB770 File Offset: 0x002C9B70
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x002CB7D5 File Offset: 0x002C9BD5
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x060056BC RID: 22204
		public abstract void FeedbackOnGUI();

		// Token: 0x060056BD RID: 22205 RVA: 0x002CB80C File Offset: 0x002C9C0C
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f);
		}
	}
}
