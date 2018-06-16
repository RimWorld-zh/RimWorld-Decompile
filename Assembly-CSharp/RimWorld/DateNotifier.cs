using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F0 RID: 752
	public class DateNotifier : IExposable
	{
		// Token: 0x06000C74 RID: 3188 RVA: 0x0006E9F9 File Offset: 0x0006CDF9
		public void ExposeData()
		{
			Scribe_Values.Look<Season>(ref this.lastSeason, "lastSeason", Season.Undefined, false);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0006EA10 File Offset: 0x0006CE10
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

		// Token: 0x06000C76 RID: 3190 RVA: 0x0006EB58 File Offset: 0x0006CF58
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

		// Token: 0x06000C77 RID: 3191 RVA: 0x0006EBEC File Offset: 0x0006CFEC
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

		// Token: 0x06000C78 RID: 3192 RVA: 0x0006EC50 File Offset: 0x0006D050
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

		// Token: 0x0400082A RID: 2090
		private Season lastSeason = Season.Undefined;
	}
}
