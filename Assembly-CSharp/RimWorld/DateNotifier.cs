using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class DateNotifier : IExposable
	{
		private Season lastSeason = Season.Undefined;

		public void ExposeData()
		{
			Scribe_Values.Look<Season>(ref this.lastSeason, "lastSeason", Season.Undefined, false);
		}

		public void DateNotifierTick()
		{
			Map map = this.FindPlayerHomeWithMinTimezone();
			double num;
			if (map != null)
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
				num = vector.y;
			}
			else
			{
				num = 0.0;
			}
			float latitude = (float)num;
			double num2;
			if (map != null)
			{
				Vector2 vector2 = Find.WorldGrid.LongLatOf(map.Tile);
				num2 = vector2.x;
			}
			else
			{
				num2 = 0.0;
			}
			float longitude = (float)num2;
			Season season = GenDate.Season(Find.TickManager.TicksAbs, latitude, longitude);
			if (season != this.lastSeason)
			{
				if (this.lastSeason != 0 && season == this.lastSeason.GetPreviousSeason())
					return;
				if (this.lastSeason != 0 && this.AnyPlayerHomeSeasonsAreMeaningful())
				{
					if (GenDate.YearsPassed == 0 && season == Season.Summer && this.AnyPlayerHomeAvgTempIsLowInWinter())
					{
						Find.LetterStack.ReceiveLetter("LetterLabelFirstSummerWarning".Translate(), "FirstSummerWarning".Translate(), LetterDefOf.NeutralEvent, (string)null);
					}
					else if (GenDate.DaysPassed > 5)
					{
						Messages.Message("MessageSeasonBegun".Translate(season.Label()).CapitalizeFirst(), MessageTypeDefOf.NeutralEvent);
					}
				}
				this.lastSeason = season;
			}
		}

		private Map FindPlayerHomeWithMinTimezone()
		{
			List<Map> maps = Find.Maps;
			Map map = null;
			int num = -1;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					Vector2 vector = Find.WorldGrid.LongLatOf(maps[i].Tile);
					int num2 = GenDate.TimeZoneAt(vector.x);
					if (map == null || num2 < num)
					{
						map = maps[i];
						num = num2;
					}
				}
			}
			return map;
		}

		private bool AnyPlayerHomeSeasonsAreMeaningful()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < maps.Count)
				{
					if (maps[num].IsPlayerHome && maps[num].mapTemperature.LocalSeasonsAreMeaningful())
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		private bool AnyPlayerHomeAvgTempIsLowInWinter()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < maps.Count)
				{
					if (maps[num].IsPlayerHome)
					{
						int tile = maps[num].Tile;
						Vector2 vector = Find.WorldGrid.LongLatOf(maps[num].Tile);
						if (GenTemperature.AverageTemperatureAtTileForTwelfth(tile, Season.Winter.GetMiddleTwelfth(vector.y)) < 8.0)
						{
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
