using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class MapTemperature
	{
		private Map map;

		private HashSet<RoomGroup> fastProcessedRoomGroups = new HashSet<RoomGroup>();

		public float OutdoorTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		public float SeasonalTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		public MapTemperature(Map map)
		{
			this.map = map;
		}

		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7)
			{
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
			}
		}

		public bool SeasonAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAcceptableFor(this.map.Tile, animalRace);
		}

		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.OutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		public bool LocalSeasonsAreMeaningful()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 12; i++)
			{
				float num = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, (Twelfth)(byte)i);
				if (num > 0.0)
				{
					flag2 = true;
				}
				if (num < 0.0)
				{
					flag = true;
				}
			}
			return flag2 && flag;
		}

		public void DebugLogTemps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			double num;
			if (Find.VisibleMap != null)
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(Find.VisibleMap.Tile);
				num = vector.y;
			}
			else
			{
				num = 0.0;
			}
			float num2 = (float)num;
			stringBuilder.AppendLine("Latitude " + num2);
			stringBuilder.AppendLine("-----Temperature for each hour this day------");
			stringBuilder.AppendLine("Hour    Temp    SunEffect");
			int num3 = Find.TickManager.TicksAbs - Find.TickManager.TicksAbs % 60000;
			for (int i = 0; i < 24; i++)
			{
				int absTick = num3 + i * 2500;
				stringBuilder.Append(i.ToString().PadRight(5));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick).ToString("F2").PadRight(8));
				stringBuilder.Append(GenTemperature.OffsetFromSunCycle(absTick, this.map.Tile).ToString("F2"));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each twelfth this year------");
			for (int j = 0; j < 12; j++)
			{
				Twelfth twelfth = (Twelfth)(byte)j;
				float num4 = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, twelfth);
				stringBuilder.AppendLine(twelfth.GetQuadrum() + "/" + twelfth.GetSeason(num2) + " - " + ((Enum)(object)twelfth).ToString() + " " + num4.ToString("F2"));
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
				int absTick2 = (int)((float)(k * 60000) + 15000.0);
				int absTick3 = (int)((float)(k * 60000) + 45000.0);
				stringBuilder.Append(k.ToString().PadRight(8));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick2).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick3).ToString("F2").PadRight(11));
				stringBuilder.Append(GenTemperature.OffsetFromSeasonCycle(absTick3, this.map.Tile).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OffsetFromDailyRandomVariation(this.map.Tile, absTick3).ToString("F2"));
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
