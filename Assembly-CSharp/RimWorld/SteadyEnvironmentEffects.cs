using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020009CC RID: 2508
	public class SteadyEnvironmentEffects
	{
		// Token: 0x06003825 RID: 14373 RVA: 0x001DE84F File Offset: 0x001DCC4F
		public SteadyEnvironmentEffects(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x001DE860 File Offset: 0x001DCC60
		public void SteadyEnvironmentEffectsTick()
		{
			Profiler.BeginSample("Init");
			if ((float)Find.TickManager.TicksGame % 97f == 0f && Rand.Chance(0.02f))
			{
				this.RollForRainFire();
			}
			this.outdoorMeltAmount = this.MeltAmountAt(this.map.mapTemperature.OutdoorTemp);
			this.snowRate = this.map.weatherManager.SnowRate;
			this.rainRate = this.map.weatherManager.RainRate;
			this.deteriorationRate = Mathf.Lerp(1f, 5f, this.rainRate);
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			Profiler.EndSample();
			Profiler.BeginSample("DoCells");
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.cycleIndex = 0;
				}
				Profiler.BeginSample("Get cell");
				IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				Profiler.EndSample();
				Profiler.BeginSample("Affect cell");
				this.DoCellSteadyEffects(c);
				Profiler.EndSample();
				this.cycleIndex++;
			}
			Profiler.EndSample();
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x001DE9B4 File Offset: 0x001DCDB4
		private void DoCellSteadyEffects(IntVec3 c)
		{
			Room room = c.GetRoom(this.map, RegionType.Set_All);
			bool flag = this.map.roofGrid.Roofed(c);
			bool flag2 = room != null && room.UsesOutdoorTemperature;
			if (room == null || flag2)
			{
				Profiler.BeginSample("Roomless or Outdoors");
				if (this.outdoorMeltAmount > 0f)
				{
					this.map.snowGrid.AddDepth(c, -this.outdoorMeltAmount);
				}
				if (!flag)
				{
					if (this.snowRate > 0.001f)
					{
						this.AddFallenSnowAt(c, 0.046f * this.map.weatherManager.SnowRate);
					}
				}
				Profiler.EndSample();
			}
			if (room != null)
			{
				Profiler.BeginSample("Deteriorate");
				bool protectedByEdifice = SteadyEnvironmentEffects.ProtectedByEdifice(c, this.map);
				TerrainDef terrain = c.GetTerrain(this.map);
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					Filth filth = thing as Filth;
					if (filth != null)
					{
						if (!flag && thing.def.filth.rainWashes && Rand.Chance(this.rainRate))
						{
							filth.ThinFilth();
						}
					}
					else
					{
						this.TryDoDeteriorate(thing, flag, flag2, protectedByEdifice, terrain);
					}
				}
				Profiler.EndSample();
				if (!flag2)
				{
					Profiler.BeginSample("Indoors");
					float temperature = room.Temperature;
					if (temperature > 0f)
					{
						float num = this.MeltAmountAt(temperature);
						if (num > 0f)
						{
							this.map.snowGrid.AddDepth(c, -num);
						}
						if (room.RegionType.Passable() && temperature > SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.min)
						{
							float value = Rand.Value;
							if (value < SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.InverseLerpThroughRange(temperature) * 0.7f)
							{
								if (Rand.Chance(FireUtility.ChanceToStartFireIn(c, this.map)))
								{
									FireUtility.TryStartFireIn(c, this.map, 0.1f);
								}
							}
							if (value < 0.33f)
							{
								MoteMaker.ThrowHeatGlow(c, this.map, 2.3f);
							}
						}
					}
					Profiler.EndSample();
				}
			}
			Profiler.BeginSample("GameConditions");
			this.map.gameConditionManager.DoSteadyEffects(c, this.map);
			Profiler.EndSample();
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x001DEC3C File Offset: 0x001DD03C
		private static bool ProtectedByEdifice(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building != null && edifice.def.building.preventDeteriorationOnTop;
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x001DEC8C File Offset: 0x001DD08C
		private float MeltAmountAt(float temperature)
		{
			float result;
			if (temperature < 0f)
			{
				result = 0f;
			}
			else if (temperature < 10f)
			{
				result = temperature * temperature * 0.0058f * 0.1f;
			}
			else
			{
				result = temperature * 0.0058f;
			}
			return result;
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x001DECE4 File Offset: 0x001DD0E4
		public void AddFallenSnowAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.039999999105930328, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.snowGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x001DED74 File Offset: 0x001DD174
		public static float FinalDeteriorationRate(Thing t, List<string> reasons = null)
		{
			float result;
			if (t.Spawned)
			{
				Room room = t.GetRoom(RegionType.Set_Passable);
				result = SteadyEnvironmentEffects.FinalDeteriorationRate(t, t.Position.Roofed(t.Map), room != null && room.UsesOutdoorTemperature, SteadyEnvironmentEffects.ProtectedByEdifice(t.Position, t.Map), t.Position.GetTerrain(t.Map), reasons);
			}
			else
			{
				result = SteadyEnvironmentEffects.FinalDeteriorationRate(t, false, false, false, null, reasons);
			}
			return result;
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x001DEDFC File Offset: 0x001DD1FC
		public static float FinalDeteriorationRate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, bool protectedByEdifice, TerrainDef terrain, List<string> reasons = null)
		{
			float result;
			if (!t.def.CanEverDeteriorate)
			{
				result = 0f;
			}
			else if (protectedByEdifice)
			{
				result = 0f;
			}
			else
			{
				float statValue = t.GetStatValue(StatDefOf.DeteriorationRate, true);
				if (statValue <= 0f)
				{
					result = 0f;
				}
				else
				{
					float num = 0f;
					if (!roofed)
					{
						num += 0.5f;
						if (reasons != null)
						{
							reasons.Add("DeterioratingUnroofed".Translate());
						}
					}
					if (roomUsesOutdoorTemperature)
					{
						num += 0.5f;
						if (reasons != null)
						{
							reasons.Add("DeterioratingOutdoors".Translate());
						}
					}
					if (terrain != null && terrain.extraDeteriorationFactor != 0f)
					{
						num += terrain.extraDeteriorationFactor;
						if (reasons != null)
						{
							reasons.Add(terrain.label);
						}
					}
					if (num <= 0f)
					{
						result = 0f;
					}
					else
					{
						result = statValue * num;
					}
				}
			}
			return result;
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x001DEF08 File Offset: 0x001DD308
		private void TryDoDeteriorate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, bool protectedByEdifice, TerrainDef terrain)
		{
			Corpse corpse = t as Corpse;
			if (corpse != null)
			{
				if (corpse.InnerPawn.apparel != null)
				{
					List<Apparel> wornApparel = corpse.InnerPawn.apparel.WornApparel;
					for (int i = 0; i < wornApparel.Count; i++)
					{
						this.TryDoDeteriorate(wornApparel[i], roofed, roomUsesOutdoorTemperature, protectedByEdifice, terrain);
					}
				}
			}
			float num = SteadyEnvironmentEffects.FinalDeteriorationRate(t, roofed, roomUsesOutdoorTemperature, protectedByEdifice, terrain, null);
			if (num >= 0.001f)
			{
				float chance = this.deteriorationRate * num / 36f;
				if (Rand.Chance(chance))
				{
					t.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x001DEFD0 File Offset: 0x001DD3D0
		private void RollForRainFire()
		{
			float chance = 0.2f * (float)this.map.listerBuildings.allBuildingsColonistElecFire.Count * this.map.weatherManager.RainRate;
			if (Rand.Chance(chance))
			{
				Building building = this.map.listerBuildings.allBuildingsColonistElecFire.RandomElement<Building>();
				if (!this.map.roofGrid.Roofed(building.Position))
				{
					ShortCircuitUtility.TryShortCircuitInRain(building);
				}
			}
		}

		// Token: 0x040023D2 RID: 9170
		private Map map;

		// Token: 0x040023D3 RID: 9171
		private ModuleBase snowNoise;

		// Token: 0x040023D4 RID: 9172
		private int cycleIndex;

		// Token: 0x040023D5 RID: 9173
		private float outdoorMeltAmount;

		// Token: 0x040023D6 RID: 9174
		private float snowRate;

		// Token: 0x040023D7 RID: 9175
		private float rainRate;

		// Token: 0x040023D8 RID: 9176
		private float deteriorationRate;

		// Token: 0x040023D9 RID: 9177
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x040023DA RID: 9178
		private const float RainFireCheckInterval = 97f;

		// Token: 0x040023DB RID: 9179
		private const float RainFireChanceOverall = 0.02f;

		// Token: 0x040023DC RID: 9180
		private const float RainFireChancePerBuilding = 0.2f;

		// Token: 0x040023DD RID: 9181
		private const float SnowFallRateFactor = 0.046f;

		// Token: 0x040023DE RID: 9182
		private const float SnowMeltRateFactor = 0.0058f;

		// Token: 0x040023DF RID: 9183
		private static readonly FloatRange AutoIgnitionTemperatureRange = new FloatRange(240f, 1000f);

		// Token: 0x040023E0 RID: 9184
		private const float AutoIgnitionChanceFactor = 0.7f;

		// Token: 0x040023E1 RID: 9185
		private const float FireGlowRate = 0.33f;
	}
}
