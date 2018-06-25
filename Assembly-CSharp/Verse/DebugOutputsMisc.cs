using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E1E RID: 3614
	[HasDebugOutput]
	internal static class DebugOutputsMisc
	{
		// Token: 0x06005312 RID: 21266 RVA: 0x002AA92C File Offset: 0x002A8D2C
		[DebugOutput]
		public static void MiningResourceGeneration()
		{
			Func<ThingDef, ThingDef> mineable = delegate(ThingDef d)
			{
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.mineableThing == d)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			};
			Func<ThingDef, float> mineableCommonality = delegate(ThingDef d)
			{
				float result;
				if (mineable(d) != null)
				{
					result = mineable(d).building.mineableScatterCommonality;
				}
				else
				{
					result = 0f;
				}
				return result;
			};
			Func<ThingDef, IntRange> mineableLumpSizeRange = delegate(ThingDef d)
			{
				IntRange result;
				if (mineable(d) != null)
				{
					result = mineable(d).building.mineableScatterLumpSizeRange;
				}
				else
				{
					result = IntRange.zero;
				}
				return result;
			};
			Func<ThingDef, float> mineableYield = delegate(ThingDef d)
			{
				float result;
				if (mineable(d) != null)
				{
					result = (float)mineable(d).building.mineableYield;
				}
				else
				{
					result = 0f;
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.deepCommonality > 0f || mineableCommonality(d) > 0f
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("deep\ncommonality", (ThingDef d) => d.deepCommonality.ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("deep\nlump size", (ThingDef d) => d.deepLumpSizeRange);
			array[4] = new TableDataGetter<ThingDef>("deep\nyield", (ThingDef d) => d.deepCountPerCell);
			array[5] = new TableDataGetter<ThingDef>("mineable\ncommonality", (ThingDef d) => mineableCommonality(d).ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("mineable\nlump size", (ThingDef d) => mineableLumpSizeRange(d));
			array[7] = new TableDataGetter<ThingDef>("mineable\nyield", (ThingDef d) => mineableYield(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x002AAAD8 File Offset: 0x002A8ED8
		[DebugOutput]
		public static void DefaultStuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.MadeFromStuff && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("default stuff", (ThingDef d) => GenStuff.DefaultStuffFor(d).defName);
			array[3] = new TableDataGetter<ThingDef>("stuff categories", (ThingDef d) => (from c in d.stuffCategories
			select c.defName).ToCommaList(false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x002AABC0 File Offset: 0x002A8FC0
		[DebugOutput]
		public static void Beauties()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>()).Where(delegate(BuildableDef d)
			{
				ThingDef thingDef = d as ThingDef;
				bool result;
				if (thingDef != null)
				{
					result = BeautyUtility.BeautyRelevant(thingDef.category);
				}
				else
				{
					result = (d is TerrainDef);
				}
				return result;
			})
			orderby (int)d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[6];
			array[0] = new TableDataGetter<BuildableDef>("category", (BuildableDef d) => (!(d is ThingDef)) ? "Terrain" : ((ThingDef)d).category.ToString());
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("beauty", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<BuildableDef>("market value", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[4] = new TableDataGetter<BuildableDef>("work to produce", (BuildableDef d) => DebugOutputsEconomy.WorkToProduceBest(d).ToString());
			array[5] = new TableDataGetter<BuildableDef>("beauty per market value", (BuildableDef d) => (d.GetStatValueAbstract(StatDefOf.Beauty, null) <= 0f) ? "" : (d.GetStatValueAbstract(StatDefOf.Beauty, null) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5"));
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x002AAD34 File Offset: 0x002A9134
		[DebugOutput]
		public static void ThingsPowerAndHeat()
		{
			Func<ThingDef, CompProperties_HeatPusher> heatPusher = delegate(ThingDef d)
			{
				CompProperties_HeatPusher result;
				if (d.comps == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < d.comps.Count; i++)
					{
						CompProperties_HeatPusher compProperties_HeatPusher = d.comps[i] as CompProperties_HeatPusher;
						if (compProperties_HeatPusher != null)
						{
							return compProperties_HeatPusher;
						}
					}
					result = null;
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Building || d.GetCompProperties<CompProperties_Power>() != null || heatPusher(d) != null) && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[10];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base\npower consumption", (ThingDef d) => (d.GetCompProperties<CompProperties_Power>() != null) ? d.GetCompProperties<CompProperties_Power>().basePowerConsumption.ToString() : "");
			array[2] = new TableDataGetter<ThingDef>("short circuit\nin rain", (ThingDef d) => (d.GetCompProperties<CompProperties_Power>() != null) ? ((!d.GetCompProperties<CompProperties_Power>().shortCircuitInRain) ? "" : "rainfire") : "");
			array[3] = new TableDataGetter<ThingDef>("transmits\npower", (ThingDef d) => (d.GetCompProperties<CompProperties_Power>() != null) ? ((!d.GetCompProperties<CompProperties_Power>().transmitsPower) ? "" : "transmit") : "");
			array[4] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue);
			array[5] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, false));
			array[6] = new TableDataGetter<ThingDef>("heat pusher\ncompClass", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).compClass.ToString() : "");
			array[7] = new TableDataGetter<ThingDef>("heat pusher\nheat per sec", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).heatPerSecond.ToString() : "");
			array[8] = new TableDataGetter<ThingDef>("heat pusher\nmin temp", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).heatPushMinTemperature.ToStringTemperature("F1") : "");
			array[9] = new TableDataGetter<ThingDef>("heat pusher\nmax temp", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).heatPushMaxTemperature.ToStringTemperature("F1") : "");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x002AAEF0 File Offset: 0x002A92F0
		[DebugOutput]
		public static void FoodPoisonChances()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsIngestible
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category);
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("food poison chance", delegate(ThingDef d)
			{
				CompProperties_FoodPoisonable compProperties = d.GetCompProperties<CompProperties_FoodPoisonable>();
				string result;
				if (compProperties != null)
				{
					result = "poisonable by cook";
				}
				else
				{
					float statValueAbstract = d.GetStatValueAbstract(StatDefOf.FoodPoisonChanceFixedHuman, null);
					if (statValueAbstract != 0f)
					{
						result = statValueAbstract.ToStringPercent();
					}
					else
					{
						result = "";
					}
				}
				return result;
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x002AAFB0 File Offset: 0x002A93B0
		[DebugOutput]
		public static void TechLevels()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building || d.category == ThingCategory.Item
			where !d.IsFrame && (d.building == null || !d.building.isNaturalRock)
			orderby (int)d.techLevel descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("tech level", (ThingDef d) => d.techLevel.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x002AB0B4 File Offset: 0x002A94B4
		[DebugOutput]
		public static void Stuffs()
		{
			Func<ThingDef, StatDef, string> getStatFactor = delegate(ThingDef d, StatDef stat)
			{
				string result;
				if (d.stuffProps.statFactors == null)
				{
					result = "";
				}
				else
				{
					StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
					if (statModifier == null)
					{
						result = "";
					}
					else
					{
						result = stat.ValueToString(statModifier.value, ToStringNumberSense.Absolute);
					}
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[23];
			array[0] = new TableDataGetter<ThingDef>("fabric", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric).ToStringCheckBlank());
			array[1] = new TableDataGetter<ThingDef>("leather", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("metal", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("stony", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony).ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("woody", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody).ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[6] = new TableDataGetter<ThingDef>("base market value", (ThingDef d) => d.BaseMarketValue.ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("sharp damage", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("blunt damage", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("armor sharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp, null).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("armor blunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt, null).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("armor heat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat, null).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("ins. cold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold, null).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("ins. heat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat, null).ToString("F2"));
			array[14] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("fac-Flammability", (ThingDef d) => getStatFactor(d, StatDefOf.Flammability));
			array[16] = new TableDataGetter<ThingDef>("fac-WorkToMake", (ThingDef d) => getStatFactor(d, StatDefOf.WorkToMake));
			array[17] = new TableDataGetter<ThingDef>("fac-WorkToBuild", (ThingDef d) => getStatFactor(d, StatDefOf.WorkToBuild));
			array[18] = new TableDataGetter<ThingDef>("fac-MaxHp", (ThingDef d) => getStatFactor(d, StatDefOf.MaxHitPoints));
			array[19] = new TableDataGetter<ThingDef>("fac-Beauty", (ThingDef d) => getStatFactor(d, StatDefOf.Beauty));
			array[20] = new TableDataGetter<ThingDef>("fac-Doorspeed", (ThingDef d) => getStatFactor(d, StatDefOf.DoorOpenSpeed));
			array[21] = new TableDataGetter<ThingDef>("fac-MeleeCooldown", (ThingDef d) => getStatFactor(d, StatDefOf.MeleeWeapon_CooldownMultiplier));
			array[22] = new TableDataGetter<ThingDef>("fac-MeleeDamage", (ThingDef d) => getStatFactor(d, StatDefOf.MeleeWeapon_DamageMultiplier));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x002AB48C File Offset: 0x002A988C
		[DebugOutput]
		public static void Drugs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("pleasure", (ThingDef d) => (!d.IsPleasureDrug) ? "" : "pleasure");
			array[2] = new TableDataGetter<ThingDef>("non-medical", (ThingDef d) => (!d.IsNonMedicalDrug) ? "" : "non-medical");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x002AB54C File Offset: 0x002A994C
		[DebugOutput]
		public static void Medicines()
		{
			List<float> list = new List<float>();
			list.Add(0.3f);
			list.AddRange(from d in DefDatabase<ThingDef>.AllDefs
			where typeof(Medicine).IsAssignableFrom(d.thingClass)
			select d.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			SkillNeed_Direct skillNeed_Direct = (SkillNeed_Direct)StatDefOf.MedicalTendQuality.skillNeedFactors[0];
			TableDataGetter<float>[] array = new TableDataGetter<float>[21];
			array[0] = new TableDataGetter<float>("potency", (float p) => p.ToStringPercent());
			for (int i = 0; i < 20; i++)
			{
				float factor = skillNeed_Direct.valuesPerLevel[i];
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (float p) => (p * factor).ToStringPercent());
			}
			DebugTables.MakeTablesDialog<float>(list, array);
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x002AB664 File Offset: 0x002A9A64
		[DebugOutput]
		public static void ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracy;
			Func<int, float, int, float> accAtDistance = delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					float value = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets.First((StatModifier so) => so.stat == stat).value;
					num += value;
				}
				foreach (SkillNeed skillNeed in stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeed as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			};
			List<int> list = new List<int>();
			for (int i = 0; i <= 20; i++)
			{
				list.Add(i);
			}
			IEnumerable<int> dataSources = list;
			TableDataGetter<int>[] array = new TableDataGetter<int>[18];
			array[0] = new TableDataGetter<int>("No trait skill", (int lev) => lev.ToString());
			array[1] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2"));
			array[2] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2"));
			array[3] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2"));
			array[4] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2"));
			array[5] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2"));
			array[6] = new TableDataGetter<int>("Careful shooter skill", (int lev) => lev.ToString());
			array[7] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2"));
			array[8] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2"));
			array[9] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2"));
			array[10] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2"));
			array[11] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2"));
			array[12] = new TableDataGetter<int>("Trigger-happy skill", (int lev) => lev.ToString());
			array[13] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2"));
			array[14] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2"));
			array[15] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2"));
			array[16] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2"));
			array[17] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2"));
			DebugTables.MakeTablesDialog<int>(dataSources, array);
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x002AB8C0 File Offset: 0x002A9CC0
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void TemperatureData()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemps();
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x002AB8D2 File Offset: 0x002A9CD2
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WeatherChances()
		{
			Find.CurrentMap.weatherDecider.LogWeatherChances();
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x002AB8E4 File Offset: 0x002A9CE4
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void CelestialGlow()
		{
			GenCelestial.LogSunGlowForYear();
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x002AB8EC File Offset: 0x002A9CEC
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void FallColor()
		{
			GenPlant.LogFallColorForYear();
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x002AB8F4 File Offset: 0x002A9CF4
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void PawnsListAllOnMap()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x002AB906 File Offset: 0x002A9D06
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WindSpeeds()
		{
			Find.CurrentMap.windManager.LogWindSpeeds();
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x002AB918 File Offset: 0x002A9D18
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void MapPawnsList()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x002AB92A File Offset: 0x002A9D2A
		[DebugOutput]
		public static void Lords()
		{
			Find.CurrentMap.lordManager.LogLords();
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x002AB93C File Offset: 0x002A9D3C
		[DebugOutput]
		public static void DamageTest()
		{
			ThingDef thingDef = ThingDef.Named("Bullet_BoltActionRifle");
			PawnKindDef pawnKindDef = PawnKindDef.Named("Slave");
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			DamageInfo dinfo = new DamageInfo(thingDef.projectile.damageDef, (float)thingDef.projectile.DamageAmount, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
			int num = 0;
			int num2 = 0;
			DefMap<BodyPartDef, int> defMap = new DefMap<BodyPartDef, int>();
			for (int i = 0; i < 500; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				List<BodyPartDef> list = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList<BodyPartDef>();
				for (int j = 0; j < 2; j++)
				{
					pawn.TakeDamage(dinfo);
					if (pawn.Dead)
					{
						num++;
						break;
					}
				}
				List<BodyPartDef> list2 = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList<BodyPartDef>();
				if (list2.Count > list.Count)
				{
					num2++;
					foreach (BodyPartDef bodyPartDef in list2)
					{
						DefMap<BodyPartDef, int> defMap2;
						BodyPartDef def;
						(defMap2 = defMap)[def = bodyPartDef] = defMap2[def] + 1;
					}
					foreach (BodyPartDef bodyPartDef2 in list)
					{
						DefMap<BodyPartDef, int> defMap2;
						BodyPartDef def2;
						(defMap2 = defMap)[def2 = bodyPartDef2] = defMap2[def2] - 1;
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Damage test");
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Hit ",
				500,
				" ",
				pawnKindDef.label,
				"s with ",
				2,
				"x ",
				thingDef.label,
				" (",
				thingDef.projectile.DamageAmount,
				" damage) each. Results:"
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Killed: ",
				num,
				" / ",
				500,
				" (",
				((float)num / 500f).ToStringPercent(),
				")"
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Part losers: ",
				num2,
				" / ",
				500,
				" (",
				((float)num2 / 500f).ToStringPercent(),
				")"
			}));
			stringBuilder.AppendLine("Parts lost:");
			foreach (BodyPartDef bodyPartDef3 in DefDatabase<BodyPartDef>.AllDefs)
			{
				if (defMap[bodyPartDef3] > 0)
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"   ",
						bodyPartDef3.label,
						": ",
						defMap[bodyPartDef3]
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x002ABDAC File Offset: 0x002AA1AC
		[DebugOutput]
		public static void BodyPartTagGroups()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					using (IEnumerator<BodyPartTagDef> enumerator2 = (from elem in localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags)
					orderby elem
					select elem).Distinct<BodyPartTagDef>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BodyPartTagDef tag = enumerator2.Current;
							stringBuilder.AppendLine(tag.defName);
							foreach (BodyPartRecord bodyPartRecord in from part in localBd.AllParts
							where part.def.tags.Contains(tag)
							orderby part.def.defName
							select part)
							{
								stringBuilder.AppendLine("  " + bodyPartRecord.def.defName);
							}
						}
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x002ABE54 File Offset: 0x002AA254
		[DebugOutput]
		public static void MinifiableTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.Minifiable)
				{
					stringBuilder.Append(thingDef.defName);
					if (!thingDef.tradeTags.NullOrEmpty<string>())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(thingDef.tradeTags.ToCommaList(false));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x002ABF10 File Offset: 0x002AA310
		[DebugOutput]
		public static void ListSolidBackstories()
		{
			IEnumerable<string> enumerable = SolidBioDatabase.allBios.SelectMany((PawnBio bio) => bio.adulthood.spawnCategories).Distinct<string>();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (string catInner2 in enumerable)
			{
				string catInner = catInner2;
				FloatMenuOption item = new FloatMenuOption(catInner, delegate()
				{
					IEnumerable<PawnBio> enumerable2 = from b in SolidBioDatabase.allBios
					where b.adulthood.spawnCategories.Contains(catInner)
					select b;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Backstories with category: ",
						catInner,
						" (",
						enumerable2.Count<PawnBio>(),
						")"
					}));
					foreach (PawnBio pawnBio in enumerable2)
					{
						stringBuilder.AppendLine(pawnBio.ToString());
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x002ABFE0 File Offset: 0x002AA3E0
		[DebugOutput]
		public static void ThingSetMakerTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Action<ThingSetMakerParams> generate = delegate(ThingSetMakerParams parms)
					{
						StringBuilder stringBuilder = new StringBuilder();
						float num = 0f;
						float num2 = 0f;
						for (int i = 0; i < 50; i++)
						{
							List<Thing> list3 = localDef.root.Generate(parms);
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
							}
							float num3 = 0f;
							float num4 = 0f;
							for (int j = 0; j < list3.Count; j++)
							{
								stringBuilder.AppendLine("-" + list3[j].LabelCap + " - $" + (list3[j].MarketValue * (float)list3[j].stackCount).ToString("F0"));
								num3 += list3[j].MarketValue * (float)list3[j].stackCount;
								if (!(list3[j] is Pawn))
								{
									num4 += list3[j].GetStatValue(StatDefOf.Mass, true) * (float)list3[j].stackCount;
								}
								list3[j].Destroy(DestroyMode.Vanish);
							}
							num += num3;
							num2 += num4;
							stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
							stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
						}
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.AppendLine("Default thing sets generated by: " + localDef.defName);
						string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localDef.root.fixedParams);
						stringBuilder2.AppendLine("root fixedParams: " + ((!nonNullFieldsDebugInfo.NullOrEmpty()) ? nonNullFieldsDebugInfo : "none"));
						string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
						if (!nonNullFieldsDebugInfo2.NullOrEmpty())
						{
							stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
						}
						stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
						stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
						stringBuilder2.AppendLine();
						stringBuilder2.Append(stringBuilder.ToString());
						Log.Message(stringBuilder2.ToString(), false);
					};
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.traderFaction = localF;
											obj.traderDef = localKind;
											generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}
					else
					{
						generate(localDef.debugParams);
					}
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x002AC080 File Offset: 0x002AA480
		[DebugOutput]
		public static void ThingSetMakerPossibleDefs()
		{
			Dictionary<ThingSetMakerDef, List<ThingDef>> generatableThings = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
			foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				generatableThings[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
			}
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney()));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass()));
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				list.Add(new TableDataGetter<ThingDef>(localDef.defName.Shorten(), (ThingDef d) => generatableThings[localDef].Contains(d).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && !d.IsCorpse && !d.isUnfinishedThing) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.BaseMarketValue descending
			select d, list.ToArray());
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x002AC27C File Offset: 0x002AA67C
		[DebugOutput]
		public static void ThingSetMakerSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Action<ThingSetMakerParams> generate = delegate(ThingSetMakerParams parms)
					{
						Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
						for (int i = 0; i < 500; i++)
						{
							List<Thing> list3 = localDef.root.Generate(parms);
							foreach (ThingDef thingDef in (from th in list3
							select th.GetInnerIfMinified().def).Distinct<ThingDef>())
							{
								if (!counts.ContainsKey(thingDef))
								{
									counts.Add(thingDef, 0);
								}
								Dictionary<ThingDef, int> counts2;
								ThingDef key;
								(counts2 = counts)[key = thingDef] = counts2[key] + 1;
							}
							for (int j = 0; j < list3.Count; j++)
							{
								list3[j].Destroy(DestroyMode.Vanish);
							}
						}
						IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
						where counts.ContainsKey(d)
						orderby counts[d] descending
						select d;
						TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
						array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
						array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney());
						array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
						array[3] = new TableDataGetter<ThingDef>("appearance rate in " + localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
						DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
					};
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.traderFaction = localF;
											obj.traderDef = localKind;
											generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}
					else
					{
						generate(localDef.debugParams);
					}
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x002AC31C File Offset: 0x002AA71C
		[DebugOutput]
		public static void WorkDisables()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pkInner2 in from ki in DefDatabase<PawnKindDef>.AllDefs
			where ki.RaceProps.Humanlike
			select ki)
			{
				PawnKindDef pkInner = pkInner2;
				Faction faction = FactionUtility.DefaultFactionFrom(pkInner.defaultFactionType);
				FloatMenuOption item = new FloatMenuOption(pkInner.defName, delegate()
				{
					int num = 500;
					DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
					for (int i = 0; i < num; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(pkInner, faction);
						foreach (WorkTypeDef workTypeDef in pawn.story.DisabledWorkTypes)
						{
							DefMap<WorkTypeDef, int> defMap2;
							WorkTypeDef def;
							(defMap2 = defMap)[def = workTypeDef] = defMap2[def] + 1;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Generated ",
						num,
						" pawns of kind ",
						pkInner.defName,
						" on faction ",
						faction.ToStringSafe<Faction>()
					}));
					stringBuilder.AppendLine("Work types disabled:");
					foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefs)
					{
						if (workTypeDef2.workTags != WorkTags.None)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								workTypeDef2.defName,
								": ",
								defMap[workTypeDef2],
								"        ",
								((float)defMap[workTypeDef2] / (float)num).ToStringPercent()
							}));
						}
					}
					IEnumerable<Backstory> enumerable = BackstoryDatabase.allBackstories.Select((KeyValuePair<string, Backstory> kvp) => kvp.Value);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
					using (IEnumerator<WorkTypeDef> enumerator4 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							WorkTypeDef wt = enumerator4.Current;
							int num2 = 0;
							foreach (Backstory backstory in enumerable)
							{
								if (backstory.DisabledWorkTypes.Any((WorkTypeDef wd) => wt == wd))
								{
									num2++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								wt.defName,
								": ",
								num2,
								"     ",
								((float)num2 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
							}));
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
					IEnumerator enumerator6 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							object obj = enumerator6.Current;
							WorkTags workTags = (WorkTags)obj;
							int num3 = 0;
							foreach (Backstory backstory2 in enumerable)
							{
								if ((workTags & backstory2.workDisables) != WorkTags.None)
								{
									num3++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								workTags,
								": ",
								num3,
								"     ",
								((float)num3 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
							}));
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator6 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x002AC3FC File Offset: 0x002AA7FC
		[DebugOutput]
		public static void KeyStrings()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					KeyCode k = (KeyCode)obj;
					stringBuilder.AppendLine(k.ToString() + " - " + k.ToStringReadable());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x002AC4A0 File Offset: 0x002AA8A0
		[DebugOutput]
		public static void SocialPropernessMatters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Social-properness-matters things:");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.socialPropernessMatters)
				{
					stringBuilder.AppendLine(string.Format("  {0}", thingDef.defName));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x002AC538 File Offset: 0x002AA938
		[DebugOutput]
		public static void FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", thingDef.ingestible.preferability, thingDef.defName));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x002AC618 File Offset: 0x002AAA18
		[DebugOutput]
		public static void MapDanger()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Map danger status:");
			foreach (Map map in Find.Maps)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", map, map.dangerWatcher.DangerRating));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x002AC6B0 File Offset: 0x002AAAB0
		[DebugOutput]
		public static void DefNames()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Type> enumerator = (from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(type.Name, DebugMenuOptionMode.Action, delegate()
					{
						IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
						int num = 0;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (Def def in source.Cast<Def>())
						{
							stringBuilder.AppendLine(def.defName);
							num++;
							if (num >= 500)
							{
								Log.Message(stringBuilder.ToString(), false);
								stringBuilder = new StringBuilder();
								num = 0;
							}
						}
						Log.Message(stringBuilder.ToString(), false);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x002AC770 File Offset: 0x002AAB70
		[DebugOutput]
		public static void DefNamesAll()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (Type type in from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def)
			{
				IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
				stringBuilder.AppendLine("--    " + type.ToString());
				foreach (Def def2 in source.Cast<Def>().OrderBy((Def def) => def.defName))
				{
					stringBuilder.AppendLine(def2.defName);
					num++;
					if (num >= 500)
					{
						Log.Message(stringBuilder.ToString(), false);
						stringBuilder = new StringBuilder();
						num = 0;
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x002AC8DC File Offset: 0x002AACDC
		[DebugOutput]
		public static void DefLabels()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Type> enumerator = (from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(type.Name, DebugMenuOptionMode.Action, delegate()
					{
						IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
						int num = 0;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (Def def in source.Cast<Def>())
						{
							stringBuilder.AppendLine(def.label);
							num++;
							if (num >= 500)
							{
								Log.Message(stringBuilder.ToString(), false);
								stringBuilder = new StringBuilder();
								num = 0;
							}
						}
						Log.Message(stringBuilder.ToString(), false);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x002AC99C File Offset: 0x002AAD9C
		[DebugOutput]
		public static void Bodies()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				list.Add(new FloatMenuOption(localBd.defName, delegate()
				{
					IEnumerable<BodyPartRecord> dataSources = from d in localBd.AllParts
					orderby d.height descending
					select d;
					TableDataGetter<BodyPartRecord>[] array = new TableDataGetter<BodyPartRecord>[7];
					array[0] = new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord d) => d.def.defName);
					array[1] = new TableDataGetter<BodyPartRecord>("hitPoints\n(non-adjusted)", (BodyPartRecord d) => d.def.hitPoints);
					array[2] = new TableDataGetter<BodyPartRecord>("coverage", (BodyPartRecord d) => d.coverage.ToStringPercent());
					array[3] = new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent());
					array[4] = new TableDataGetter<BodyPartRecord>("coverageAbs", (BodyPartRecord d) => d.coverageAbs.ToStringPercent());
					array[5] = new TableDataGetter<BodyPartRecord>("depth", (BodyPartRecord d) => d.depth.ToString());
					array[6] = new TableDataGetter<BodyPartRecord>("height", (BodyPartRecord d) => d.height.ToString());
					DebugTables.MakeTablesDialog<BodyPartRecord>(dataSources, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x002ACA40 File Offset: 0x002AAE40
		[DebugOutput]
		public static void BodyParts()
		{
			IEnumerable<BodyPartDef> allDefs = DefDatabase<BodyPartDef>.AllDefs;
			TableDataGetter<BodyPartDef>[] array = new TableDataGetter<BodyPartDef>[17];
			array[0] = new TableDataGetter<BodyPartDef>("defName", (BodyPartDef d) => d.defName);
			array[1] = new TableDataGetter<BodyPartDef>("hit\npoints", (BodyPartDef d) => d.hitPoints);
			array[2] = new TableDataGetter<BodyPartDef>("bleeding\nate\nmultiplier", (BodyPartDef d) => d.bleedRate.ToStringPercent());
			array[3] = new TableDataGetter<BodyPartDef>("perm injury\nbase chance", (BodyPartDef d) => d.permanentInjuryBaseChance.ToStringPercent());
			array[4] = new TableDataGetter<BodyPartDef>("frostbite\nvulnerability", (BodyPartDef d) => d.frostbiteVulnerability);
			array[5] = new TableDataGetter<BodyPartDef>("solid", (BodyPartDef d) => (!d.IsSolidInDefinition_Debug) ? "" : "S");
			array[6] = new TableDataGetter<BodyPartDef>("beauty\nrelated", (BodyPartDef d) => (!d.beautyRelated) ? "" : "B");
			array[7] = new TableDataGetter<BodyPartDef>("alive", (BodyPartDef d) => (!d.alive) ? "" : "A");
			array[8] = new TableDataGetter<BodyPartDef>("conceptual", (BodyPartDef d) => (!d.conceptual) ? "" : "C");
			array[9] = new TableDataGetter<BodyPartDef>("delicate", (BodyPartDef d) => (!d.IsDelicate) ? "" : "D");
			array[10] = new TableDataGetter<BodyPartDef>("can\nsuggest\namputation", (BodyPartDef d) => (!d.canSuggestAmputation) ? "no A" : "");
			array[11] = new TableDataGetter<BodyPartDef>("socketed", (BodyPartDef d) => (!d.socketed) ? "" : "DoL");
			array[12] = new TableDataGetter<BodyPartDef>("skin covered", (BodyPartDef d) => (!d.IsSkinCoveredInDefinition_Debug) ? "" : "skin");
			array[13] = new TableDataGetter<BodyPartDef>("pawn generator\ncan amputate", (BodyPartDef d) => (!d.pawnGeneratorCanAmputate) ? "" : "amp");
			array[14] = new TableDataGetter<BodyPartDef>("spawn thing\non removed", (BodyPartDef d) => d.spawnThingOnRemoved);
			array[15] = new TableDataGetter<BodyPartDef>("hitChanceFactors", delegate(BodyPartDef d)
			{
				string result;
				if (d.hitChanceFactors == null)
				{
					result = "";
				}
				else
				{
					result = (from kvp in d.hitChanceFactors
					select kvp.ToString()).ToCommaList(false);
				}
				return result;
			});
			array[16] = new TableDataGetter<BodyPartDef>("tags", delegate(BodyPartDef d)
			{
				string result;
				if (d.tags == null)
				{
					result = "";
				}
				else
				{
					result = (from t in d.tags
					select t.defName).ToCommaList(false);
				}
				return result;
			});
			DebugTables.MakeTablesDialog<BodyPartDef>(allDefs, array);
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x002ACD34 File Offset: 0x002AB134
		[DebugOutput]
		public static void TraderKinds()
		{
			IEnumerable<TraderKindDef> allDefs = DefDatabase<TraderKindDef>.AllDefs;
			TableDataGetter<TraderKindDef>[] array = new TableDataGetter<TraderKindDef>[2];
			array[0] = new TableDataGetter<TraderKindDef>("defName", (TraderKindDef d) => d.defName);
			array[1] = new TableDataGetter<TraderKindDef>("commonality", (TraderKindDef d) => d.CalculatedCommonality.ToString("F2"));
			DebugTables.MakeTablesDialog<TraderKindDef>(allDefs, array);
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x002ACDA8 File Offset: 0x002AB1A8
		[DebugOutput]
		public static void TraderKindThings()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			foreach (TraderKindDef localTk2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localTk = localTk2;
				string text = localTk.defName;
				text = text.Replace("Caravan", "Car");
				text = text.Replace("Visitor", "Vis");
				text = text.Replace("Orbital", "Orb");
				text = text.Replace("Neolithic", "Ne");
				text = text.Replace("Outlander", "Out");
				text = text.Replace("_", " ");
				text = text.Shorten();
				list.Add(new TableDataGetter<ThingDef>(text, (ThingDef td) => localTk.WillTrade(td).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.001f && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty<ThingCategoryDef>()) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.thingCategories.NullOrEmpty<ThingCategoryDef>() ? "zzzzzzz" : d.thingCategories[0].defName, d.BaseMarketValue
			select d, list.ToArray());
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x002ACF54 File Offset: 0x002AB354
		[DebugOutput]
		public static void Surgeries()
		{
			IEnumerable<RecipeDef> dataSources = from d in DefDatabase<RecipeDef>.AllDefs
			where d.IsSurgery
			orderby d.WorkAmountTotal(null) descending
			select d;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[6];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("work", (RecipeDef d) => d.WorkAmountTotal(null).ToString("F0"));
			array[2] = new TableDataGetter<RecipeDef>("ingredients", (RecipeDef d) => (from ing in d.ingredients
			select ing.ToString()).ToCommaList(false));
			array[3] = new TableDataGetter<RecipeDef>("skillRequirements", delegate(RecipeDef d)
			{
				string result;
				if (d.skillRequirements == null)
				{
					result = "-";
				}
				else
				{
					result = (from ing in d.skillRequirements
					select ing.ToString()).ToCommaList(false);
				}
				return result;
			});
			array[4] = new TableDataGetter<RecipeDef>("surgerySuccessChanceFactor", (RecipeDef d) => d.surgerySuccessChanceFactor.ToStringPercent());
			array[5] = new TableDataGetter<RecipeDef>("deathOnFailChance", (RecipeDef d) => d.deathOnFailedSurgeryChance.ToStringPercent());
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x002AD0B4 File Offset: 0x002AB4B4
		[DebugOutput]
		public static void HitsToKill()
		{
			Dictionary<ThingDef, <>__AnonType0<ThingDef, float, int>> data = (from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			select d).Select(delegate(ThingDef x)
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < 15; i++)
				{
					PawnGenerationRequest request = new PawnGenerationRequest(x.race.AnyPawnKind, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					for (int j = 0; j < 1000; j++)
					{
						pawn.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
						if (pawn.Destroyed)
						{
							num += j + 1;
							break;
						}
					}
					if (!pawn.Destroyed)
					{
						Log.Error("Could not kill pawn " + pawn.ToStringSafe<Pawn>(), false);
					}
					if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
					{
						num2++;
					}
					if (Find.WorldPawns.Contains(pawn))
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				float hits = (float)num / 15f;
				return new
				{
					Race = x,
					Hits = hits,
					DiedDueToDamageThreshold = num2
				};
			}).ToDictionary(x => x.Race);
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			orderby d.race.baseHealthScale descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("10 damage hits", (ThingDef d) => data[d].Hits.ToString("F0"));
			array[2] = new TableDataGetter<ThingDef>("died due to\ndam. thresh.", (ThingDef d) => data[d].DiedDueToDamageThreshold + "/" + 15);
			array[3] = new TableDataGetter<ThingDef>("mech", (ThingDef d) => (!d.race.IsMechanoid) ? "" : "mech");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x002AD214 File Offset: 0x002AB614
		[DebugOutput]
		public static void Terrains()
		{
			IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
			TableDataGetter<TerrainDef>[] array = new TableDataGetter<TerrainDef>[16];
			array[0] = new TableDataGetter<TerrainDef>("defName", (TerrainDef d) => d.defName);
			array[1] = new TableDataGetter<TerrainDef>("work", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.WorkToBuild, null).ToString());
			array[2] = new TableDataGetter<TerrainDef>("beauty", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<TerrainDef>("cleanliness", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null).ToString());
			array[4] = new TableDataGetter<TerrainDef>("path\ncost", (TerrainDef d) => d.pathCost.ToString());
			array[5] = new TableDataGetter<TerrainDef>("fertility", (TerrainDef d) => d.fertility.ToStringPercentEmptyZero("F0"));
			array[6] = new TableDataGetter<TerrainDef>("accept\nfilth", (TerrainDef d) => d.acceptFilth.ToStringCheckBlank());
			array[7] = new TableDataGetter<TerrainDef>("accept terrain\nsource filth", (TerrainDef d) => d.acceptTerrainSourceFilth.ToStringCheckBlank());
			array[8] = new TableDataGetter<TerrainDef>("generated\nfilth", (TerrainDef d) => (d.generatedFilth == null) ? "" : d.generatedFilth.defName);
			array[9] = new TableDataGetter<TerrainDef>("hold\nsnow", (TerrainDef d) => d.holdSnow.ToStringCheckBlank());
			array[10] = new TableDataGetter<TerrainDef>("take\nfootprints", (TerrainDef d) => d.takeFootprints.ToStringCheckBlank());
			array[11] = new TableDataGetter<TerrainDef>("avoid\nwander", (TerrainDef d) => d.avoidWander.ToStringCheckBlank());
			array[12] = new TableDataGetter<TerrainDef>("buildable", (TerrainDef d) => d.BuildableByPlayer.ToStringCheckBlank());
			array[13] = new TableDataGetter<TerrainDef>("cost\nlist", (TerrainDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<TerrainDef>("research", delegate(TerrainDef d)
			{
				string result;
				if (d.researchPrerequisites != null)
				{
					result = (from pr in d.researchPrerequisites
					select pr.defName).ToCommaList(false);
				}
				else
				{
					result = "";
				}
				return result;
			});
			array[15] = new TableDataGetter<TerrainDef>("affordances", (TerrainDef d) => (from af in d.affordances
			select af.defName).ToCommaList(false));
			DebugTables.MakeTablesDialog<TerrainDef>(allDefs, array);
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x002AD4DC File Offset: 0x002AB8DC
		[DebugOutput]
		public static void TerrainAffordances()
		{
			IEnumerable<BuildableDef> dataSources = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && !d.IsFrame
			select d).Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>());
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[3];
			array[0] = new TableDataGetter<BuildableDef>("type", (BuildableDef d) => (!(d is TerrainDef)) ? "building" : "terrain");
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("terrain\naffordance\nneeded", (BuildableDef d) => (d.terrainAffordanceNeeded == null) ? "" : d.terrainAffordanceNeeded.defName);
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x002AD5B0 File Offset: 0x002AB9B0
		[DebugOutput]
		public static void MentalBreaks()
		{
			IEnumerable<MentalBreakDef> dataSources = from d in DefDatabase<MentalBreakDef>.AllDefs
			orderby d.intensity, d.defName
			select d;
			TableDataGetter<MentalBreakDef>[] array = new TableDataGetter<MentalBreakDef>[11];
			array[0] = new TableDataGetter<MentalBreakDef>("defName", (MentalBreakDef d) => d.defName);
			array[1] = new TableDataGetter<MentalBreakDef>("intensity", (MentalBreakDef d) => d.intensity.ToString());
			array[2] = new TableDataGetter<MentalBreakDef>("chance in intensity", (MentalBreakDef d) => (d.baseCommonality / (from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == d.intensity
			select x).Sum((MentalBreakDef x) => x.baseCommonality)).ToStringPercent());
			array[3] = new TableDataGetter<MentalBreakDef>("min duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)d.mentalState.minTicksBeforeRecovery / 60000f).ToString("0.##") : "");
			array[4] = new TableDataGetter<MentalBreakDef>("avg duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? (Mathf.Min((float)d.mentalState.minTicksBeforeRecovery + d.mentalState.recoveryMtbDays * 60000f, (float)d.mentalState.maxTicksBeforeRecovery) / 60000f).ToString("0.##") : "");
			array[5] = new TableDataGetter<MentalBreakDef>("max duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)d.mentalState.maxTicksBeforeRecovery / 60000f).ToString("0.##") : "");
			array[6] = new TableDataGetter<MentalBreakDef>("recoverFromSleep", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.recoverFromSleep) ? "" : "recoverFromSleep");
			array[7] = new TableDataGetter<MentalBreakDef>("recoveryThought", (MentalBreakDef d) => (d.mentalState != null) ? d.mentalState.moodRecoveryThought.ToStringSafe<ThoughtDef>() : "");
			array[8] = new TableDataGetter<MentalBreakDef>("category", (MentalBreakDef d) => (d.mentalState == null) ? "" : d.mentalState.category.ToString());
			array[9] = new TableDataGetter<MentalBreakDef>("blockNormalThoughts", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.blockNormalThoughts) ? "" : "blockNormalThoughts");
			array[10] = new TableDataGetter<MentalBreakDef>("allowBeatfire", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.allowBeatfire) ? "" : "allowBeatfire");
			DebugTables.MakeTablesDialog<MentalBreakDef>(dataSources, array);
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x002AD7E4 File Offset: 0x002ABBE4
		[DebugOutput]
		public static void Traits()
		{
			List<Pawn> testColonists = new List<Pawn>();
			for (int i = 0; i < 1000; i++)
			{
				testColonists.Add(PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer));
			}
			Func<TraitDegreeData, TraitDef> getTrait = (TraitDegreeData d) => DefDatabase<TraitDef>.AllDefs.First((TraitDef td) => td.degreeDatas.Contains(d));
			Func<TraitDegreeData, float> getPrevalence = delegate(TraitDegreeData d)
			{
				float num = 0f;
				foreach (Pawn pawn in testColonists)
				{
					Trait trait = pawn.story.traits.GetTrait(getTrait(d));
					if (trait != null && trait.Degree == d.degree)
					{
						num += 1f;
					}
				}
				return num / 1000f;
			};
			IEnumerable<TraitDegreeData> dataSources = DefDatabase<TraitDef>.AllDefs.SelectMany((TraitDef tr) => tr.degreeDatas);
			TableDataGetter<TraitDegreeData>[] array = new TableDataGetter<TraitDegreeData>[8];
			array[0] = new TableDataGetter<TraitDegreeData>("trait", (TraitDegreeData d) => getTrait(d).defName);
			array[1] = new TableDataGetter<TraitDegreeData>("trait commonality", (TraitDegreeData d) => getTrait(d).GetGenderSpecificCommonality(Gender.None).ToString("F2"));
			array[2] = new TableDataGetter<TraitDegreeData>("trait commonalityFemale", (TraitDegreeData d) => getTrait(d).GetGenderSpecificCommonality(Gender.Female).ToString("F2"));
			array[3] = new TableDataGetter<TraitDegreeData>("degree", (TraitDegreeData d) => d.label);
			array[4] = new TableDataGetter<TraitDegreeData>("degree num", (TraitDegreeData d) => (getTrait(d).degreeDatas.Count <= 0) ? "" : d.degree.ToString());
			array[5] = new TableDataGetter<TraitDegreeData>("degree commonality", (TraitDegreeData d) => (getTrait(d).degreeDatas.Count <= 0) ? "" : d.commonality.ToString("F2"));
			array[6] = new TableDataGetter<TraitDegreeData>("marketValueFactorOffset", (TraitDegreeData d) => d.marketValueFactorOffset.ToString("F0"));
			array[7] = new TableDataGetter<TraitDegreeData>("prevalence among Colonists", (TraitDegreeData d) => getPrevalence(d).ToStringPercent());
			DebugTables.MakeTablesDialog<TraitDegreeData>(dataSources, array);
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x002AD988 File Offset: 0x002ABD88
		[DebugOutput]
		public static void BestThingRequestGroup()
		{
			IEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefs
			where ListerThings.EverListable(x, ListerThingsUse.Global) || ListerThings.EverListable(x, ListerThingsUse.Region)
			orderby x.label
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("best local", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source;
				if (!ListerThings.EverListable(d, ListerThingsUse.Region))
				{
					source = Enumerable.Empty<ThingRequestGroup>();
				}
				else
				{
					source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
					where x.StoreInRegion() && x.Includes(d)
					select x;
				}
				string result;
				if (!source.Any<ThingRequestGroup>())
				{
					result = "-";
				}
				else
				{
					ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y)));
					result = string.Concat(new object[]
					{
						best,
						" (defs: ",
						DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Region) && best.Includes(x)),
						")"
					});
				}
				return result;
			});
			array[2] = new TableDataGetter<ThingDef>("best global", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source;
				if (!ListerThings.EverListable(d, ListerThingsUse.Global))
				{
					source = Enumerable.Empty<ThingRequestGroup>();
				}
				else
				{
					source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
					where x.Includes(d)
					select x;
				}
				string result;
				if (!source.Any<ThingRequestGroup>())
				{
					result = "-";
				}
				else
				{
					ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y)));
					result = string.Concat(new object[]
					{
						best,
						" (defs: ",
						DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Global) && best.Includes(x)),
						")"
					});
				}
				return result;
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x002ADA68 File Offset: 0x002ABE68
		[DebugOutput]
		public static void Prosthetics()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, (Pawn p) => p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			Action refreshPawn = delegate()
			{
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
			};
			Func<RecipeDef, BodyPartRecord> getApplicationPoint = (RecipeDef recipe) => recipe.appliedOnFixedBodyParts.SelectMany((BodyPartDef bpd) => pawn.def.race.body.GetPartsWithDef(bpd)).FirstOrDefault<BodyPartRecord>();
			Func<RecipeDef, ThingDef> getProstheticItem = (RecipeDef recipe) => (from ic in recipe.ingredients
			select ic.filter.AnyAllowedDef).FirstOrDefault((ThingDef td) => !td.IsMedicine);
			List<TableDataGetter<RecipeDef>> list = new List<TableDataGetter<RecipeDef>>();
			list.Add(new TableDataGetter<RecipeDef>("defName", (RecipeDef r) => r.defName));
			list.Add(new TableDataGetter<RecipeDef>("price", delegate(RecipeDef r)
			{
				ThingDef thingDef = getProstheticItem(r);
				return (thingDef == null) ? 0f : thingDef.BaseMarketValue;
			}));
			list.Add(new TableDataGetter<RecipeDef>("install time", (RecipeDef r) => r.workAmount));
			list.Add(new TableDataGetter<RecipeDef>("install total cost", delegate(RecipeDef r)
			{
				float num = r.ingredients.Sum((IngredientCount ic) => ic.filter.AnyAllowedDef.BaseMarketValue * ic.GetBaseCount());
				float num2 = r.workAmount * 0.0036f;
				return num + num2;
			}));
			list.Add(new TableDataGetter<RecipeDef>("install skill", (RecipeDef r) => (from sr in r.skillRequirements
			select sr.minLevel).Max()));
			using (IEnumerator<PawnCapacityDef> enumerator = (from pc in DefDatabase<PawnCapacityDef>.AllDefs
			orderby pc.listOrder
			select pc).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnCapacityDef cap = enumerator.Current;
					list.Add(new TableDataGetter<RecipeDef>(cap.defName, delegate(RecipeDef r)
					{
						refreshPawn();
						Thing pawn;
						r.Worker.ApplyOnPawn(pawn, getApplicationPoint(r), null, null, null);
						float num = pawn.health.capacities.GetLevel(cap) - 1f;
						string result;
						if ((double)Math.Abs(num) > 0.001)
						{
							result = num.ToStringPercent();
						}
						else
						{
							refreshPawn();
							BodyPartRecord bodyPartRecord = getApplicationPoint(r);
							pawn = pawn;
							DamageDef executionCut = DamageDefOf.ExecutionCut;
							float amount = pawn.health.hediffSet.GetPartHealth(bodyPartRecord) / 2f;
							BodyPartRecord hitPart = bodyPartRecord;
							pawn.TakeDamage(new DamageInfo(executionCut, amount, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
							List<PawnCapacityUtility.CapacityImpactor> list2 = new List<PawnCapacityUtility.CapacityImpactor>();
							PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, cap, list2);
							if (list2.Any((PawnCapacityUtility.CapacityImpactor imp) => imp.IsDirect))
							{
								result = 0f.ToStringPercent();
							}
							else
							{
								result = "";
							}
						}
						return result;
					}));
				}
			}
			list.Add(new TableDataGetter<RecipeDef>("tech level", (RecipeDef r) => (getProstheticItem(r) != null) ? getProstheticItem(r).techLevel.ToStringHuman() : ""));
			list.Add(new TableDataGetter<RecipeDef>("thingSetMakerTags", (RecipeDef r) => (getProstheticItem(r) != null) ? getProstheticItem(r).thingSetMakerTags.ToCommaList(false) : ""));
			list.Add(new TableDataGetter<RecipeDef>("techHediffsTags", (RecipeDef r) => (getProstheticItem(r) != null) ? getProstheticItem(r).techHediffsTags.ToCommaList(false) : ""));
			DebugTables.MakeTablesDialog<RecipeDef>(from r in ThingDefOf.Human.AllRecipes
			where r.workerClass == typeof(Recipe_InstallArtificialBodyPart) || r.workerClass == typeof(Recipe_InstallNaturalBodyPart)
			select r, list.ToArray());
			Messages.Clear();
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x002ADD58 File Offset: 0x002AC158
		[DebugOutput]
		public static void JoyGivers()
		{
			IEnumerable<JoyGiverDef> allDefs = DefDatabase<JoyGiverDef>.AllDefs;
			TableDataGetter<JoyGiverDef>[] array = new TableDataGetter<JoyGiverDef>[11];
			array[0] = new TableDataGetter<JoyGiverDef>("defName", (JoyGiverDef d) => d.defName);
			array[1] = new TableDataGetter<JoyGiverDef>("joyKind", (JoyGiverDef d) => (d.joyKind != null) ? d.joyKind.defName : "null");
			array[2] = new TableDataGetter<JoyGiverDef>("baseChance", (JoyGiverDef d) => d.baseChance.ToString());
			array[3] = new TableDataGetter<JoyGiverDef>("canDoWhileInBed", (JoyGiverDef d) => d.canDoWhileInBed.ToStringCheckBlank());
			array[4] = new TableDataGetter<JoyGiverDef>("desireSit", (JoyGiverDef d) => d.desireSit.ToStringCheckBlank());
			array[5] = new TableDataGetter<JoyGiverDef>("unroofedOnly", (JoyGiverDef d) => d.unroofedOnly.ToStringCheckBlank());
			array[6] = new TableDataGetter<JoyGiverDef>("jobDef", (JoyGiverDef d) => (d.jobDef != null) ? d.jobDef.defName : "null");
			array[7] = new TableDataGetter<JoyGiverDef>("pctPawnsEverDo", (JoyGiverDef d) => d.pctPawnsEverDo.ToStringPercent());
			array[8] = new TableDataGetter<JoyGiverDef>("requiredCapacities", delegate(JoyGiverDef d)
			{
				string result;
				if (d.requiredCapacities == null)
				{
					result = "";
				}
				else
				{
					result = (from c in d.requiredCapacities
					select c.defName).ToCommaList(false);
				}
				return result;
			});
			array[9] = new TableDataGetter<JoyGiverDef>("thingDefs", delegate(JoyGiverDef d)
			{
				string result;
				if (d.thingDefs == null)
				{
					result = "";
				}
				else
				{
					result = (from c in d.thingDefs
					select c.defName).ToCommaList(false);
				}
				return result;
			});
			array[10] = new TableDataGetter<JoyGiverDef>("JoyGainFactors", delegate(JoyGiverDef d)
			{
				string result;
				if (d.thingDefs == null)
				{
					result = "";
				}
				else
				{
					result = (from c in d.thingDefs
					select c.GetStatValueAbstract(StatDefOf.JoyGainFactor, null).ToString("F2")).ToCommaList(false);
				}
				return result;
			});
			DebugTables.MakeTablesDialog<JoyGiverDef>(allDefs, array);
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x002ADF48 File Offset: 0x002AC348
		[DebugOutput]
		public static void JoyJobs()
		{
			IEnumerable<JobDef> dataSources = from j in DefDatabase<JobDef>.AllDefs
			where j.joyKind != null
			select j;
			TableDataGetter<JobDef>[] array = new TableDataGetter<JobDef>[7];
			array[0] = new TableDataGetter<JobDef>("defName", (JobDef d) => d.defName);
			array[1] = new TableDataGetter<JobDef>("joyKind", (JobDef d) => d.joyKind.defName);
			array[2] = new TableDataGetter<JobDef>("joyDuration", (JobDef d) => d.joyDuration.ToString());
			array[3] = new TableDataGetter<JobDef>("joyGainRate", (JobDef d) => d.joyGainRate.ToString());
			array[4] = new TableDataGetter<JobDef>("joyMaxParticipants", (JobDef d) => d.joyMaxParticipants.ToString());
			array[5] = new TableDataGetter<JobDef>("joySkill", (JobDef d) => (d.joySkill == null) ? "" : d.joySkill.defName);
			array[6] = new TableDataGetter<JobDef>("joyXpPerTick", (JobDef d) => d.joyXpPerTick.ToString());
			DebugTables.MakeTablesDialog<JobDef>(dataSources, array);
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x002AE0B0 File Offset: 0x002AC4B0
		[DebugOutput]
		public static void Thoughts()
		{
			Func<ThoughtDef, string> stagesText = delegate(ThoughtDef t)
			{
				string text = "";
				string result;
				if (t.stages == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < t.stages.Count; i++)
					{
						ThoughtStage thoughtStage = t.stages[i];
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"[",
							i,
							"] "
						});
						if (thoughtStage == null)
						{
							text += "null";
						}
						else
						{
							if (thoughtStage.label != null)
							{
								text += thoughtStage.label;
							}
							if (thoughtStage.labelSocial != null)
							{
								if (thoughtStage.label != null)
								{
									text += "/";
								}
								text += thoughtStage.labelSocial;
							}
							text += " ";
							if (thoughtStage.baseMoodEffect != 0f)
							{
								text = text + "[" + thoughtStage.baseMoodEffect.ToStringWithSign("0.##") + " Mo]";
							}
							if (thoughtStage.baseOpinionOffset != 0f)
							{
								text = text + "(" + thoughtStage.baseOpinionOffset.ToStringWithSign("0.##") + " Op)";
							}
						}
						if (i < t.stages.Count - 1)
						{
							text += "\n";
						}
					}
					result = text;
				}
				return result;
			};
			IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
			TableDataGetter<ThoughtDef>[] array = new TableDataGetter<ThoughtDef>[18];
			array[0] = new TableDataGetter<ThoughtDef>("defName", (ThoughtDef d) => d.defName);
			array[1] = new TableDataGetter<ThoughtDef>("type", (ThoughtDef d) => (!d.IsMemory) ? "situ" : "mem");
			array[2] = new TableDataGetter<ThoughtDef>("social", (ThoughtDef d) => (!d.IsSocial) ? "mood" : "soc");
			array[3] = new TableDataGetter<ThoughtDef>("stages", (ThoughtDef d) => stagesText(d));
			array[4] = new TableDataGetter<ThoughtDef>("best\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Max((ThoughtStage st) => st.baseMoodEffect));
			array[5] = new TableDataGetter<ThoughtDef>("worst\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Min((ThoughtStage st) => st.baseMoodEffect));
			array[6] = new TableDataGetter<ThoughtDef>("stack\nlimit", (ThoughtDef d) => d.stackLimit.ToString());
			array[7] = new TableDataGetter<ThoughtDef>("stack\nlimit\nper o. pawn", (ThoughtDef d) => (d.stackLimitForSameOtherPawn >= 0) ? d.stackLimitForSameOtherPawn.ToString() : "");
			array[8] = new TableDataGetter<ThoughtDef>("stacked\neffect\nmultiplier", (ThoughtDef d) => (d.stackLimit != 1) ? d.stackedEffectMultiplier.ToStringPercent() : "");
			array[9] = new TableDataGetter<ThoughtDef>("duration\n(days)", (ThoughtDef d) => d.durationDays.ToString());
			array[10] = new TableDataGetter<ThoughtDef>("effect\nmultiplying\nstat", (ThoughtDef d) => (d.effectMultiplyingStat != null) ? d.effectMultiplyingStat.defName : "");
			array[11] = new TableDataGetter<ThoughtDef>("game\ncondition", (ThoughtDef d) => (d.gameCondition != null) ? d.gameCondition.defName : "");
			array[12] = new TableDataGetter<ThoughtDef>("hediff", (ThoughtDef d) => (d.hediff != null) ? d.hediff.defName : "");
			array[13] = new TableDataGetter<ThoughtDef>("lerp opinion\nto zero\nafter duration pct", (ThoughtDef d) => d.lerpOpinionToZeroAfterDurationPct.ToStringPercent());
			array[14] = new TableDataGetter<ThoughtDef>("max cumulated\nopinion\noffset", (ThoughtDef d) => (d.maxCumulatedOpinionOffset <= 99999f) ? d.maxCumulatedOpinionOffset.ToString() : "");
			array[15] = new TableDataGetter<ThoughtDef>("next\nthought", (ThoughtDef d) => (d.nextThought != null) ? d.nextThought.defName : "");
			array[16] = new TableDataGetter<ThoughtDef>("nullified\nif not colonist", (ThoughtDef d) => d.nullifiedIfNotColonist.ToStringCheckBlank());
			array[17] = new TableDataGetter<ThoughtDef>("show\nbubble", (ThoughtDef d) => d.showBubble.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThoughtDef>(allDefs, array);
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x002AE3E4 File Offset: 0x002AC7E4
		[DebugOutput]
		public static void GenSteps()
		{
			IEnumerable<GenStepDef> dataSources = from x in DefDatabase<GenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<GenStepDef>[] array = new TableDataGetter<GenStepDef>[4];
			array[0] = new TableDataGetter<GenStepDef>("defName", (GenStepDef x) => x.defName);
			array[1] = new TableDataGetter<GenStepDef>("order", (GenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<GenStepDef>("class", (GenStepDef x) => x.genStep.GetType().Name);
			array[3] = new TableDataGetter<GenStepDef>("site", (GenStepDef x) => (x.linkWithSite == null) ? "" : x.linkWithSite.defName);
			DebugTables.MakeTablesDialog<GenStepDef>(dataSources, array);
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x002AE4F0 File Offset: 0x002AC8F0
		[DebugOutput]
		public static void WorldGenSteps()
		{
			IEnumerable<WorldGenStepDef> dataSources = from x in DefDatabase<WorldGenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<WorldGenStepDef>[] array = new TableDataGetter<WorldGenStepDef>[3];
			array[0] = new TableDataGetter<WorldGenStepDef>("defName", (WorldGenStepDef x) => x.defName);
			array[1] = new TableDataGetter<WorldGenStepDef>("order", (WorldGenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<WorldGenStepDef>("class", (WorldGenStepDef x) => x.worldGenStep.GetType().Name);
			DebugTables.MakeTablesDialog<WorldGenStepDef>(dataSources, array);
		}
	}
}
