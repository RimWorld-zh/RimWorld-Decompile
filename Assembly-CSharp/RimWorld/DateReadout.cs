using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class DateReadout
	{
		private static string dateString;

		private static int dateStringDay = -1;

		private static Season dateStringSeason = Season.Undefined;

		private static Quadrum dateStringQuadrum = Quadrum.Undefined;

		private static int dateStringYear = -1;

		private static readonly List<string> fastHourStrings = new List<string>();

		private const float DateRightPadding = 7f;

		static DateReadout()
		{
			DateReadout.Reset();
		}

		public static float Height
		{
			get
			{
				return (float)(48 + ((!DateReadout.SeasonLabelVisible) ? 0 : 26));
			}
		}

		private static bool SeasonLabelVisible
		{
			get
			{
				return !WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null;
			}
		}

		public static void Reset()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringQuadrum = Quadrum.Undefined;
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
				if (Find.CurrentMap == null)
				{
					return;
				}
				location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
			}
			int index = GenDate.HourInteger((long)Find.TickManager.TicksAbs, location.x);
			int num = GenDate.DayOfTwelfth((long)Find.TickManager.TicksAbs, location.x);
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, location);
			Quadrum quadrum = GenDate.Quadrum((long)Find.TickManager.TicksAbs, location.x);
			int num2 = GenDate.Year((long)Find.TickManager.TicksAbs, location.x);
			string text = (!DateReadout.SeasonLabelVisible) ? string.Empty : season.LabelCap();
			if (num != DateReadout.dateStringDay || season != DateReadout.dateStringSeason || quadrum != DateReadout.dateStringQuadrum || num2 != DateReadout.dateStringYear)
			{
				DateReadout.dateString = GenDate.DateReadoutStringAt((long)Find.TickManager.TicksAbs, location);
				DateReadout.dateStringDay = num;
				DateReadout.dateStringSeason = season;
				DateReadout.dateStringQuadrum = quadrum;
				DateReadout.dateStringYear = num2;
			}
			Text.Font = GameFont.Small;
			float num3 = Mathf.Max(Mathf.Max(Text.CalcSize(DateReadout.fastHourStrings[index]).x, Text.CalcSize(DateReadout.dateString).x + 7f), Text.CalcSize(text).x);
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
			rect.yMin += 26f;
			if (!text.NullOrEmpty())
			{
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			TooltipHandler.TipRegion(dateRect, new TipSignal(delegate()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum2 = (Quadrum)i;
					stringBuilder.AppendLine(quadrum2.Label() + " - " + quadrum2.GetSeason(location.y).LabelCap());
				}
				return "DateReadoutTip".Translate(new object[]
				{
					GenDate.DaysPassed,
					15,
					season.LabelCap(),
					15,
					GenDate.Quadrum((long)GenTicks.TicksAbs, location.x).Label(),
					stringBuilder.ToString()
				});
			}, 86423));
		}

		[CompilerGenerated]
		private sealed class <DateOnGUI>c__AnonStorey0
		{
			internal Vector2 location;

			internal Season season;

			public <DateOnGUI>c__AnonStorey0()
			{
			}

			internal string <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum = (Quadrum)i;
					stringBuilder.AppendLine(quadrum.Label() + " - " + quadrum.GetSeason(this.location.y).LabelCap());
				}
				return "DateReadoutTip".Translate(new object[]
				{
					GenDate.DaysPassed,
					15,
					this.season.LabelCap(),
					15,
					GenDate.Quadrum((long)GenTicks.TicksAbs, this.location.x).Label(),
					stringBuilder.ToString()
				});
			}
		}
	}
}
