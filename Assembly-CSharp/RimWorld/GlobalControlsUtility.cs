using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000841 RID: 2113
	public static class GlobalControlsUtility
	{
		// Token: 0x040019D9 RID: 6617
		private const int VisibilityControlsPerRow = 5;

		// Token: 0x06002FCE RID: 12238 RVA: 0x0019EEF8 File Offset: 0x0019D2F8
		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float y = curBaseY - TimeControls.TimeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x0019EF44 File Offset: 0x0019D344
		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, width - 150f);
			width = Mathf.Min(width, 150f);
			float y = TimeControls.TimeButSize.y;
			Rect timerRect = new Rect(leftX + 16f, curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x0019EFA8 File Offset: 0x0019D3A8
		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x0019EFE0 File Offset: 0x0019D3E0
		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, DateTime.Now.ToString("HH:mm"));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}
	}
}
