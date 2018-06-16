using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000843 RID: 2115
	public static class GlobalControlsUtility
	{
		// Token: 0x06002FD0 RID: 12240 RVA: 0x0019E898 File Offset: 0x0019CC98
		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float y = curBaseY - TimeControls.TimeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x0019E8E4 File Offset: 0x0019CCE4
		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, width - 150f);
			width = Mathf.Min(width, 150f);
			float y = TimeControls.TimeButSize.y;
			Rect timerRect = new Rect(leftX + 16f, curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0019E948 File Offset: 0x0019CD48
		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x0019E980 File Offset: 0x0019CD80
		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, DateTime.Now.ToString("HH:mm"));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x040019D7 RID: 6615
		private const int VisibilityControlsPerRow = 5;
	}
}
