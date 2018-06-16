using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000CAA RID: 3242
	public class MapTemperature
	{
		// Token: 0x06004765 RID: 18277 RVA: 0x00259A76 File Offset: 0x00257E76
		public MapTemperature(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004766 RID: 18278 RVA: 0x00259A94 File Offset: 0x00257E94
		public float OutdoorTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06004767 RID: 18279 RVA: 0x00259AC4 File Offset: 0x00257EC4
		public float SeasonalTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x00259AF4 File Offset: 0x00257EF4
		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7 || DebugSettings.fastEcology)
			{
				Profiler.BeginSample("Rooms equalize temperatures");
				this.fastProcessedRoomGroups.Clear();
				List<Room> allRooms = this.map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					RoomGroup group = allRooms[i].Group;
					if (!this.fastProcessedRoomGroups.Contains(group))
					{
						group.TempTracker.EqualizeTemperature();
						this.fastProcessedRoomGroups.Add(group);
					}
				}
				this.fastProcessedRoomGroups.Clear();
				Profiler.EndSample();
			}
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x00259BA8 File Offset: 0x00257FA8
		public bool SeasonAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x00259BD8 File Offset: 0x00257FD8
		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.OutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00259C08 File Offset: 0x00258008
		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x00259C38 File Offset: 0x00258038
		public bool LocalSeasonsAreMeaningful()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 12; i++)
			{
				float num = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, (Twelfth)i);
				if (num > 0f)
				{
					flag2 = true;
				}
				if (num < 0f)
				{
					flag = true;
				}
			}
			return flag2 && flag;
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x00259CAC File Offset: 0x002580AC
		public void DebugLogTemps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (Find.CurrentMap == null) ? 0f : Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).y;
			stringBuilder.AppendLine("Latitude " + num);
			stringBuilder.AppendLine("-----Temperature for each hour this day------");
			stringBuilder.AppendLine("Hour    Temp    SunEffect");
			int num2 = Find.TickManager.TicksAbs - Find.TickManager.TicksAbs % 60000;
			for (int i = 0; i < 24; i++)
			{
				int absTick = num2 + i * 2500;
				stringBuilder.Append(i.ToString().PadRight(5));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick).ToString("F2").PadRight(8));
				stringBuilder.Append(GenTemperature.OffsetFromSunCycle(absTick, this.map.Tile).ToString("F2"));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each twelfth this year------");
			for (int j = 0; j < 12; j++)
			{
				Twelfth twelfth = (Twelfth)j;
				float num3 = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, twelfth);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					twelfth.GetQuadrum(),
					"/",
					SeasonUtility.GetReportedSeason(twelfth.GetMiddleYearPct(), num),
					" - ",
					twelfth.ToString(),
					" ",
					num3.ToString("F2")
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each day this year------");
			stringBuilder.AppendLine("Tile avg: " + this.map.TileInfo.temperature + "°C");
			stringBuilder.AppendLine("Seasonal shift: " + GenTemperature.SeasonalShiftAmplitudeAt(this.map.Tile));
			stringBuilder.AppendLine("Equatorial distance: " + Find.WorldGrid.DistanceFromEquatorNormalized(this.map.Tile));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Day  Lo   Hi   OffsetFromSeason RandomDailyVariation");
			for (int k = 0; k < 60; k++)
			{
				int absTick2 = (int)((float)(k * 60000) + 15000f);
				int absTick3 = (int)((float)(k * 60000) + 45000f);
				stringBuilder.Append(k.ToString().PadRight(8));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick2).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick3).ToString("F2").PadRight(11));
				stringBuilder.Append(GenTemperature.OffsetFromSeasonCycle(absTick3, this.map.Tile).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OffsetFromDailyRandomVariation(this.map.Tile, absTick3).ToString("F2"));
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04003069 RID: 12393
		private Map map;

		// Token: 0x0400306A RID: 12394
		private HashSet<RoomGroup> fastProcessedRoomGroups = new HashSet<RoomGroup>();
	}
}
