using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GlobalControlsUtility
	{
		private const int VisibilityControlsPerRow = 5;

		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float num = curBaseY;
			Vector2 timeButSize = TimeControls.TimeButSize;
			float y = num - timeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, (float)(width - 150.0));
			width = Mathf.Min(width, 150f);
			Vector2 timeButSize = TimeControls.TimeButSize;
			float y = timeButSize.y;
			Rect timerRect = new Rect((float)(leftX + 16.0), curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect((float)(leftX - 20.0), (float)(curBaseY - 26.0), (float)(width + 20.0 - 7.0), 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, DateTime.Now.ToString("HH:mm"));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}
	}
}
