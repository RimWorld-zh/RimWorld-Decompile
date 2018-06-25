using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C9A RID: 3226
	public sealed class RoomGroupTempTracker
	{
		// Token: 0x04003048 RID: 12360
		private RoomGroup roomGroup;

		// Token: 0x04003049 RID: 12361
		private float temperatureInt;

		// Token: 0x0400304A RID: 12362
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x0400304B RID: 12363
		private float noRoofCoverage;

		// Token: 0x0400304C RID: 12364
		private float thickRoofCoverage;

		// Token: 0x0400304D RID: 12365
		private int cycleIndex = 0;

		// Token: 0x0400304E RID: 12366
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x0400304F RID: 12367
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x04003050 RID: 12368
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04003051 RID: 12369
		private static int debugGetFrame = -999;

		// Token: 0x04003052 RID: 12370
		private static float debugWallEq;

		// Token: 0x06004705 RID: 18181 RVA: 0x002575C0 File Offset: 0x002559C0
		public RoomGroupTempTracker(RoomGroup roomGroup, Map map)
		{
			this.roomGroup = roomGroup;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004706 RID: 18182 RVA: 0x002575F4 File Offset: 0x002559F4
		private Map Map
		{
			get
			{
				return this.roomGroup.Map;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x00257614 File Offset: 0x00255A14
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004708 RID: 18184 RVA: 0x0025763C File Offset: 0x00255A3C
		// (set) Token: 0x06004709 RID: 18185 RVA: 0x00257657 File Offset: 0x00255A57
		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -273.15f, 2000f);
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x00257670 File Offset: 0x00255A70
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00257679 File Offset: 0x00255A79
		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x002576A0 File Offset: 0x00255AA0
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
					foreach (IntVec3 c in this.roomGroup.Cells)
					{
						RoofDef roof = c.GetRoof(map);
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
					foreach (IntVec3 a in this.roomGroup.Cells)
					{
						int i = 0;
						while (i < 4)
						{
							IntVec3 intVec = a + GenAdj.CardinalDirections[i];
							IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
							if (intVec.InBounds(map))
							{
								Region region = intVec.GetRegion(map, RegionType.Set_Passable);
								if (region != null)
								{
									if (region.type != RegionType.Portal)
									{
										goto IL_2DD;
									}
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
									if (flag)
									{
										goto IL_2DD;
									}
								}
								goto IL_24F;
							}
							goto IL_24F;
							IL_2DD:
							i++;
							continue;
							IL_24F:
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
							goto IL_2DD;
						}
					}
					this.equalizeCells.Shuffle<IntVec3>();
				}
			}
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x002579FC File Offset: 0x00255DFC
		public void EqualizeTemperature()
		{
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
			}
			else if (this.roomGroup.RoomCount == 0 || this.roomGroup.Rooms[0].RegionType != RegionType.Portal)
			{
				float num = this.ThinRoofEqualizationTempChangePerInterval();
				float num2 = this.NoRoofEqualizationTempChangePerInterval();
				float num3 = this.WallEqualizationTempChangePerInterval();
				float num4 = this.DeepEqualizationTempChangePerInterval();
				this.Temperature += num + num2 + num3 + num4;
			}
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00257A98 File Offset: 0x00255E98
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
				int num2 = Mathf.CeilToInt((float)this.equalizeCells.Count * 0.2f);
				for (int i = 0; i < num2; i++)
				{
					this.cycleIndex++;
					int index = this.cycleIndex % this.equalizeCells.Count;
					float num3;
					if (GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num3))
					{
						num += num3 - this.Temperature;
					}
					else
					{
						num += Mathf.Lerp(this.Temperature, this.Map.mapTemperature.OutdoorTemp, 0.5f) - this.Temperature;
					}
				}
				float num4 = num / (float)num2;
				float num5 = num4 * (float)this.equalizeCells.Count;
				result = num5 * 120f * 0.00017f / (float)this.roomGroup.CellCount;
			}
			return result;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00257BAC File Offset: 0x00255FAC
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			float result;
			if (Mathf.Abs(num) < 100f)
			{
				result = num;
			}
			else
			{
				result = Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
			}
			return result;
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00257C14 File Offset: 0x00256014
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			float result;
			if (this.ThinRoofCoverage < 0.001f)
			{
				result = 0f;
			}
			else
			{
				float num = this.TempDiffFromOutdoorsAdjusted();
				float num2 = num * this.ThinRoofCoverage * 5E-05f;
				num2 *= 120f;
				result = num2;
			}
			return result;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x00257C64 File Offset: 0x00256064
		private float NoRoofEqualizationTempChangePerInterval()
		{
			float result;
			if (this.noRoofCoverage < 0.001f)
			{
				result = 0f;
			}
			else
			{
				float num = this.TempDiffFromOutdoorsAdjusted();
				float num2 = num * this.noRoofCoverage * 0.0007f;
				num2 *= 120f;
				result = num2;
			}
			return result;
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00257CB4 File Offset: 0x002560B4
		private float DeepEqualizationTempChangePerInterval()
		{
			float result;
			if (this.thickRoofCoverage < 0.001f)
			{
				result = 0f;
			}
			else
			{
				float num = 15f - this.temperatureInt;
				if (num > 0f)
				{
					result = 0f;
				}
				else
				{
					float num2 = num * this.thickRoofCoverage * 5E-05f;
					num2 *= 120f;
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00257D20 File Offset: 0x00256120
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x00257D84 File Offset: 0x00256184
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
				result = string.Concat(new object[]
				{
					"  thick roof coverage: ",
					this.thickRoofCoverage.ToStringPercent("F0"),
					"\n  thin roof coverage: ",
					this.ThinRoofCoverage.ToStringPercent("F0"),
					"\n  no roof coverage: ",
					this.noRoofCoverage.ToStringPercent("F0"),
					"\n\n  wall equalization: ",
					RoomGroupTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
					"\n  thin roof equalization: ",
					this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
					"\n  no roof equalization: ",
					this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
					"\n  deep equalization: ",
					this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
					"\n\n  temp diff from outdoors, adjusted: ",
					this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
					"\n  tempChange e=20 targ= 200C: ",
					GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First<IntVec3>(), this.roomGroup.Map, 20f, 200f),
					"\n  tempChange e=20 targ=-200C: ",
					GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First<IntVec3>(), this.roomGroup.Map, 20f, -200f),
					"\n  equalize interval ticks: ",
					120,
					"\n  equalize cells count:",
					this.equalizeCells.Count
				});
			}
			return result;
		}
	}
}
