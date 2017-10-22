using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public class SteadyAtmosphereEffects
	{
		private const float MapFractionCheckPerTick = 0.0006f;

		private const float RainFireCheckInterval = 97f;

		private const float RainFireChanceOverall = 0.02f;

		private const float RainFireChancePerBuilding = 0.2f;

		private const float SnowFallRateFactor = 0.046f;

		private const float SnowMeltRateFactor = 0.0058f;

		private const float AutoIgnitionChanceFactor = 0.7f;

		private const float FireGlowRate = 0.33f;

		private Map map;

		private ModuleBase snowNoise;

		private int cycleIndex;

		private float outdoorMeltAmount;

		private float snowRate;

		private float rainRate;

		private float deteriorationRate;

		private static readonly FloatRange AutoIgnitionTemperatureRange = new FloatRange(240f, 1000f);

		public SteadyAtmosphereEffects(Map map)
		{
			this.map = map;
		}

		public void SteadyAtmosphereEffectsTick()
		{
			if ((float)Find.TickManager.TicksGame % 97.0 == 0.0 && Rand.Value < 0.019999999552965164)
			{
				this.RollForRainFire();
			}
			this.outdoorMeltAmount = this.MeltAmountAt(this.map.mapTemperature.OutdoorTemp);
			this.snowRate = this.map.weatherManager.SnowRate;
			this.rainRate = this.map.weatherManager.RainRate;
			this.deteriorationRate = Mathf.Lerp(1f, 5f, this.rainRate);
			int num = Mathf.RoundToInt((float)((float)this.map.Area * 0.00060000002849847078));
			int area = this.map.Area;
			for (int num2 = 0; num2 < num; num2++)
			{
				if (this.cycleIndex >= area)
				{
					this.cycleIndex = 0;
				}
				IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.DoCellSteadyEffects(c);
				this.cycleIndex++;
			}
		}

		private void DoCellSteadyEffects(IntVec3 c)
		{
			Room room = c.GetRoom(this.map, RegionType.Set_All);
			bool flag = this.map.roofGrid.Roofed(c);
			bool flag2 = room != null && room.UsesOutdoorTemperature;
			if (room == null || flag2)
			{
				if (this.outdoorMeltAmount > 0.0)
				{
					this.map.snowGrid.AddDepth(c, (float)(0.0 - this.outdoorMeltAmount));
				}
				if (!flag && this.snowRate > 0.0010000000474974513)
				{
					this.AddFallenSnowAt(c, (float)(0.046000000089406967 * this.map.weatherManager.SnowRate));
				}
			}
			if (room != null)
			{
				if (flag2)
				{
					if (!flag)
					{
						List<Thing> thingList = c.GetThingList(this.map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							Filth filth = thing as Filth;
							if (filth != null)
							{
								if (thing.def.filth.rainWashes && Rand.Value < this.rainRate)
								{
									((Filth)thing).ThinFilth();
								}
							}
							else
							{
								Corpse corpse = thing as Corpse;
								if (corpse != null && corpse.InnerPawn.apparel != null)
								{
									List<Apparel> wornApparel = corpse.InnerPawn.apparel.WornApparel;
									for (int j = 0; j < wornApparel.Count; j++)
									{
										this.TryDoDeteriorate(wornApparel[j], c, false);
									}
								}
								this.TryDoDeteriorate(thing, c, true);
							}
						}
					}
				}
				else
				{
					float temperature = room.Temperature;
					if (temperature > 0.0)
					{
						float num = this.MeltAmountAt(temperature);
						if (num > 0.0)
						{
							this.map.snowGrid.AddDepth(c, (float)(0.0 - num));
						}
						if (room.RegionType.Passable())
						{
							float num2 = temperature;
							FloatRange autoIgnitionTemperatureRange = SteadyAtmosphereEffects.AutoIgnitionTemperatureRange;
							if (num2 > autoIgnitionTemperatureRange.min)
							{
								float value = Rand.Value;
								if (value < SteadyAtmosphereEffects.AutoIgnitionTemperatureRange.InverseLerpThroughRange(temperature) * 0.699999988079071 && Rand.Chance(FireUtility.ChanceToStartFireIn(c, this.map)))
								{
									FireUtility.TryStartFireIn(c, this.map, 0.1f);
								}
								if (value < 0.33000001311302185)
								{
									MoteMaker.ThrowHeatGlow(c, this.map, 2.3f);
								}
							}
						}
					}
				}
			}
			List<GameCondition> activeConditions = this.map.gameConditionManager.ActiveConditions;
			for (int k = 0; k < activeConditions.Count; k++)
			{
				activeConditions[k].DoCellSteadyEffects(c);
			}
		}

		public static bool InDeterioratingPosition(Thing t)
		{
			if (t.Position.Roofed(t.Map))
			{
				return false;
			}
			if (SteadyAtmosphereEffects.ProtectedByEdifice(t.Position, t.Map))
			{
				return false;
			}
			return true;
		}

		private static bool ProtectedByEdifice(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.building != null && edifice.def.building.preventDeterioration)
			{
				return true;
			}
			return false;
		}

		private float MeltAmountAt(float temperature)
		{
			if (temperature < 0.0)
			{
				return 0f;
			}
			if (temperature < 10.0)
			{
				return (float)(temperature * temperature * 0.0057999999262392521 * 0.10000000149011612);
			}
			return (float)(temperature * 0.0057999999262392521);
		}

		public void AddFallenSnowAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.039999999105930328, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float value = this.snowNoise.GetValue(c);
			value = (float)(value + 1.0);
			value = (float)(value * 0.5);
			if (value < 0.5)
			{
				value = 0.5f;
			}
			float depthToAdd = baseAmount * value;
			this.map.snowGrid.AddDepth(c, depthToAdd);
		}

		public static float FinalDeteriorationRate(Thing t)
		{
			if (!t.def.CanEverDeteriorate)
			{
				return 0f;
			}
			return t.GetStatValue(StatDefOf.DeteriorationRate, true);
		}

		private void TryDoDeteriorate(Thing t, IntVec3 c, bool checkEdifice)
		{
			float num = SteadyAtmosphereEffects.FinalDeteriorationRate(t);
			if (!(num < 0.0010000000474974513))
			{
				float num2 = (float)(this.deteriorationRate * num / 36.0);
				if (Rand.Value < num2)
				{
					if (checkEdifice && SteadyAtmosphereEffects.ProtectedByEdifice(c, t.Map))
						return;
					t.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			}
		}

		private void RollForRainFire()
		{
			float num = (float)(0.20000000298023224 * (float)this.map.listerBuildings.allBuildingsColonistElecFire.Count * this.map.weatherManager.RainRate);
			if (!(Rand.Value > num))
			{
				Building building = this.map.listerBuildings.allBuildingsColonistElecFire.RandomElement();
				if (!this.map.roofGrid.Roofed(building.Position))
				{
					ThingWithComps thingWithComps = building;
					CompPowerTrader comp = thingWithComps.GetComp<CompPowerTrader>();
					if (comp == null || !comp.PowerOn || !comp.Props.shortCircuitInRain)
					{
						if (thingWithComps.GetComp<CompPowerBattery>() == null)
							return;
						if (!(thingWithComps.GetComp<CompPowerBattery>().StoredEnergy > 100.0))
							return;
					}
					GenExplosion.DoExplosion(building.OccupiedRect().RandomCell, this.map, 1.9f, DamageDefOf.Flame, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
					Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), "ShortCircuitRain".Translate(building.Label), LetterDefOf.BadUrgent, new TargetInfo(building.Position, building.Map, false), (string)null);
				}
			}
		}
	}
}
