using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000841 RID: 2113
	public static class DateReadout
	{
		// Token: 0x06002FC9 RID: 12233 RVA: 0x0019DF17 File Offset: 0x0019C317
		static DateReadout()
		{
			DateReadout.Reset();
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002FCA RID: 12234 RVA: 0x0019DF44 File Offset: 0x0019C344
		public static float Height
		{
			get
			{
				return (float)(48 + ((!DateReadout.SeasonLabelVisible) ? 0 : 26));
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x0019DF70 File Offset: 0x0019C370
		private static bool SeasonLabelVisible
		{
			get
			{
				return !WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null;
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x0019DFA0 File Offset: 0x0019C3A0
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

		// Token: 0x06002FCD RID: 12237 RVA: 0x0019E00C File Offset: 0x0019C40C
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
			string text = (!DateReadout.SeasonLabelVisible) ? "" : season.LabelCap();
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

		// Token: 0x040019CE RID: 6606
		private static string dateString;

		// Token: 0x040019CF RID: 6607
		private static int dateStringDay = -1;

		// Token: 0x040019D0 RID: 6608
		private static Season dateStringSeason = Season.Undefined;

		// Token: 0x040019D1 RID: 6609
		private static Quadrum dateStringQuadrum = Quadrum.Undefined;

		// Token: 0x040019D2 RID: 6610
		private static int dateStringYear = -1;

		// Token: 0x040019D3 RID: 6611
		private static readonly List<string> fastHourStrings = new List<string>();

		// Token: 0x040019D4 RID: 6612
		private const float DateRightPadding = 7f;
	}
}
