using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E20 RID: 3616
	[HasDebugOutput]
	internal static class DebugOutputsPawns
	{
		// Token: 0x06005413 RID: 21523 RVA: 0x002B2144 File Offset: 0x002B0544
		[DebugOutput]
		public static void PawnKindsBasics()
		{
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Humanlike
			select d into k
			orderby (k.defaultFactionType == null) ? "" : k.defaultFactionType.label, k.combatPower
			select k;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[18];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName);
			array[1] = new TableDataGetter<PawnKindDef>("faction", (PawnKindDef d) => (d.defaultFactionType == null) ? "" : d.defaultFactionType.defName);
			array[2] = new TableDataGetter<PawnKindDef>("points", (PawnKindDef d) => d.combatPower.ToString("F0"));
			array[3] = new TableDataGetter<PawnKindDef>("minAge", (PawnKindDef d) => d.minGenerationAge.ToString("F0"));
			array[4] = new TableDataGetter<PawnKindDef>("maxAge", (PawnKindDef d) => (d.maxGenerationAge >= 10000) ? "" : d.maxGenerationAge.ToString("F0"));
			array[5] = new TableDataGetter<PawnKindDef>("recruitDiff", (PawnKindDef d) => d.baseRecruitDifficulty.ToStringPercent());
			array[6] = new TableDataGetter<PawnKindDef>("itemQuality", (PawnKindDef d) => d.itemQuality.ToString());
			array[7] = new TableDataGetter<PawnKindDef>("forceNormGearQual", (PawnKindDef d) => d.forceNormalGearQuality.ToStringCheckBlank());
			array[8] = new TableDataGetter<PawnKindDef>("weapon$", (PawnKindDef d) => d.weaponMoney.ToString());
			array[9] = new TableDataGetter<PawnKindDef>("apparel$", (PawnKindDef d) => d.apparelMoney.ToString());
			array[10] = new TableDataGetter<PawnKindDef>("techHediffsCh", (PawnKindDef d) => d.techHediffsChance.ToStringPercentEmptyZero("F0"));
			array[11] = new TableDataGetter<PawnKindDef>("techHediffs$", (PawnKindDef d) => d.techHediffsMoney.ToString());
			array[12] = new TableDataGetter<PawnKindDef>("gearHealth", (PawnKindDef d) => d.gearHealthRange.ToString());
			array[13] = new TableDataGetter<PawnKindDef>("invNutrition", (PawnKindDef d) => d.invNutrition.ToString());
			array[14] = new TableDataGetter<PawnKindDef>("addictionChance", (PawnKindDef d) => d.chemicalAddictionChance.ToStringPercent());
			array[15] = new TableDataGetter<PawnKindDef>("combatDrugChance", (PawnKindDef d) => (d.combatEnhancingDrugsChance <= 0f) ? "" : d.combatEnhancingDrugsChance.ToStringPercent());
			array[16] = new TableDataGetter<PawnKindDef>("combatDrugCount", (PawnKindDef d) => (d.combatEnhancingDrugsCount.max <= 0) ? "" : d.combatEnhancingDrugsCount.ToString());
			array[17] = new TableDataGetter<PawnKindDef>("bsCryptosleepComm", (PawnKindDef d) => d.backstoryCryptosleepCommonality.ToStringPercentEmptyZero("F0"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x002B24C8 File Offset: 0x002B08C8
		[DebugOutput]
		public static void PawnKindsWeaponUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (PawnKindDef x) => x.weaponMoney.Average.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (PawnKindDef x) => x.weaponMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (PawnKindDef x) => x.weaponMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			list.AddRange(from w in DefDatabase<ThingDef>.AllDefs
			where w.IsWeapon && !w.weaponTags.NullOrEmpty<string>()
			orderby w.IsMeleeWeapon descending, w.techLevel, w.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(w.label.Shorten() + "\n$" + w.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
			{
				string result;
				if (k.weaponTags != null && w.weaponTags.Any((string z) => k.weaponTags.Contains(z)))
				{
					float num = PawnWeaponGenerator.CheapestNonDerpPriceFor(w);
					if (k.weaponMoney.max < num)
					{
						result = "-";
					}
					else if (k.weaponMoney.min > num)
					{
						result = "✓";
					}
					else
					{
						result = (1f - (num - k.weaponMoney.min) / (k.weaponMoney.max - k.weaponMoney.min)).ToStringPercent("F0");
					}
				}
				else
				{
					result = "";
				}
				return result;
			}));
			DebugTables.MakeTablesDialog<PawnKindDef>(from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.intelligence >= Intelligence.ToolUser
			orderby (x.defaultFactionType == null) ? int.MaxValue : ((int)x.defaultFactionType.techLevel), x.combatPower
			select x, list.ToArray());
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x002B26E8 File Offset: 0x002B0AE8
		[DebugOutput]
		public static void PawnKindsApparelUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (PawnKindDef x) => x.apparelMoney.Average.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (PawnKindDef x) => x.apparelMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (PawnKindDef x) => x.apparelMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			list.AddRange(from a in DefDatabase<ThingDef>.AllDefs
			where a.IsApparel
			orderby PawnApparelGenerator.IsHeadgear(a), a.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(a.label.Shorten() + "\n$" + a.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
			{
				string result;
				if (k.apparelRequired != null && k.apparelRequired.Contains(a))
				{
					result = "Rq";
				}
				else if (k.apparelAllowHeadgearChance <= 0f && PawnApparelGenerator.IsHeadgear(a))
				{
					result = "nohat";
				}
				else if (k.apparelTags != null && a.apparel.tags.Any((string z) => k.apparelTags.Contains(z)))
				{
					float baseMarketValue = a.BaseMarketValue;
					if (k.apparelMoney.max < baseMarketValue)
					{
						result = "-";
					}
					else if (k.apparelMoney.min > baseMarketValue)
					{
						result = "✓";
					}
					else
					{
						result = (1f - (baseMarketValue - k.apparelMoney.min) / (k.apparelMoney.max - k.apparelMoney.min)).ToStringPercent("F0");
					}
				}
				else
				{
					result = "";
				}
				return result;
			}));
			DebugTables.MakeTablesDialog<PawnKindDef>(from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Humanlike
			orderby (x.defaultFactionType == null) ? int.MaxValue : ((int)x.defaultFactionType.techLevel), x.combatPower
			select x, list.ToArray());
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x002B28E8 File Offset: 0x002B0CE8
		[DebugOutput]
		public static void PawnKindsTechHediffUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.Add(new TableDataGetter<PawnKindDef>("chance", (PawnKindDef x) => x.techHediffsChance.ToStringPercent()));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (PawnKindDef x) => x.techHediffsMoney.Average.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (PawnKindDef x) => x.techHediffsMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (PawnKindDef x) => x.techHediffsMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			list.AddRange(from t in DefDatabase<ThingDef>.AllDefs
			where t.isTechHediff && t.techHediffsTags != null
			orderby t.techLevel descending, t.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(t.label.Shorten() + "\n$" + t.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
			{
				string result;
				if (k.techHediffsTags != null && t.techHediffsTags.Any((string tag) => k.techHediffsTags.Contains(tag)))
				{
					if (k.techHediffsMoney.max < t.BaseMarketValue)
					{
						result = "-";
					}
					else if (k.techHediffsMoney.min >= t.BaseMarketValue)
					{
						result = "✓";
					}
					else
					{
						result = (1f - (t.BaseMarketValue - k.techHediffsMoney.min) / (k.techHediffsMoney.max - k.techHediffsMoney.min)).ToStringPercent("F0");
					}
				}
				else
				{
					result = "";
				}
				return result;
			}));
			DebugTables.MakeTablesDialog<PawnKindDef>(from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Humanlike
			orderby (x.defaultFactionType == null) ? int.MaxValue : ((int)x.defaultFactionType.techLevel), x.combatPower
			select x, list.ToArray());
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x002B2B14 File Offset: 0x002B0F14
		[DebugOutput]
		public static void PawnKindGearSampled()
		{
			IOrderedEnumerable<PawnKindDef> orderedEnumerable = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			orderby k.combatPower
			select k;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pawnKindDef in orderedEnumerable)
			{
				Faction fac = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
				PawnKindDef kind = pawnKindDef;
				FloatMenuOption item = new FloatMenuOption(string.Concat(new object[]
				{
					kind.defName,
					" (",
					kind.combatPower,
					")"
				}), delegate()
				{
					DefMap<ThingDef, int> weapons = new DefMap<ThingDef, int>();
					DefMap<ThingDef, int> apparel = new DefMap<ThingDef, int>();
					DefMap<HediffDef, int> hediffs = new DefMap<HediffDef, int>();
					for (int i = 0; i < 400; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, fac);
						if (pawn.equipment.Primary != null)
						{
							DefMap<ThingDef, int> defMap;
							ThingDef def;
							(defMap = weapons)[def = pawn.equipment.Primary.def] = defMap[def] + 1;
						}
						foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
						{
							DefMap<HediffDef, int> hediffs2;
							HediffDef def2;
							(hediffs2 = hediffs)[def2 = hediff.def] = hediffs2[def2] + 1;
						}
						foreach (Apparel apparel2 in pawn.apparel.WornApparel)
						{
							DefMap<ThingDef, int> defMap;
							ThingDef def3;
							(defMap = apparel)[def3 = apparel2.def] = defMap[def3] + 1;
						}
						pawn.Destroy(DestroyMode.Vanish);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Sampled ",
						400,
						"x ",
						kind.defName,
						":"
					}));
					stringBuilder.AppendLine("Weapons");
					foreach (ThingDef thingDef in from t in DefDatabase<ThingDef>.AllDefs
					orderby weapons[t] descending
					select t)
					{
						int num = weapons[thingDef];
						if (num > 0)
						{
							stringBuilder.AppendLine("  " + thingDef.defName + "    " + ((float)num / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Apparel");
					foreach (ThingDef thingDef2 in from t in DefDatabase<ThingDef>.AllDefs
					orderby apparel[t] descending
					select t)
					{
						int num2 = apparel[thingDef2];
						if (num2 > 0)
						{
							stringBuilder.AppendLine("  " + thingDef2.defName + "    " + ((float)num2 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Tech hediffs");
					foreach (HediffDef hediffDef in from h in DefDatabase<HediffDef>.AllDefs
					where h.spawnThingOnRemoved != null
					orderby hediffs[h] descending
					select h)
					{
						int num3 = hediffs[hediffDef];
						if (num3 > 0)
						{
							stringBuilder.AppendLine("  " + hediffDef.defName + "    " + ((float)num3 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Addiction hediffs");
					foreach (HediffDef hediffDef2 in from h in DefDatabase<HediffDef>.AllDefs
					where h.IsAddiction
					orderby hediffs[h] descending
					select h)
					{
						int num4 = hediffs[hediffDef2];
						if (num4 > 0)
						{
							stringBuilder.AppendLine("  " + hediffDef2.defName + "    " + ((float)num4 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Other hediffs");
					foreach (HediffDef hediffDef3 in from h in DefDatabase<HediffDef>.AllDefs
					where h.spawnThingOnRemoved == null && !h.IsAddiction
					orderby hediffs[h] descending
					select h)
					{
						int num5 = hediffs[hediffDef3];
						if (num5 > 0)
						{
							stringBuilder.AppendLine("  " + hediffDef3.defName + "    " + ((float)num5 / 400f).ToStringPercent());
						}
					}
					Log.Message(stringBuilder.ToString().TrimEndNewlines(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x002B2C4C File Offset: 0x002B104C
		[DebugOutput]
		public static void RacesFoodConsumption()
		{
			Func<ThingDef, int, string> lsName = delegate(ThingDef d, int lsIndex)
			{
				string result;
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					result = "";
				}
				else
				{
					LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
					result = def.defName;
				}
				return result;
			};
			Func<ThingDef, int, string> maxFood = delegate(ThingDef d, int lsIndex)
			{
				string result;
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					result = "";
				}
				else
				{
					LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
					result = (d.race.baseBodySize * def.bodySizeFactor * def.foodMaxFactor).ToString("F2");
				}
				return result;
			};
			Func<ThingDef, int, string> hungerRate = delegate(ThingDef d, int lsIndex)
			{
				string result;
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					result = "";
				}
				else
				{
					LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
					result = (d.race.baseHungerRate * def.hungerRateFactor).ToString("F2");
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null && d.race.EatsFood
			orderby d.race.baseHungerRate descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[13];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("Lifestage 0", (ThingDef d) => lsName(d, 0));
			array[2] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 0));
			array[3] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 0));
			array[4] = new TableDataGetter<ThingDef>("Lifestage 1", (ThingDef d) => lsName(d, 1));
			array[5] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 1));
			array[6] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 1));
			array[7] = new TableDataGetter<ThingDef>("Lifestage 2", (ThingDef d) => lsName(d, 2));
			array[8] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 2));
			array[9] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 2));
			array[10] = new TableDataGetter<ThingDef>("Lifestage 3", (ThingDef d) => lsName(d, 3));
			array[11] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 3));
			array[12] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 3));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x002B2E78 File Offset: 0x002B1278
		[DebugOutput]
		public static void RacesButchery()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			orderby d.race.baseBodySize
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("mktval", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[2] = new TableDataGetter<ThingDef>("healthScale", (ThingDef d) => d.race.baseHealthScale.ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("hunger rate", (ThingDef d) => d.race.baseHungerRate.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("wildness", (ThingDef d) => d.race.wildness.ToStringPercent());
			array[5] = new TableDataGetter<ThingDef>("bodySize", (ThingDef d) => d.race.baseBodySize.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("meatAmount", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeatAmount, null).ToString("F0"));
			array[7] = new TableDataGetter<ThingDef>("leatherAmount", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.LeatherAmount, null).ToString("F0"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x002B302C File Offset: 0x002B142C
		[DebugOutput]
		public static void AnimalsBasics()
		{
			Func<PawnKindDef, float> dps = (PawnKindDef d) => DebugOutputsPawns.RaceMeleeDpsEstimate(d.race);
			Func<PawnKindDef, float> pointsGuess = delegate(PawnKindDef d)
			{
				float num = 15f;
				num += dps(d) * 10f;
				num *= Mathf.Lerp(1f, d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 3f, 0.25f);
				num *= d.RaceProps.baseHealthScale;
				num *= GenMath.LerpDouble(0.25f, 1f, 1.65f, 1f, Mathf.Clamp(d.RaceProps.baseBodySize, 0.25f, 1f));
				return num * 0.76f;
			};
			Func<PawnKindDef, float> mktValGuess = delegate(PawnKindDef d)
			{
				float num = 18f;
				num += pointsGuess(d) * 2.7f;
				if (d.RaceProps.trainability == TrainabilityDefOf.None)
				{
					num *= 0.5f;
				}
				else if (d.RaceProps.trainability == TrainabilityDefOf.Simple)
				{
					num *= 0.8f;
				}
				else if (d.RaceProps.trainability == TrainabilityDefOf.Intermediate)
				{
					num = num;
				}
				else
				{
					if (d.RaceProps.trainability != TrainabilityDefOf.Advanced)
					{
						throw new InvalidOperationException();
					}
					num += 250f;
				}
				num += d.RaceProps.baseBodySize * 80f;
				if (d.race.HasComp(typeof(CompMilkable)))
				{
					num += 125f;
				}
				if (d.race.HasComp(typeof(CompShearable)))
				{
					num += 90f;
				}
				if (d.race.HasComp(typeof(CompEggLayer)))
				{
					num += 90f;
				}
				num *= Mathf.Lerp(0.8f, 1.2f, d.RaceProps.wildness);
				return num * 0.75f;
			};
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[15];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName);
			array[1] = new TableDataGetter<PawnKindDef>("dps", (PawnKindDef d) => dps(d).ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("healthScale", (PawnKindDef d) => d.RaceProps.baseHealthScale.ToString("F2"));
			array[3] = new TableDataGetter<PawnKindDef>("points", (PawnKindDef d) => d.combatPower.ToString("F0"));
			array[4] = new TableDataGetter<PawnKindDef>("points guess", (PawnKindDef d) => pointsGuess(d).ToString("F0"));
			array[5] = new TableDataGetter<PawnKindDef>("speed", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString("F2"));
			array[6] = new TableDataGetter<PawnKindDef>("mktval", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[7] = new TableDataGetter<PawnKindDef>("mktval guess", (PawnKindDef d) => mktValGuess(d).ToString("F0"));
			array[8] = new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2"));
			array[9] = new TableDataGetter<PawnKindDef>("hunger", (PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2"));
			array[10] = new TableDataGetter<PawnKindDef>("wildness", (PawnKindDef d) => d.RaceProps.wildness.ToStringPercent());
			array[11] = new TableDataGetter<PawnKindDef>("lifespan", (PawnKindDef d) => d.RaceProps.lifeExpectancy.ToString("F1"));
			array[12] = new TableDataGetter<PawnKindDef>("trainability", (PawnKindDef d) => (d.RaceProps.trainability == null) ? "null" : d.RaceProps.trainability.label);
			array[13] = new TableDataGetter<PawnKindDef>("tempMin", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0"));
			array[14] = new TableDataGetter<PawnKindDef>("tempMax", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x002B3304 File Offset: 0x002B1704
		private static float RaceMeleeDpsEstimate(ThingDef race)
		{
			return race.GetStatValueAbstract(StatDefOf.MeleeDPS, null);
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x002B3328 File Offset: 0x002B1728
		[DebugOutput]
		public static void AnimalCombatBalance()
		{
			Func<PawnKindDef, float> meleeDps = delegate(PawnKindDef k)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(k, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, true, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null));
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
				float statValue = pawn.GetStatValue(StatDefOf.MeleeDPS, true);
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				return statValue;
			};
			Func<PawnKindDef, float> averageArmor = delegate(PawnKindDef k)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(k, null);
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
				float result = (pawn.GetStatValue(StatDefOf.ArmorRating_Blunt, true) + pawn.GetStatValue(StatDefOf.ArmorRating_Sharp, true)) / 2f;
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				return result;
			};
			Func<PawnKindDef, float> combatPowerCalculated = delegate(PawnKindDef k)
			{
				float num = 1f + meleeDps(k) * 2f;
				float num2 = 1f + (k.RaceProps.baseHealthScale + averageArmor(k) * 1.8f) * 2f;
				float num3 = num * num2 * 2.5f + 10f;
				return num3 + k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) * 2f;
			};
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.combatPower
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[7];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("meleeDps", (PawnKindDef k) => meleeDps(k).ToString("F1"));
			array[2] = new TableDataGetter<PawnKindDef>("baseHealthScale", (PawnKindDef k) => k.RaceProps.baseHealthScale.ToString());
			array[3] = new TableDataGetter<PawnKindDef>("moveSpeed", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString());
			array[4] = new TableDataGetter<PawnKindDef>("averageArmor", (PawnKindDef k) => averageArmor(k).ToStringPercent());
			array[5] = new TableDataGetter<PawnKindDef>("combatPowerCalculated", (PawnKindDef k) => combatPowerCalculated(k).ToString("F0"));
			array[6] = new TableDataGetter<PawnKindDef>("combatPower", (PawnKindDef k) => k.combatPower.ToString());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x002B34DC File Offset: 0x002B18DC
		[DebugOutput]
		public static void AnimalTradeTags()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName));
			using (IEnumerator<string> enumerator = (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.race.tradeTags != null
			select k).SelectMany((PawnKindDef k) => k.race.tradeTags).Distinct<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string tag = enumerator.Current;
					list.Add(new TableDataGetter<PawnKindDef>(tag, (PawnKindDef k) => (k.race.tradeTags != null && k.race.tradeTags.Contains(tag)).ToStringCheckBlank()));
				}
			}
			DebugTables.MakeTablesDialog<PawnKindDef>(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			select d, list.ToArray());
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x002B3608 File Offset: 0x002B1A08
		[DebugOutput]
		public static void AnimalBehavior()
		{
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[17];
			array[0] = new TableDataGetter<PawnKindDef>("", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("wildness", (PawnKindDef k) => k.RaceProps.wildness.ToStringPercent());
			array[2] = new TableDataGetter<PawnKindDef>("manhunterOnDamageChance", (PawnKindDef k) => k.RaceProps.manhunterOnDamageChance.ToStringPercentEmptyZero("F1"));
			array[3] = new TableDataGetter<PawnKindDef>("manhunterOnTameFailChance", (PawnKindDef k) => k.RaceProps.manhunterOnTameFailChance.ToStringPercentEmptyZero("F1"));
			array[4] = new TableDataGetter<PawnKindDef>("predator", (PawnKindDef k) => k.RaceProps.predator.ToStringCheckBlank());
			array[5] = new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef k) => k.RaceProps.baseBodySize.ToString("F2"));
			array[6] = new TableDataGetter<PawnKindDef>("maxPreyBodySize", (PawnKindDef k) => (!k.RaceProps.predator) ? "" : k.RaceProps.maxPreyBodySize.ToString("F2"));
			array[7] = new TableDataGetter<PawnKindDef>("canBePredatorPrey", (PawnKindDef k) => k.RaceProps.canBePredatorPrey.ToStringCheckBlank());
			array[8] = new TableDataGetter<PawnKindDef>("petness", (PawnKindDef k) => k.RaceProps.petness.ToStringPercent());
			array[9] = new TableDataGetter<PawnKindDef>("nuzzleMtbHours", (PawnKindDef k) => (k.RaceProps.nuzzleMtbHours <= 0f) ? "" : k.RaceProps.nuzzleMtbHours.ToString());
			array[10] = new TableDataGetter<PawnKindDef>("packAnimal", (PawnKindDef k) => k.RaceProps.packAnimal.ToStringCheckBlank());
			array[11] = new TableDataGetter<PawnKindDef>("herdAnimal", (PawnKindDef k) => k.RaceProps.herdAnimal.ToStringCheckBlank());
			array[12] = new TableDataGetter<PawnKindDef>("wildGroupSizeMin", (PawnKindDef k) => (k.wildGroupSize.min == 1) ? "" : k.wildGroupSize.min.ToString());
			array[13] = new TableDataGetter<PawnKindDef>("wildGroupSizeMax", (PawnKindDef k) => (k.wildGroupSize.max == 1) ? "" : k.wildGroupSize.max.ToString());
			array[14] = new TableDataGetter<PawnKindDef>("CanDoHerdMigration", (PawnKindDef k) => k.RaceProps.CanDoHerdMigration.ToStringCheckBlank());
			array[15] = new TableDataGetter<PawnKindDef>("herdMigrationAllowed", (PawnKindDef k) => k.RaceProps.herdMigrationAllowed.ToStringCheckBlank());
			array[16] = new TableDataGetter<PawnKindDef>("mateMtb", (PawnKindDef k) => k.RaceProps.mateMtbHours.ToStringEmptyZero("F0"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x002B391C File Offset: 0x002B1D1C
		[DebugOutput]
		public static void AnimalsEcosystem()
		{
			Func<PawnKindDef, float> ecosystemWeightGuess = (PawnKindDef k) => k.RaceProps.baseBodySize * 0.2f + k.RaceProps.baseHungerRate * 0.8f;
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.ecoSystemWeight descending
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[6];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName);
			array[1] = new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("hunger rate", (PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2"));
			array[3] = new TableDataGetter<PawnKindDef>("ecosystem weight", (PawnKindDef d) => d.ecoSystemWeight.ToString("F2"));
			array[4] = new TableDataGetter<PawnKindDef>("ecosystem weight guess", (PawnKindDef d) => ecosystemWeightGuess(d).ToString("F2"));
			array[5] = new TableDataGetter<PawnKindDef>("predator", (PawnKindDef d) => d.RaceProps.predator.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}
	}
}
