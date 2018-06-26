using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	internal static class DebugOutputsPawns
	{
		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheD;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheE;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheF;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache10;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache11;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache12;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache13;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache14;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache15;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache16;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache17;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache18;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache19;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1A;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1B;

		[CompilerGenerated]
		private static Func<ThingDef, TechLevel> <>f__am$cache1C;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1D;

		[CompilerGenerated]
		private static Func<ThingDef, TableDataGetter<PawnKindDef>> <>f__am$cache1E;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache1F;

		[CompilerGenerated]
		private static Func<PawnKindDef, int> <>f__am$cache20;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache21;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache22;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache23;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache24;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache25;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache26;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache27;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache28;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache29;

		[CompilerGenerated]
		private static Func<ThingDef, TableDataGetter<PawnKindDef>> <>f__am$cache2A;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache2B;

		[CompilerGenerated]
		private static Func<PawnKindDef, int> <>f__am$cache2C;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache2D;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache2E;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache2F;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache30;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache31;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache32;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache33;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache34;

		[CompilerGenerated]
		private static Func<ThingDef, TechLevel> <>f__am$cache35;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache36;

		[CompilerGenerated]
		private static Func<ThingDef, TableDataGetter<PawnKindDef>> <>f__am$cache37;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache38;

		[CompilerGenerated]
		private static Func<PawnKindDef, int> <>f__am$cache39;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache3A;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache3B;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache3C;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache3D;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache3E;

		[CompilerGenerated]
		private static Func<Pawn, string> <>f__am$cache3F;

		[CompilerGenerated]
		private static Func<ThingDef, int, string> <>f__am$cache40;

		[CompilerGenerated]
		private static Func<ThingDef, int, string> <>f__am$cache41;

		[CompilerGenerated]
		private static Func<ThingDef, int, string> <>f__am$cache42;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache43;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache44;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache45;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache46;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache47;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache48;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache49;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4A;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4F;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache50;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache51;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache52;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache53;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache54;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache55;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache56;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache57;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache58;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache59;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache5A;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache5B;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache5C;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache5D;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache5E;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache5F;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache60;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache61;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache62;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache63;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache64;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache65;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache66;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache67;

		[CompilerGenerated]
		private static Func<PawnKindDef, IEnumerable<string>> <>f__am$cache68;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache69;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache6A;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache6B;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache6C;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache6D;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache6E;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache6F;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache70;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache71;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache72;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache73;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache74;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache75;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache76;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache77;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache78;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache79;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache7A;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache7B;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache7C;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache7D;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache7E;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache7F;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache80;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache81;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache82;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache83;

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

		[DebugOutput]
		public static void PawnWorkDisablesSampled()
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
					Dictionary<WorkTags, int> dictionary = new Dictionary<WorkTags, int>();
					for (int i = 0; i < 1000; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, fac);
						WorkTags combinedDisabledWorkTags = pawn.story.CombinedDisabledWorkTags;
						IEnumerator enumerator2 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object obj = enumerator2.Current;
								WorkTags workTags = (WorkTags)obj;
								if (!dictionary.ContainsKey(workTags))
								{
									dictionary.Add(workTags, 0);
								}
								if ((combinedDisabledWorkTags & workTags) != WorkTags.None)
								{
									Dictionary<WorkTags, int> dictionary2;
									WorkTags key;
									(dictionary2 = dictionary)[key = workTags] = dictionary2[key] + 1;
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator2 as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
						pawn.Destroy(DestroyMode.Vanish);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Sampled ",
						1000,
						"x ",
						kind.defName,
						":"
					}));
					stringBuilder.AppendLine("Worktags disabled");
					IEnumerator enumerator3 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object obj2 = enumerator3.Current;
							WorkTags key2 = (WorkTags)obj2;
							int num = dictionary[key2];
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"  ",
								key2.ToString(),
								"    ",
								num,
								" (",
								((float)num / 1000f).ToStringPercent(),
								")"
							}));
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator3 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
					Log.Message(stringBuilder.ToString().TrimEndNewlines(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[DebugOutput]
		public static void LivePawnsInspirationChances()
		{
			List<TableDataGetter<Pawn>> list = new List<TableDataGetter<Pawn>>();
			list.Add(new TableDataGetter<Pawn>("name", (Pawn p) => p.Label));
			using (IEnumerator<InspirationDef> enumerator = DefDatabase<InspirationDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InspirationDef iDef = enumerator.Current;
					list.Add(new TableDataGetter<Pawn>(iDef.defName, (Pawn p) => iDef.Worker.InspirationCanOccur(p) ? iDef.Worker.CommonalityFor(p).ToString() : "-no-"));
				}
			}
			DebugTables.MakeTablesDialog<Pawn>(Find.CurrentMap.mapPawns.FreeColonistsSpawned, list.ToArray());
		}

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

		private static float RaceMeleeDpsEstimate(ThingDef race)
		{
			return race.GetStatValueAbstract(StatDefOf.MeleeDPS, null);
		}

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

		[CompilerGenerated]
		private static bool <PawnKindsBasics>m__0(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__1(PawnKindDef k)
		{
			return (k.defaultFactionType == null) ? "" : k.defaultFactionType.label;
		}

		[CompilerGenerated]
		private static float <PawnKindsBasics>m__2(PawnKindDef k)
		{
			return k.combatPower;
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__3(PawnKindDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__4(PawnKindDef d)
		{
			return (d.defaultFactionType == null) ? "" : d.defaultFactionType.defName;
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__5(PawnKindDef d)
		{
			return d.combatPower.ToString("F0");
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__6(PawnKindDef d)
		{
			return d.minGenerationAge.ToString("F0");
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__7(PawnKindDef d)
		{
			return (d.maxGenerationAge >= 10000) ? "" : d.maxGenerationAge.ToString("F0");
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__8(PawnKindDef d)
		{
			return d.baseRecruitDifficulty.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__9(PawnKindDef d)
		{
			return d.itemQuality.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__A(PawnKindDef d)
		{
			return d.forceNormalGearQuality.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__B(PawnKindDef d)
		{
			return d.weaponMoney.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__C(PawnKindDef d)
		{
			return d.apparelMoney.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__D(PawnKindDef d)
		{
			return d.techHediffsChance.ToStringPercentEmptyZero("F0");
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__E(PawnKindDef d)
		{
			return d.techHediffsMoney.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__F(PawnKindDef d)
		{
			return d.gearHealthRange.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__10(PawnKindDef d)
		{
			return d.invNutrition.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__11(PawnKindDef d)
		{
			return d.chemicalAddictionChance.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__12(PawnKindDef d)
		{
			return (d.combatEnhancingDrugsChance <= 0f) ? "" : d.combatEnhancingDrugsChance.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__13(PawnKindDef d)
		{
			return (d.combatEnhancingDrugsCount.max <= 0) ? "" : d.combatEnhancingDrugsCount.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsBasics>m__14(PawnKindDef d)
		{
			return d.backstoryCryptosleepCommonality.ToStringPercentEmptyZero("F0");
		}

		[CompilerGenerated]
		private static string <PawnKindsWeaponUsage>m__15(PawnKindDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private static string <PawnKindsWeaponUsage>m__16(PawnKindDef x)
		{
			return x.weaponMoney.Average.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsWeaponUsage>m__17(PawnKindDef x)
		{
			return x.weaponMoney.min.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsWeaponUsage>m__18(PawnKindDef x)
		{
			return x.weaponMoney.max.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsWeaponUsage>m__19(PawnKindDef x)
		{
			return x.combatPower.ToString();
		}

		[CompilerGenerated]
		private static bool <PawnKindsWeaponUsage>m__1A(ThingDef w)
		{
			return w.IsWeapon && !w.weaponTags.NullOrEmpty<string>();
		}

		[CompilerGenerated]
		private static bool <PawnKindsWeaponUsage>m__1B(ThingDef w)
		{
			return w.IsMeleeWeapon;
		}

		[CompilerGenerated]
		private static TechLevel <PawnKindsWeaponUsage>m__1C(ThingDef w)
		{
			return w.techLevel;
		}

		[CompilerGenerated]
		private static float <PawnKindsWeaponUsage>m__1D(ThingDef w)
		{
			return w.BaseMarketValue;
		}

		[CompilerGenerated]
		private static TableDataGetter<PawnKindDef> <PawnKindsWeaponUsage>m__1E(ThingDef w)
		{
			return new TableDataGetter<PawnKindDef>(w.label.Shorten() + "\n$" + w.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
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
			});
		}

		[CompilerGenerated]
		private static bool <PawnKindsWeaponUsage>m__1F(PawnKindDef x)
		{
			return x.RaceProps.intelligence >= Intelligence.ToolUser;
		}

		[CompilerGenerated]
		private static int <PawnKindsWeaponUsage>m__20(PawnKindDef x)
		{
			return (x.defaultFactionType == null) ? int.MaxValue : ((int)x.defaultFactionType.techLevel);
		}

		[CompilerGenerated]
		private static float <PawnKindsWeaponUsage>m__21(PawnKindDef x)
		{
			return x.combatPower;
		}

		[CompilerGenerated]
		private static string <PawnKindsApparelUsage>m__22(PawnKindDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private static string <PawnKindsApparelUsage>m__23(PawnKindDef x)
		{
			return x.apparelMoney.Average.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsApparelUsage>m__24(PawnKindDef x)
		{
			return x.apparelMoney.min.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsApparelUsage>m__25(PawnKindDef x)
		{
			return x.apparelMoney.max.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsApparelUsage>m__26(PawnKindDef x)
		{
			return x.combatPower.ToString();
		}

		[CompilerGenerated]
		private static bool <PawnKindsApparelUsage>m__27(ThingDef a)
		{
			return a.IsApparel;
		}

		[CompilerGenerated]
		private static bool <PawnKindsApparelUsage>m__28(ThingDef a)
		{
			return PawnApparelGenerator.IsHeadgear(a);
		}

		[CompilerGenerated]
		private static float <PawnKindsApparelUsage>m__29(ThingDef a)
		{
			return a.BaseMarketValue;
		}

		[CompilerGenerated]
		private static TableDataGetter<PawnKindDef> <PawnKindsApparelUsage>m__2A(ThingDef a)
		{
			return new TableDataGetter<PawnKindDef>(a.label.Shorten() + "\n$" + a.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
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
			});
		}

		[CompilerGenerated]
		private static bool <PawnKindsApparelUsage>m__2B(PawnKindDef x)
		{
			return x.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static int <PawnKindsApparelUsage>m__2C(PawnKindDef x)
		{
			return (x.defaultFactionType == null) ? int.MaxValue : ((int)x.defaultFactionType.techLevel);
		}

		[CompilerGenerated]
		private static float <PawnKindsApparelUsage>m__2D(PawnKindDef x)
		{
			return x.combatPower;
		}

		[CompilerGenerated]
		private static string <PawnKindsTechHediffUsage>m__2E(PawnKindDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private static string <PawnKindsTechHediffUsage>m__2F(PawnKindDef x)
		{
			return x.techHediffsChance.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <PawnKindsTechHediffUsage>m__30(PawnKindDef x)
		{
			return x.techHediffsMoney.Average.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsTechHediffUsage>m__31(PawnKindDef x)
		{
			return x.techHediffsMoney.min.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsTechHediffUsage>m__32(PawnKindDef x)
		{
			return x.techHediffsMoney.max.ToString();
		}

		[CompilerGenerated]
		private static string <PawnKindsTechHediffUsage>m__33(PawnKindDef x)
		{
			return x.combatPower.ToString();
		}

		[CompilerGenerated]
		private static bool <PawnKindsTechHediffUsage>m__34(ThingDef t)
		{
			return t.isTechHediff && t.techHediffsTags != null;
		}

		[CompilerGenerated]
		private static TechLevel <PawnKindsTechHediffUsage>m__35(ThingDef t)
		{
			return t.techLevel;
		}

		[CompilerGenerated]
		private static float <PawnKindsTechHediffUsage>m__36(ThingDef t)
		{
			return t.BaseMarketValue;
		}

		[CompilerGenerated]
		private static TableDataGetter<PawnKindDef> <PawnKindsTechHediffUsage>m__37(ThingDef t)
		{
			return new TableDataGetter<PawnKindDef>(t.label.Shorten() + "\n$" + t.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
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
			});
		}

		[CompilerGenerated]
		private static bool <PawnKindsTechHediffUsage>m__38(PawnKindDef x)
		{
			return x.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static int <PawnKindsTechHediffUsage>m__39(PawnKindDef x)
		{
			return (x.defaultFactionType == null) ? int.MaxValue : ((int)x.defaultFactionType.techLevel);
		}

		[CompilerGenerated]
		private static float <PawnKindsTechHediffUsage>m__3A(PawnKindDef x)
		{
			return x.combatPower;
		}

		[CompilerGenerated]
		private static bool <PawnKindGearSampled>m__3B(PawnKindDef k)
		{
			return k.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static float <PawnKindGearSampled>m__3C(PawnKindDef k)
		{
			return k.combatPower;
		}

		[CompilerGenerated]
		private static bool <PawnWorkDisablesSampled>m__3D(PawnKindDef k)
		{
			return k.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static float <PawnWorkDisablesSampled>m__3E(PawnKindDef k)
		{
			return k.combatPower;
		}

		[CompilerGenerated]
		private static string <LivePawnsInspirationChances>m__3F(Pawn p)
		{
			return p.Label;
		}

		[CompilerGenerated]
		private static string <RacesFoodConsumption>m__40(ThingDef d, int lsIndex)
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
		}

		[CompilerGenerated]
		private static string <RacesFoodConsumption>m__41(ThingDef d, int lsIndex)
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
		}

		[CompilerGenerated]
		private static string <RacesFoodConsumption>m__42(ThingDef d, int lsIndex)
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
		}

		[CompilerGenerated]
		private static bool <RacesFoodConsumption>m__43(ThingDef d)
		{
			return d.race != null && d.race.EatsFood;
		}

		[CompilerGenerated]
		private static float <RacesFoodConsumption>m__44(ThingDef d)
		{
			return d.race.baseHungerRate;
		}

		[CompilerGenerated]
		private static string <RacesFoodConsumption>m__45(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static bool <RacesButchery>m__46(ThingDef d)
		{
			return d.race != null;
		}

		[CompilerGenerated]
		private static float <RacesButchery>m__47(ThingDef d)
		{
			return d.race.baseBodySize;
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__48(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__49(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0");
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__4A(ThingDef d)
		{
			return d.race.baseHealthScale.ToString("F2");
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__4B(ThingDef d)
		{
			return d.race.baseHungerRate.ToString("F2");
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__4C(ThingDef d)
		{
			return d.race.wildness.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__4D(ThingDef d)
		{
			return d.race.baseBodySize.ToString("F2");
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__4E(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null).ToString("F0");
		}

		[CompilerGenerated]
		private static string <RacesButchery>m__4F(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.LeatherAmount, null).ToString("F0");
		}

		[CompilerGenerated]
		private static float <AnimalsBasics>m__50(PawnKindDef d)
		{
			return DebugOutputsPawns.RaceMeleeDpsEstimate(d.race);
		}

		[CompilerGenerated]
		private static bool <AnimalsBasics>m__51(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__52(PawnKindDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__53(PawnKindDef d)
		{
			return d.RaceProps.baseHealthScale.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__54(PawnKindDef d)
		{
			return d.combatPower.ToString("F0");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__55(PawnKindDef d)
		{
			return d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__56(PawnKindDef d)
		{
			return d.race.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__57(PawnKindDef d)
		{
			return d.RaceProps.baseBodySize.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__58(PawnKindDef d)
		{
			return d.RaceProps.baseHungerRate.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__59(PawnKindDef d)
		{
			return d.RaceProps.wildness.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__5A(PawnKindDef d)
		{
			return d.RaceProps.lifeExpectancy.ToString("F1");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__5B(PawnKindDef d)
		{
			return (d.RaceProps.trainability == null) ? "null" : d.RaceProps.trainability.label;
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__5C(PawnKindDef d)
		{
			return d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0");
		}

		[CompilerGenerated]
		private static string <AnimalsBasics>m__5D(PawnKindDef d)
		{
			return d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0");
		}

		[CompilerGenerated]
		private static float <AnimalCombatBalance>m__5E(PawnKindDef k)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(k, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, true, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null));
			while (pawn.health.hediffSet.hediffs.Count > 0)
			{
				pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
			}
			float statValue = pawn.GetStatValue(StatDefOf.MeleeDPS, true);
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			return statValue;
		}

		[CompilerGenerated]
		private static float <AnimalCombatBalance>m__5F(PawnKindDef k)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(k, null);
			while (pawn.health.hediffSet.hediffs.Count > 0)
			{
				pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
			}
			float result = (pawn.GetStatValue(StatDefOf.ArmorRating_Blunt, true) + pawn.GetStatValue(StatDefOf.ArmorRating_Sharp, true)) / 2f;
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			return result;
		}

		[CompilerGenerated]
		private static bool <AnimalCombatBalance>m__60(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static float <AnimalCombatBalance>m__61(PawnKindDef d)
		{
			return d.combatPower;
		}

		[CompilerGenerated]
		private static string <AnimalCombatBalance>m__62(PawnKindDef k)
		{
			return k.defName;
		}

		[CompilerGenerated]
		private static string <AnimalCombatBalance>m__63(PawnKindDef k)
		{
			return k.RaceProps.baseHealthScale.ToString();
		}

		[CompilerGenerated]
		private static string <AnimalCombatBalance>m__64(PawnKindDef k)
		{
			return k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString();
		}

		[CompilerGenerated]
		private static string <AnimalCombatBalance>m__65(PawnKindDef k)
		{
			return k.combatPower.ToString();
		}

		[CompilerGenerated]
		private static string <AnimalTradeTags>m__66(PawnKindDef k)
		{
			return k.defName;
		}

		[CompilerGenerated]
		private static bool <AnimalTradeTags>m__67(PawnKindDef k)
		{
			return k.race.tradeTags != null;
		}

		[CompilerGenerated]
		private static IEnumerable<string> <AnimalTradeTags>m__68(PawnKindDef k)
		{
			return k.race.tradeTags;
		}

		[CompilerGenerated]
		private static bool <AnimalTradeTags>m__69(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static bool <AnimalBehavior>m__6A(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__6B(PawnKindDef k)
		{
			return k.defName;
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__6C(PawnKindDef k)
		{
			return k.RaceProps.wildness.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__6D(PawnKindDef k)
		{
			return k.RaceProps.manhunterOnDamageChance.ToStringPercentEmptyZero("F1");
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__6E(PawnKindDef k)
		{
			return k.RaceProps.manhunterOnTameFailChance.ToStringPercentEmptyZero("F1");
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__6F(PawnKindDef k)
		{
			return k.RaceProps.predator.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__70(PawnKindDef k)
		{
			return k.RaceProps.baseBodySize.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__71(PawnKindDef k)
		{
			return (!k.RaceProps.predator) ? "" : k.RaceProps.maxPreyBodySize.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__72(PawnKindDef k)
		{
			return k.RaceProps.canBePredatorPrey.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__73(PawnKindDef k)
		{
			return k.RaceProps.petness.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__74(PawnKindDef k)
		{
			return (k.RaceProps.nuzzleMtbHours <= 0f) ? "" : k.RaceProps.nuzzleMtbHours.ToString();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__75(PawnKindDef k)
		{
			return k.RaceProps.packAnimal.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__76(PawnKindDef k)
		{
			return k.RaceProps.herdAnimal.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__77(PawnKindDef k)
		{
			return (k.wildGroupSize.min == 1) ? "" : k.wildGroupSize.min.ToString();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__78(PawnKindDef k)
		{
			return (k.wildGroupSize.max == 1) ? "" : k.wildGroupSize.max.ToString();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__79(PawnKindDef k)
		{
			return k.RaceProps.CanDoHerdMigration.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__7A(PawnKindDef k)
		{
			return k.RaceProps.herdMigrationAllowed.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <AnimalBehavior>m__7B(PawnKindDef k)
		{
			return k.RaceProps.mateMtbHours.ToStringEmptyZero("F0");
		}

		[CompilerGenerated]
		private static float <AnimalsEcosystem>m__7C(PawnKindDef k)
		{
			return k.RaceProps.baseBodySize * 0.2f + k.RaceProps.baseHungerRate * 0.8f;
		}

		[CompilerGenerated]
		private static bool <AnimalsEcosystem>m__7D(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static float <AnimalsEcosystem>m__7E(PawnKindDef d)
		{
			return d.ecoSystemWeight;
		}

		[CompilerGenerated]
		private static string <AnimalsEcosystem>m__7F(PawnKindDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <AnimalsEcosystem>m__80(PawnKindDef d)
		{
			return d.RaceProps.baseBodySize.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsEcosystem>m__81(PawnKindDef d)
		{
			return d.RaceProps.baseHungerRate.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsEcosystem>m__82(PawnKindDef d)
		{
			return d.ecoSystemWeight.ToString("F2");
		}

		[CompilerGenerated]
		private static string <AnimalsEcosystem>m__83(PawnKindDef d)
		{
			return d.RaceProps.predator.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private sealed class <PawnKindGearSampled>c__AnonStorey6
		{
			internal PawnKindDef kind;

			internal Faction fac;

			private static Func<HediffDef, bool> <>f__am$cache0;

			private static Func<HediffDef, bool> <>f__am$cache1;

			private static Func<HediffDef, bool> <>f__am$cache2;

			public <PawnKindGearSampled>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				DefMap<ThingDef, int> weapons = new DefMap<ThingDef, int>();
				DefMap<ThingDef, int> apparel = new DefMap<ThingDef, int>();
				DefMap<HediffDef, int> hediffs = new DefMap<HediffDef, int>();
				for (int i = 0; i < 400; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(this.kind, this.fac);
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
					this.kind.defName,
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
			}

			private static bool <>m__1(HediffDef h)
			{
				return h.spawnThingOnRemoved != null;
			}

			private static bool <>m__2(HediffDef h)
			{
				return h.IsAddiction;
			}

			private static bool <>m__3(HediffDef h)
			{
				return h.spawnThingOnRemoved == null && !h.IsAddiction;
			}

			private sealed class <PawnKindGearSampled>c__AnonStorey7
			{
				internal DefMap<ThingDef, int> weapons;

				internal DefMap<ThingDef, int> apparel;

				internal DefMap<HediffDef, int> hediffs;

				internal DebugOutputsPawns.<PawnKindGearSampled>c__AnonStorey6 <>f__ref$6;

				public <PawnKindGearSampled>c__AnonStorey7()
				{
				}

				internal int <>m__0(ThingDef t)
				{
					return this.weapons[t];
				}

				internal int <>m__1(ThingDef t)
				{
					return this.apparel[t];
				}

				internal int <>m__2(HediffDef h)
				{
					return this.hediffs[h];
				}

				internal int <>m__3(HediffDef h)
				{
					return this.hediffs[h];
				}

				internal int <>m__4(HediffDef h)
				{
					return this.hediffs[h];
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PawnWorkDisablesSampled>c__AnonStorey8
		{
			internal PawnKindDef kind;

			internal Faction fac;

			public <PawnWorkDisablesSampled>c__AnonStorey8()
			{
			}

			internal void <>m__0()
			{
				Dictionary<WorkTags, int> dictionary = new Dictionary<WorkTags, int>();
				for (int i = 0; i < 1000; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(this.kind, this.fac);
					WorkTags combinedDisabledWorkTags = pawn.story.CombinedDisabledWorkTags;
					IEnumerator enumerator = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							WorkTags workTags = (WorkTags)obj;
							if (!dictionary.ContainsKey(workTags))
							{
								dictionary.Add(workTags, 0);
							}
							if ((combinedDisabledWorkTags & workTags) != WorkTags.None)
							{
								Dictionary<WorkTags, int> dictionary2;
								WorkTags key;
								(dictionary2 = dictionary)[key = workTags] = dictionary2[key] + 1;
							}
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
					pawn.Destroy(DestroyMode.Vanish);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Sampled ",
					1000,
					"x ",
					this.kind.defName,
					":"
				}));
				stringBuilder.AppendLine("Worktags disabled");
				IEnumerator enumerator2 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						WorkTags key2 = (WorkTags)obj2;
						int num = dictionary[key2];
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  ",
							key2.ToString(),
							"    ",
							num,
							" (",
							((float)num / 1000f).ToStringPercent(),
							")"
						}));
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				Log.Message(stringBuilder.ToString().TrimEndNewlines(), false);
			}
		}

		[CompilerGenerated]
		private sealed class <LivePawnsInspirationChances>c__AnonStorey9
		{
			internal InspirationDef iDef;

			public <LivePawnsInspirationChances>c__AnonStorey9()
			{
			}

			internal string <>m__0(Pawn p)
			{
				return this.iDef.Worker.InspirationCanOccur(p) ? this.iDef.Worker.CommonalityFor(p).ToString() : "-no-";
			}
		}

		[CompilerGenerated]
		private sealed class <RacesFoodConsumption>c__AnonStoreyA
		{
			internal Func<ThingDef, int, string> lsName;

			internal Func<ThingDef, int, string> maxFood;

			internal Func<ThingDef, int, string> hungerRate;

			public <RacesFoodConsumption>c__AnonStoreyA()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.lsName(d, 0);
			}

			internal string <>m__1(ThingDef d)
			{
				return this.maxFood(d, 0);
			}

			internal string <>m__2(ThingDef d)
			{
				return this.hungerRate(d, 0);
			}

			internal string <>m__3(ThingDef d)
			{
				return this.lsName(d, 1);
			}

			internal string <>m__4(ThingDef d)
			{
				return this.maxFood(d, 1);
			}

			internal string <>m__5(ThingDef d)
			{
				return this.hungerRate(d, 1);
			}

			internal string <>m__6(ThingDef d)
			{
				return this.lsName(d, 2);
			}

			internal string <>m__7(ThingDef d)
			{
				return this.maxFood(d, 2);
			}

			internal string <>m__8(ThingDef d)
			{
				return this.hungerRate(d, 2);
			}

			internal string <>m__9(ThingDef d)
			{
				return this.lsName(d, 3);
			}

			internal string <>m__A(ThingDef d)
			{
				return this.maxFood(d, 3);
			}

			internal string <>m__B(ThingDef d)
			{
				return this.hungerRate(d, 3);
			}
		}

		[CompilerGenerated]
		private sealed class <AnimalsBasics>c__AnonStoreyB
		{
			internal Func<PawnKindDef, float> dps;

			internal Func<PawnKindDef, float> pointsGuess;

			internal Func<PawnKindDef, float> mktValGuess;

			public <AnimalsBasics>c__AnonStoreyB()
			{
			}

			internal float <>m__0(PawnKindDef d)
			{
				float num = 15f;
				num += this.dps(d) * 10f;
				num *= Mathf.Lerp(1f, d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 3f, 0.25f);
				num *= d.RaceProps.baseHealthScale;
				num *= GenMath.LerpDouble(0.25f, 1f, 1.65f, 1f, Mathf.Clamp(d.RaceProps.baseBodySize, 0.25f, 1f));
				return num * 0.76f;
			}

			internal float <>m__1(PawnKindDef d)
			{
				float num = 18f;
				num += this.pointsGuess(d) * 2.7f;
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
			}

			internal string <>m__2(PawnKindDef d)
			{
				return this.dps(d).ToString("F2");
			}

			internal string <>m__3(PawnKindDef d)
			{
				return this.pointsGuess(d).ToString("F0");
			}

			internal string <>m__4(PawnKindDef d)
			{
				return this.mktValGuess(d).ToString("F0");
			}
		}

		[CompilerGenerated]
		private sealed class <AnimalCombatBalance>c__AnonStoreyC
		{
			internal Func<PawnKindDef, float> meleeDps;

			internal Func<PawnKindDef, float> averageArmor;

			internal Func<PawnKindDef, float> combatPowerCalculated;

			public <AnimalCombatBalance>c__AnonStoreyC()
			{
			}

			internal float <>m__0(PawnKindDef k)
			{
				float num = 1f + this.meleeDps(k) * 2f;
				float num2 = 1f + (k.RaceProps.baseHealthScale + this.averageArmor(k) * 1.8f) * 2f;
				float num3 = num * num2 * 2.5f + 10f;
				return num3 + k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) * 2f;
			}

			internal string <>m__1(PawnKindDef k)
			{
				return this.meleeDps(k).ToString("F1");
			}

			internal string <>m__2(PawnKindDef k)
			{
				return this.averageArmor(k).ToStringPercent();
			}

			internal string <>m__3(PawnKindDef k)
			{
				return this.combatPowerCalculated(k).ToString("F0");
			}
		}

		[CompilerGenerated]
		private sealed class <AnimalTradeTags>c__AnonStoreyD
		{
			internal string tag;

			public <AnimalTradeTags>c__AnonStoreyD()
			{
			}

			internal string <>m__0(PawnKindDef k)
			{
				return (k.race.tradeTags != null && k.race.tradeTags.Contains(this.tag)).ToStringCheckBlank();
			}
		}

		[CompilerGenerated]
		private sealed class <AnimalsEcosystem>c__AnonStoreyE
		{
			internal Func<PawnKindDef, float> ecosystemWeightGuess;

			public <AnimalsEcosystem>c__AnonStoreyE()
			{
			}

			internal string <>m__0(PawnKindDef d)
			{
				return this.ecosystemWeightGuess(d).ToString("F2");
			}
		}

		[CompilerGenerated]
		private sealed class <PawnKindsWeaponUsage>c__AnonStorey0
		{
			internal ThingDef w;

			public <PawnKindsWeaponUsage>c__AnonStorey0()
			{
			}

			internal string <>m__0(PawnKindDef k)
			{
				string result;
				if (k.weaponTags != null && this.w.weaponTags.Any((string z) => k.weaponTags.Contains(z)))
				{
					float num = PawnWeaponGenerator.CheapestNonDerpPriceFor(this.w);
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
			}

			private sealed class <PawnKindsWeaponUsage>c__AnonStorey1
			{
				internal PawnKindDef k;

				internal DebugOutputsPawns.<PawnKindsWeaponUsage>c__AnonStorey0 <>f__ref$0;

				public <PawnKindsWeaponUsage>c__AnonStorey1()
				{
				}

				internal bool <>m__0(string z)
				{
					return this.k.weaponTags.Contains(z);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PawnKindsApparelUsage>c__AnonStorey2
		{
			internal ThingDef a;

			public <PawnKindsApparelUsage>c__AnonStorey2()
			{
			}

			internal string <>m__0(PawnKindDef k)
			{
				string result;
				if (k.apparelRequired != null && k.apparelRequired.Contains(this.a))
				{
					result = "Rq";
				}
				else if (k.apparelAllowHeadgearChance <= 0f && PawnApparelGenerator.IsHeadgear(this.a))
				{
					result = "nohat";
				}
				else if (k.apparelTags != null && this.a.apparel.tags.Any((string z) => k.apparelTags.Contains(z)))
				{
					float baseMarketValue = this.a.BaseMarketValue;
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
			}

			private sealed class <PawnKindsApparelUsage>c__AnonStorey3
			{
				internal PawnKindDef k;

				internal DebugOutputsPawns.<PawnKindsApparelUsage>c__AnonStorey2 <>f__ref$2;

				public <PawnKindsApparelUsage>c__AnonStorey3()
				{
				}

				internal bool <>m__0(string z)
				{
					return this.k.apparelTags.Contains(z);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PawnKindsTechHediffUsage>c__AnonStorey4
		{
			internal ThingDef t;

			public <PawnKindsTechHediffUsage>c__AnonStorey4()
			{
			}

			internal string <>m__0(PawnKindDef k)
			{
				string result;
				if (k.techHediffsTags != null && this.t.techHediffsTags.Any((string tag) => k.techHediffsTags.Contains(tag)))
				{
					if (k.techHediffsMoney.max < this.t.BaseMarketValue)
					{
						result = "-";
					}
					else if (k.techHediffsMoney.min >= this.t.BaseMarketValue)
					{
						result = "✓";
					}
					else
					{
						result = (1f - (this.t.BaseMarketValue - k.techHediffsMoney.min) / (k.techHediffsMoney.max - k.techHediffsMoney.min)).ToStringPercent("F0");
					}
				}
				else
				{
					result = "";
				}
				return result;
			}

			private sealed class <PawnKindsTechHediffUsage>c__AnonStorey5
			{
				internal PawnKindDef k;

				internal DebugOutputsPawns.<PawnKindsTechHediffUsage>c__AnonStorey4 <>f__ref$4;

				public <PawnKindsTechHediffUsage>c__AnonStorey5()
				{
				}

				internal bool <>m__0(string tag)
				{
					return this.k.techHediffsTags.Contains(tag);
				}
			}
		}
	}
}
