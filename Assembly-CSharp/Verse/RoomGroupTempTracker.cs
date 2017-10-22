using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public sealed class RoomGroupTempTracker
	{
		private RoomGroup roomGroup;

		private float temperatureInt;

		private List<IntVec3> equalizeCells = new List<IntVec3>();

		private float noRoofCoverage;

		private float thickRoofCoverage;

		private int cycleIndex = 0;

		private const float ThinRoofEqualizeRate = 5E-05f;

		private const float NoRoofEqualizeRate = 0.0007f;

		private const float DeepEqualizeFractionPerTick = 5E-05f;

		private static int debugGetFrame = -999;

		private static float debugWallEq;

		private Map Map
		{
			get
			{
				return this.roomGroup.Map;
			}
		}

		private float ThinRoofCoverage
		{
			get
			{
				return (float)(1.0 - (this.thickRoofCoverage + this.noRoofCoverage));
			}
		}

		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -270f, 2000f);
			}
		}

		public RoomGroupTempTracker(RoomGroup roomGroup, Map map)
		{
			this.roomGroup = roomGroup;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.roomGroup.RoomCount != 0)
			{
				Map map = this.Map;
				if (!this.roomGroup.UsesOutdoorTemperature)
				{
					int num = 0;
					foreach (IntVec3 cell in this.roomGroup.Cells)
					{
						RoofDef roof = cell.GetRoof(map);
						if (roof == null)
						{
							this.noRoofCoverage += 1f;
						}
						else if (roof.isThickRoof)
						{
							this.thickRoofCoverage += 1f;
						}
						num++;
					}
					this.thickRoofCoverage /= (float)num;
					this.noRoofCoverage /= (float)num;
					foreach (IntVec3 cell2 in this.roomGroup.Cells)
					{
						for (int i = 0; i < 4; i++)
						{
							IntVec3 intVec = cell2 + GenAdj.CardinalDirections[i];
							IntVec3 intVec2 = cell2 + GenAdj.CardinalDirections[i] * 2;
							if (intVec.InBounds(map))
							{
								Region region = intVec.GetRegion(map, RegionType.Set_Passable);
								if (region != null)
								{
									if (region.type == RegionType.Portal)
									{
										bool flag = false;
										for (int j = 0; j < region.links.Count; j++)
										{
											Region regionA = region.links[j].RegionA;
											Region regionB = region.links[j].RegionB;
											if (regionA.Room.Group != this.roomGroup && regionA.portal == null)
											{
												flag = true;
												break;
											}
											if (regionB.Room.Group != this.roomGroup && regionB.portal == null)
											{
												flag = true;
												break;
											}
										}
										if (!flag)
											goto IL_024f;
									}
									continue;
								}
							}
							goto IL_024f;
							IL_024f:
							if (intVec2.InBounds(map))
							{
								RoomGroup roomGroup = intVec2.GetRoomGroup(map);
								if (roomGroup != this.roomGroup)
								{
									bool flag2 = false;
									for (int k = 0; k < 4; k++)
									{
										IntVec3 loc = intVec2 + GenAdj.CardinalDirections[k];
										if (loc.GetRoomGroup(map) == this.roomGroup)
										{
											flag2 = true;
											break;
										}
									}
									if (!flag2)
									{
										this.equalizeCells.Add(intVec2);
									}
								}
							}
						}
					}
					this.equalizeCells.Shuffle();
				}
			}
		}

		public void EqualizeTemperature()
		{
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
			}
			else
			{
				if (this.roomGroup.RoomCount != 0 && this.roomGroup.Rooms[0].RegionType == RegionType.Portal)
					return;
				float num = this.ThinRoofEqualizationTempChangePerInterval();
				float num2 = this.NoRoofEqualizationTempChangePerInterval();
				float num3 = this.WallEqualizationTempChangePerInterval();
				float num4 = this.DeepEqualizationTempChangePerInterval();
				this.Temperature += num + num2 + num3 + num4;
			}
		}

		private float WallEqualizationTempChangePerInterval()
		{
			float result;
			if (this.equalizeCells.Count == 0)
			{
				result = 0f;
			}
			else
			{
				float num = 0f;
				int num2 = Mathf.CeilToInt((float)((float)this.equalizeCells.Count * 0.20000000298023224));
				for (int num3 = 0; num3 < num2; num3++)
				{
					this.cycleIndex++;
					int index = this.cycleIndex % this.equalizeCells.Count;
					float num4 = default(float);
					num = ((!GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num4)) ? (num + (Mathf.Lerp(this.Temperature, this.Map.mapTemperature.OutdoorTemp, 0.5f) - this.Temperature)) : (num + (num4 - this.Temperature)));
				}
				float num5 = num / (float)num2;
				float num6 = num5 * (float)this.equalizeCells.Count;
				result = (float)(num6 * 120.0 * 0.00016999999934341758 / (float)this.roomGroup.CellCount);
			}
			return result;
		}

		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			return (float)((!(Mathf.Abs(num) < 100.0)) ? (Mathf.Sign(num) * 100.0 + 5.0 * (num - Mathf.Sign(num) * 100.0)) : num);
		}

		private float ThinRoofEqualizationTempChangePerInterval()
		{
			float result;
			if (this.ThinRoofCoverage < 0.0010000000474974513)
			{
				result = 0f;
			}
			else
			{
				float num = this.TempDiffFromOutdoorsAdjusted();
				float num2 = (float)(num * this.ThinRoofCoverage * 4.9999998736893758E-05);
				num2 = (result = (float)(num2 * 120.0));
			}
			return result;
		}

		private float NoRoofEqualizationTempChangePerInterval()
		{
			float result;
			if (this.noRoofCoverage < 0.0010000000474974513)
			{
				result = 0f;
			}
			else
			{
				float num = this.TempDiffFromOutdoorsAdjusted();
				float num2 = (float)(num * this.noRoofCoverage * 0.000699999975040555);
				num2 = (result = (float)(num2 * 120.0));
			}
			return result;
		}

		private float DeepEqualizationTempChangePerInterval()
		{
			float result;
			if (this.thickRoofCoverage < 0.0010000000474974513)
			{
				result = 0f;
			}
			else
			{
				float num = (float)(15.0 - this.temperatureInt);
				if (num > 0.0)
				{
					result = 0f;
				}
				else
				{
					float num2 = (float)(num * this.thickRoofCoverage * 4.9999998736893758E-05);
					num2 = (result = (float)(num2 * 120.0));
				}
			}
			return result;
		}

		public void DebugDraw()
		{
			foreach (IntVec3 equalizeCell in this.equalizeCells)
			{
				CellRenderer.RenderCell(equalizeCell, 0.5f);
			}
		}

		internal string DebugString()
		{
			string result;
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				result = "uses outdoor temperature";
			}
			else
			{
				if (Time.frameCount > RoomGroupTempTracker.debugGetFrame + 120)
				{
					RoomGroupTempTracker.debugWallEq = 0f;
					for (int i = 0; i < 40; i++)
					{
						RoomGroupTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
					}
					RoomGroupTempTracker.debugWallEq /= 40f;
					RoomGroupTempTracker.debugGetFrame = Time.frameCount;
				}
				result = "  thick roof coverage: " + this.thickRoofCoverage.ToStringPercent("F0") + "\n  thin roof coverage: " + this.ThinRoofCoverage.ToStringPercent("F0") + "\n  no roof coverage: " + this.noRoofCoverage.ToStringPercent("F0") + "\n\n  wall equalization: " + RoomGroupTempTracker.debugWallEq.ToStringTemperatureOffset("F3") + "\n  thin roof equalization: " + this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3") + "\n  no roof equalization: " + this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3") + "\n  deep equalization: " + this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3") + "\n\n  temp diff from outdoors, adjusted: " + this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3") + "\n  tempChange e=20 targ= 200C: " + GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First(), this.roomGroup.Map, 20f, 200f) + "\n  tempChange e=20 targ=-200C: " + GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First(), this.roomGroup.Map, 20f, -200f) + "\n  equalize interval ticks: " + 120 + "\n  equalize cells count:" + this.equalizeCells.Count;
			}
			return result;
		}
	}
}
