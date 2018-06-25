using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CA8 RID: 3240
	public static class GenTemperature
	{
		// Token: 0x04003071 RID: 12401
		public static readonly Color ColorSpotHot = new Color(1f, 0f, 0f, 0.6f);

		// Token: 0x04003072 RID: 12402
		public static readonly Color ColorSpotCold = new Color(0f, 0f, 1f, 0.6f);

		// Token: 0x04003073 RID: 12403
		public static readonly Color ColorRoomHot = new Color(1f, 0f, 0f, 0.3f);

		// Token: 0x04003074 RID: 12404
		public static readonly Color ColorRoomCold = new Color(0f, 0f, 1f, 0.3f);

		// Token: 0x04003075 RID: 12405
		private static List<RoomGroup> neighRoomGroups = new List<RoomGroup>();

		// Token: 0x04003076 RID: 12406
		private static RoomGroup[] beqRoomGroups = new RoomGroup[4];

		// Token: 0x06004750 RID: 18256 RVA: 0x0025A224 File Offset: 0x00258624
		public static float AverageTemperatureAtTileForTwelfth(int tile, Twelfth twelfth)
		{
			int num = 30000;
			int num2 = 300000 * (int)twelfth;
			float num3 = 0f;
			for (int i = 0; i < 120; i++)
			{
				int absTick = num2 + num + Mathf.RoundToInt((float)i / 120f * 300000f);
				num3 += GenTemperature.GetTemperatureFromSeasonAtTile(absTick, tile);
			}
			return num3 / 120f;
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x0025A290 File Offset: 0x00258690
		public static float MinTemperatureAtTile(int tile)
		{
			float num = float.MaxValue;
			for (int i = 0; i < 3600000; i += 26999)
			{
				num = Mathf.Min(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0025A2D8 File Offset: 0x002586D8
		public static float MaxTemperatureAtTile(int tile)
		{
			float num = float.MinValue;
			for (int i = 0; i < 3600000; i += 26999)
			{
				num = Mathf.Max(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x0025A320 File Offset: 0x00258720
		public static FloatRange ComfortableTemperatureRange(this Pawn p)
		{
			return new FloatRange(p.GetStatValue(StatDefOf.ComfyTemperatureMin, true), p.GetStatValue(StatDefOf.ComfyTemperatureMax, true));
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x0025A354 File Offset: 0x00258754
		public static FloatRange ComfortableTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = new FloatRange(raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null), raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null));
			if (apparel != null)
			{
				result.min -= apparel.Sum((ThingStuffPair x) => x.InsulationCold);
				result.max += apparel.Sum((ThingStuffPair x) => x.InsulationHeat);
			}
			return result;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x0025A3F4 File Offset: 0x002587F4
		public static FloatRange SafeTemperatureRange(this Pawn p)
		{
			FloatRange result = p.ComfortableTemperatureRange();
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x0025A438 File Offset: 0x00258838
		public static FloatRange SafeTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = GenTemperature.ComfortableTemperatureRange(raceDef, apparel);
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x0025A47C File Offset: 0x0025887C
		public static float GetTemperatureForCell(IntVec3 c, Map map)
		{
			float result;
			GenTemperature.TryGetTemperatureForCell(c, map, out result);
			return result;
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x0025A49C File Offset: 0x0025889C
		public static bool TryGetTemperatureForCell(IntVec3 c, Map map, out float tempResult)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Got temperature for null map.", false);
				tempResult = 21f;
				result = true;
			}
			else if (!c.InBounds(map))
			{
				tempResult = 21f;
				result = false;
			}
			else if (GenTemperature.TryGetDirectAirTemperatureForCell(c, map, out tempResult))
			{
				result = true;
			}
			else
			{
				List<Thing> list = map.thingGrid.ThingsListAtFast(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.passability == Traversability.Impassable)
					{
						return GenTemperature.TryGetAirTemperatureAroundThing(list[i], out tempResult);
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x0025A554 File Offset: 0x00258954
		public static bool TryGetDirectAirTemperatureForCell(IntVec3 c, Map map, out float temperature)
		{
			bool result;
			if (!c.InBounds(map))
			{
				temperature = 21f;
				result = false;
			}
			else
			{
				RoomGroup roomGroup = c.GetRoomGroup(map);
				if (roomGroup == null)
				{
					temperature = 21f;
					result = false;
				}
				else
				{
					temperature = roomGroup.Temperature;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x0025A5AC File Offset: 0x002589AC
		public static bool TryGetAirTemperatureAroundThing(Thing t, out float temperature)
		{
			float num = 0f;
			int num2 = 0;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(t);
			for (int i = 0; i < list.Count; i++)
			{
				float num3;
				if (list[i].InBounds(t.Map) && GenTemperature.TryGetDirectAirTemperatureForCell(list[i], t.Map, out num3))
				{
					num += num3;
					num2++;
				}
			}
			bool result;
			if (num2 > 0)
			{
				temperature = num / (float)num2;
				result = true;
			}
			else
			{
				temperature = 21f;
				result = false;
			}
			return result;
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x0025A64C File Offset: 0x00258A4C
		public static float OffsetFromSunCycle(int absTick, int tile)
		{
			float num = GenDate.DayPercent((long)absTick, Find.WorldGrid.LongLatOf(tile).x);
			float f = 6.28318548f * (num + 0.32f);
			return Mathf.Cos(f) * 7f;
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x0025A698 File Offset: 0x00258A98
		public static float OffsetFromSeasonCycle(int absTick, int tile)
		{
			float num = (float)(absTick / 60000 % 60) / 60f;
			float f = 6.28318548f * (num - Season.Winter.GetMiddleTwelfth(0f).GetBeginningYearPct());
			return Mathf.Cos(f) * -GenTemperature.SeasonalShiftAmplitudeAt(tile);
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x0025A6E8 File Offset: 0x00258AE8
		public static float GetTemperatureFromSeasonAtTile(int absTick, int tile)
		{
			if (absTick == 0)
			{
				absTick = 1;
			}
			Tile tile2 = Find.WorldGrid[tile];
			return tile2.temperature + GenTemperature.OffsetFromSeasonCycle(absTick, tile);
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x0025A720 File Offset: 0x00258B20
		public static float GetTemperatureAtTile(int tile)
		{
			Map map = Current.Game.FindMap(tile);
			float result;
			if (map != null)
			{
				result = map.mapTemperature.OutdoorTemp;
			}
			else
			{
				result = GenTemperature.GetTemperatureFromSeasonAtTile(GenTicks.TicksAbs, tile);
			}
			return result;
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x0025A764 File Offset: 0x00258B64
		public static float SeasonalShiftAmplitudeAt(int tile)
		{
			float result;
			if (Find.WorldGrid.LongLatOf(tile).y >= 0f)
			{
				result = TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			else
			{
				result = -TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			return result;
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x0025A7CC File Offset: 0x00258BCC
		public static List<Twelfth> TwelfthsInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			List<Twelfth> twelfths = new List<Twelfth>();
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					twelfths.Add((Twelfth)i);
				}
			}
			List<Twelfth> twelfths2;
			if (twelfths.Count <= 1 || twelfths.Count == 12)
			{
				twelfths2 = twelfths;
			}
			else
			{
				if (twelfths.Contains(Twelfth.Twelfth) && twelfths.Contains(Twelfth.First))
				{
					Twelfth twelfth = twelfths.First((Twelfth m) => !twelfths.Contains((Twelfth)(m - Twelfth.Second)));
					List<Twelfth> list = new List<Twelfth>();
					for (int j = (int)twelfth; j < 12; j++)
					{
						if (!twelfths.Contains((Twelfth)j))
						{
							break;
						}
						list.Add((Twelfth)j);
					}
					for (int k = 0; k < 12; k++)
					{
						if (!twelfths.Contains((Twelfth)k))
						{
							break;
						}
						list.Add((Twelfth)k);
					}
				}
				twelfths2 = twelfths;
			}
			return twelfths2;
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x0025A91C File Offset: 0x00258D1C
		public static Twelfth EarliestTwelfthInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					Twelfth result;
					if (i != 0)
					{
						result = (Twelfth)i;
					}
					else
					{
						Twelfth twelfth = (Twelfth)i;
						for (int j = 0; j < 12; j++)
						{
							float num2 = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth.PreviousTwelfth());
							if (num2 < minTemp || num2 > maxTemp)
							{
								return twelfth;
							}
							twelfth = twelfth.PreviousTwelfth();
						}
						result = (Twelfth)i;
					}
					return result;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x0025A9C0 File Offset: 0x00258DC0
		public static bool PushHeat(IntVec3 c, Map map, float energy)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Added heat to null map.", false);
				result = false;
			}
			else
			{
				RoomGroup roomGroup = c.GetRoomGroup(map);
				if (roomGroup != null)
				{
					result = roomGroup.PushHeat(energy);
				}
				else
				{
					GenTemperature.neighRoomGroups.Clear();
					for (int i = 0; i < 8; i++)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						if (intVec.InBounds(map))
						{
							roomGroup = intVec.GetRoomGroup(map);
							if (roomGroup != null)
							{
								GenTemperature.neighRoomGroups.Add(roomGroup);
							}
						}
					}
					float energy2 = energy / (float)GenTemperature.neighRoomGroups.Count;
					for (int j = 0; j < GenTemperature.neighRoomGroups.Count; j++)
					{
						GenTemperature.neighRoomGroups[j].PushHeat(energy2);
					}
					bool flag = GenTemperature.neighRoomGroups.Count > 0;
					GenTemperature.neighRoomGroups.Clear();
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x0025AAC8 File Offset: 0x00258EC8
		public static void PushHeat(Thing t, float energy)
		{
			IntVec3 c;
			if (t.GetRoomGroup() != null)
			{
				GenTemperature.PushHeat(t.Position, t.Map, energy);
			}
			else if (GenAdj.TryFindRandomAdjacentCell8WayWithRoomGroup(t, out c))
			{
				GenTemperature.PushHeat(c, t.Map, energy);
			}
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x0025AB18 File Offset: 0x00258F18
		public static float ControlTemperatureTempChange(IntVec3 cell, Map map, float energyLimit, float targetTemperature)
		{
			RoomGroup roomGroup = cell.GetRoomGroup(map);
			float result;
			if (roomGroup == null || roomGroup.UsesOutdoorTemperature)
			{
				result = 0f;
			}
			else
			{
				float b = energyLimit / (float)roomGroup.CellCount;
				float a = targetTemperature - roomGroup.Temperature;
				float num;
				if (energyLimit > 0f)
				{
					num = Mathf.Min(a, b);
					num = Mathf.Max(num, 0f);
				}
				else
				{
					num = Mathf.Max(a, b);
					num = Mathf.Min(num, 0f);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0025ABB0 File Offset: 0x00258FB0
		public static void EqualizeTemperaturesThroughBuilding(Building b, float rate, bool twoWay)
		{
			int num = 0;
			float num2 = 0f;
			if (twoWay)
			{
				for (int i = 0; i < 2; i++)
				{
					IntVec3 intVec = (i != 0) ? (b.Position - b.Rotation.FacingCell) : (b.Position + b.Rotation.FacingCell);
					if (intVec.InBounds(b.Map))
					{
						RoomGroup roomGroup = intVec.GetRoomGroup(b.Map);
						if (roomGroup != null)
						{
							num2 += roomGroup.Temperature;
							GenTemperature.beqRoomGroups[num] = roomGroup;
							num++;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 intVec2 = b.Position + GenAdj.CardinalDirections[j];
					if (intVec2.InBounds(b.Map))
					{
						RoomGroup roomGroup2 = intVec2.GetRoomGroup(b.Map);
						if (roomGroup2 != null)
						{
							num2 += roomGroup2.Temperature;
							GenTemperature.beqRoomGroups[num] = roomGroup2;
							num++;
						}
					}
				}
			}
			if (num != 0)
			{
				float num3 = num2 / (float)num;
				RoomGroup roomGroup3 = b.GetRoomGroup();
				if (roomGroup3 != null)
				{
					roomGroup3.Temperature = num3;
				}
				if (num != 1)
				{
					float num4 = 1f;
					for (int k = 0; k < num; k++)
					{
						if (!GenTemperature.beqRoomGroups[k].UsesOutdoorTemperature)
						{
							float temperature = GenTemperature.beqRoomGroups[k].Temperature;
							float num5 = num3 - temperature;
							float num6 = num5 * rate;
							float num7 = num6 / (float)GenTemperature.beqRoomGroups[k].CellCount;
							float num8 = GenTemperature.beqRoomGroups[k].Temperature + num7;
							if (num6 > 0f && num8 > num3)
							{
								num8 = num3;
							}
							else if (num6 < 0f && num8 < num3)
							{
								num8 = num3;
							}
							float num9 = Mathf.Abs((num8 - temperature) * (float)GenTemperature.beqRoomGroups[k].CellCount / num6);
							if (num9 < num4)
							{
								num4 = num9;
							}
						}
					}
					for (int l = 0; l < num; l++)
					{
						if (!GenTemperature.beqRoomGroups[l].UsesOutdoorTemperature)
						{
							float temperature2 = GenTemperature.beqRoomGroups[l].Temperature;
							float num10 = num3 - temperature2;
							float num11 = num10 * rate * num4;
							float num12 = num11 / (float)GenTemperature.beqRoomGroups[l].CellCount;
							GenTemperature.beqRoomGroups[l].Temperature += num12;
						}
					}
					for (int m = 0; m < GenTemperature.beqRoomGroups.Length; m++)
					{
						GenTemperature.beqRoomGroups[m] = null;
					}
				}
			}
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0025AE98 File Offset: 0x00259298
		public static float RotRateAtTemperature(float temperature)
		{
			float result;
			if (temperature < 0f)
			{
				result = 0f;
			}
			else if (temperature >= 10f)
			{
				result = 1f;
			}
			else
			{
				result = temperature / 10f;
			}
			return result;
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0025AEE0 File Offset: 0x002592E0
		public static bool FactionOwnsPassableRoomInTemperatureRange(Faction faction, FloatRange tempRange, Map map)
		{
			bool result;
			if (faction == Faction.OfPlayer)
			{
				List<Room> allRooms = map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					Room room = allRooms[i];
					if (room.RegionType.Passable() && !room.Fogged && tempRange.Includes(room.Temperature))
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0025AF6C File Offset: 0x0025936C
		public static string GetAverageTemperatureLabel(int tile)
		{
			return Find.WorldGrid[tile].temperature.ToStringTemperature("F1") + " " + string.Format("({0} {1} {2})", GenTemperature.MinTemperatureAtTile(tile).ToStringTemperature("F0"), "RangeTo".Translate(), GenTemperature.MaxTemperatureAtTile(tile).ToStringTemperature("F0"));
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0025AFDC File Offset: 0x002593DC
		public static float CelsiusTo(float temp, TemperatureDisplayMode oldMode)
		{
			float result;
			if (oldMode != TemperatureDisplayMode.Celsius)
			{
				if (oldMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (oldMode != TemperatureDisplayMode.Kelvin)
					{
						throw new InvalidOperationException();
					}
					result = temp + 273.15f;
				}
				else
				{
					result = temp * 1.8f + 32f;
				}
			}
			else
			{
				result = temp;
			}
			return result;
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0025B034 File Offset: 0x00259434
		public static float CelsiusToOffset(float temp, TemperatureDisplayMode oldMode)
		{
			float result;
			if (oldMode != TemperatureDisplayMode.Celsius)
			{
				if (oldMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (oldMode != TemperatureDisplayMode.Kelvin)
					{
						throw new InvalidOperationException();
					}
					result = temp;
				}
				else
				{
					result = temp * 1.8f;
				}
			}
			else
			{
				result = temp;
			}
			return result;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0025B080 File Offset: 0x00259480
		public static float ConvertTemperatureOffset(float temp, TemperatureDisplayMode oldMode, TemperatureDisplayMode newMode)
		{
			if (oldMode != TemperatureDisplayMode.Celsius)
			{
				if (oldMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (oldMode != TemperatureDisplayMode.Kelvin)
					{
					}
				}
				else
				{
					temp /= 1.8f;
				}
			}
			if (newMode != TemperatureDisplayMode.Celsius)
			{
				if (newMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (newMode != TemperatureDisplayMode.Kelvin)
					{
					}
				}
				else
				{
					temp *= 1.8f;
				}
			}
			return temp;
		}
	}
}
