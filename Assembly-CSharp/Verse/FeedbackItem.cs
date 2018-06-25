using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E61 RID: 3681
	public abstract class FeedbackItem
	{
		// Token: 0x04003978 RID: 14712
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04003979 RID: 14713
		private int uniqueID;

		// Token: 0x0400397A RID: 14714
		public float TimeLeft = 2f;

		// Token: 0x0400397B RID: 14715
		protected Vector2 CurScreenPos;

		// Token: 0x0400397C RID: 14716
		private static int freeUniqueID;

		// Token: 0x060056BE RID: 22206 RVA: 0x002CBA88 File Offset: 0x002C9E88
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x002CBAED File Offset: 0x002C9EED
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x060056C0 RID: 22208
		public abstract void FeedbackOnGUI();

		// Token: 0x060056C1 RID: 22209 RVA: 0x002CBB24 File Offset: 0x002C9F24
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
