using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class DateReadout
	{
		public const float Height = 48f;

		private const float DateRightPadding = 7f;

		private static string dateString;

		private static int dateStringDay;

		private static Season dateStringSeason;

		private static int dateStringYear;

		private static readonly List<string> fastHourStrings;

		static DateReadout()
		{
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringYear = -1;
			DateReadout.fastHourStrings = new List<string>();
			DateReadout.Reset();
		}

		public static void Reset()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringYear = -1;
			DateReadout.fastHourStrings.Clear();
			for (int i = 0; i < 24; i++)
			{
				DateReadout.fastHourStrings.Add(i + "LetterHour".Translate());
			}
		}

		public static void DateOnGUI(Rect dateRect)
		{
			Vector2 location;
			if (WorldRendererUtility.WorldRenderedNow && Find.WorldSelector.selectedTile >= 0)
			{
				location = Find.WorldGrid.LongLatOf(Find.WorldSelector.selectedTile);
			}
			else if (WorldRendererUtility.WorldRenderedNow && Find.WorldSelector.NumSelectedObjects > 0)
			{
				location = Find.WorldGrid.LongLatOf(Find.WorldSelector.FirstSelectedObject.Tile);
			}
			else
			{
				if (Find.VisibleMap == null)
				{
					return;
				}
				location = Find.WorldGrid.LongLatOf(Find.VisibleMap.Tile);
			}
			int index = GenDate.HourInteger((long)Find.TickManager.TicksAbs, location.x);
			int num = GenDate.DayOfTwelfth((long)Find.TickManager.TicksAbs, location.x);
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, location);
			int num2 = GenDate.Year((long)Find.TickManager.TicksAbs, location.x);
			if (num != DateReadout.dateStringDay || season != DateReadout.dateStringSeason || num2 != DateReadout.dateStringYear)
			{
				DateReadout.dateString = GenDate.DateReadoutStringAt((long)Find.TickManager.TicksAbs, location);
				DateReadout.dateStringDay = num;
				DateReadout.dateStringSeason = season;
				DateReadout.dateStringYear = num2;
			}
			Text.Font = GameFont.Small;
			float num3 = Mathf.Max(Text.CalcSize(DateReadout.fastHourStrings[index]).x, Text.CalcSize(DateReadout.dateString).x + 7f);
			dateRect.xMin = dateRect.xMax - num3;
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			GUI.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = dateRect.AtZero();
			rect.xMax -= 7f;
			Widgets.Label(rect, DateReadout.fastHourStrings[index]);
			rect.yMin += 26f;
			Widgets.Label(rect, DateReadout.dateString);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			TooltipHandler.TipRegion(dateRect, new TipSignal(() => "DateReadoutTip".Translate(new object[]
			{
				GenDate.DaysPassed,
				15,
				season.Label(),
				15,
				GenDate.Quadrum((long)GenTicks.TicksAbs, location.x)
			}), 86423));
		}
	}
}
