using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E60 RID: 3680
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

		// Token: 0x060056BE RID: 22206 RVA: 0x002CB89C File Offset: 0x002C9C9C
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x002CB901 File Offset: 0x002C9D01
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x060056C0 RID: 22208
		public abstract void FeedbackOnGUI();

		// Token: 0x060056C1 RID: 22209 RVA: 0x002CB938 File Offset: 0x002C9D38
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
