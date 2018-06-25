using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F2 RID: 754
	public class DateNotifier : IExposable
	{
		// Token: 0x0400082C RID: 2092
		private Season lastSeason = Season.Undefined;

		// Token: 0x06000C78 RID: 3192 RVA: 0x0006EBFD File Offset: 0x0006CFFD
		public void ExposeData()
		{
			Scribe_Values.Look<Season>(ref this.lastSeason, "lastSeason", Season.Undefined, false);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0006EC14 File Offset: 0x0006D014
		public void DateNotifierTick()
		{
			Map map = this.FindPlayerHomeWithMinTimezone();
			float latitude = (map == null) ? 0f : Find.WorldGrid.LongLatOf(map.Tile).y;
			float longitude = (map == null) ? 0f : Find.WorldGrid.LongLatOf(map.Tile).x;
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, latitude, longitude);
			if (season != this.lastSeason && (this.lastSeason == Season.Undefined || season != this.lastSeason.GetPreviousSeason()))
			{
				if (this.lastSeason != Season.Undefined && this.AnyPlayerHomeSeasonsAreMeaningful())
				{
					if (GenDate.YearsPassed == 0 && season == Season.Summer && this.AnyPlayerHomeAvgTempIsLowInWinter())
					{
						Find.LetterStack.ReceiveLetter("LetterLabelFirstSummerWarning".Translate(), "FirstSummerWarning".Translate(), LetterDefOf.NeutralEvent, null);
					}
					else if (GenDate.DaysPassed > 5)
					{
						Messages.Message("MessageSeasonBegun".Translate(new object[]
						{
							season.Label()
						}).CapitalizeFirst(), MessageTypeDefOf.NeutralEvent, true);
					}
				}
				this.lastSeason = season;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0006ED5C File Offset: 0x0006D15C
		private Map FindPlayerHomeWithMinTimezone()
		{
			List<Map> maps = Find.Maps;
			Map map = null;
			int num = -1;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					int num2 = GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(maps[i].Tile).x);
					if (map == null || num2 < num)
					{
						map = maps[i];
						num = num2;
					}
				}
			}
			return map;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0006EDF0 File Offset: 0x0006D1F0
		private bool AnyPlayerHomeSeasonsAreMeaningful()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapTemperature.LocalSeasonsAreMeaningful())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0006EE54 File Offset: 0x0006D254
		private bool AnyPlayerHomeAvgTempIsLowInWinter()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && GenTemperature.AverageTemperatureAtTileForTwelfth(maps[i].Tile, Season.Winter.GetMiddleTwelfth(Find.WorldGrid.LongLatOf(maps[i].Tile).y)) < 8f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
