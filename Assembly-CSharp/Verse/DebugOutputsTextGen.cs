using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;

namespace Verse
{
	[HasDebugOutput]
	public static class DebugOutputsTextGen
	{
		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache4;

		[CompilerGenerated]
		private static Action <>f__am$cache5;

		[CompilerGenerated]
		private static Action <>f__am$cache6;

		[CompilerGenerated]
		private static Action <>f__am$cache7;

		[CompilerGenerated]
		private static Func<RulePackDef, bool> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<RulePackDef, bool> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<TaleDef, string> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<FactionDef, RulePackDef> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<FactionDef, RulePackDef> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<FactionDef, RulePackDef> <>f__am$cacheD;

		[CompilerGenerated]
		private static Func<FactionDef, RulePackDef> <>f__am$cacheE;

		[CompilerGenerated]
		private static Func<RulePackDef, bool> <>f__am$cacheF;

		[CompilerGenerated]
		private static Func<RulePackDef, bool> <>f__am$cache10;

		[CompilerGenerated]
		private static Func<RulePackDef, string> <>f__am$cache11;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache12;

		[CompilerGenerated]
		private static Action<Action<List<BodyPartRecord>, List<bool>>> <>f__am$cache13;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache14;

		[CompilerGenerated]
		private static Action<Action<List<BodyPartRecord>, List<bool>>> <>f__am$cache15;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache16;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache17;

		[Category("Text generation")]
		[DebugOutput]
		public static void FlavorfulCombatTest()
		{
			DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1 <FlavorfulCombatTest>c__AnonStorey = new DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			<FlavorfulCombatTest>c__AnonStorey.maneuvers = DefDatabase<ManeuverDef>.AllDefsListForReading;
			DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1 <FlavorfulCombatTest>c__AnonStorey2 = <FlavorfulCombatTest>c__AnonStorey;
			Func<ManeuverDef, RulePackDef>[] array = new Func<ManeuverDef, RulePackDef>[5];
			array[0] = ((ManeuverDef m) => new RulePackDef[]
			{
				m.combatLogRulesHit,
				m.combatLogRulesDeflect,
				m.combatLogRulesMiss,
				m.combatLogRulesDodge
			}.RandomElement<RulePackDef>());
			array[1] = ((ManeuverDef m) => m.combatLogRulesHit);
			array[2] = ((ManeuverDef m) => m.combatLogRulesDeflect);
			array[3] = ((ManeuverDef m) => m.combatLogRulesMiss);
			array[4] = ((ManeuverDef m) => m.combatLogRulesDodge);
			<FlavorfulCombatTest>c__AnonStorey2.results = array;
			string[] array2 = new string[]
			{
				"(random)",
				"Hit",
				"Deflect",
				"Miss",
				"Dodge"
			};
			using (IEnumerator<Pair<ManeuverDef, int>> enumerator = <FlavorfulCombatTest>c__AnonStorey.maneuvers.Concat(null).Cross(Enumerable.Range(0, array2.Length)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pair<ManeuverDef, int> maneuverresult = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(string.Format("{0}/{1}", (maneuverresult.First != null) ? maneuverresult.First.defName : "(random)", array2[maneuverresult.Second]), DebugMenuOptionMode.Action, delegate()
					{
						DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < 100; i++)
							{
								ManeuverDef maneuver = maneuverresult.First;
								if (maneuver == null)
								{
									maneuver = <FlavorfulCombatTest>c__AnonStorey.maneuvers.RandomElement<ManeuverDef>();
								}
								RulePackDef rulePackDef = <FlavorfulCombatTest>c__AnonStorey.results[maneuverresult.Second](maneuver);
								List<BodyPartRecord> list2 = null;
								List<bool> list3 = null;
								if (rulePackDef == maneuver.combatLogRulesHit)
								{
									list2 = new List<BodyPartRecord>();
									list3 = new List<bool>();
									bodyPartCreator(list2, list3);
								}
								Pair<ThingDef, Tool> pair = (from ttp in (from td in DefDatabase<ThingDef>.AllDefsListForReading
								where td.IsMeleeWeapon && !td.tools.NullOrEmpty<Tool>()
								select td).SelectMany((ThingDef td) => from tool in td.tools
								select new Pair<ThingDef, Tool>(td, tool))
								where ttp.Second.capacities.Contains(maneuver.requiredCapacity)
								select ttp).RandomElement<Pair<ThingDef, Tool>>();
								BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackDef, false, CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), (pair.Second == null) ? ImplementOwnerTypeDefOf.Bodypart : ImplementOwnerTypeDefOf.Weapon, (pair.Second == null) ? "body part" : pair.Second.label, pair.First, null, null);
								battleLogEntry_MeleeCombat.FillTargets(list2, list3, battleLogEntry_MeleeCombat.RuleDef.defName.Contains("Deflect"));
								battleLogEntry_MeleeCombat.Debug_OverrideTicks(Rand.Int);
								stringBuilder.AppendLine(battleLogEntry_MeleeCombat.ToGameStringFromPOV(null, false));
							}
							Log.Message(stringBuilder.ToString(), false);
						});
					});
					list.Add(item);
				}
			}
			int rf;
			for (rf = 0; rf < 2; rf++)
			{
				list.Add(new DebugMenuOption((rf != 0) ? "Ranged fire burst" : "Ranged fire singleshot", DebugMenuOptionMode.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef thingDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon
						select td).RandomElement<ThingDef>();
						bool flag = Rand.Value < 0.2f;
						bool flag2 = !flag && Rand.Value < 0.95f;
						BattleLogEntry_RangedFire battleLogEntry_RangedFire = new BattleLogEntry_RangedFire(CombatLogTester.GenerateRandom(), (!flag) ? CombatLogTester.GenerateRandom() : null, (!flag2) ? thingDef : null, null, rf != 0);
						battleLogEntry_RangedFire.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedFire.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString(), false);
				}));
			}
			list.Add(new DebugMenuOption("Ranged impact hit", DebugMenuOptionMode.Action, delegate()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon
						select td).RandomElement<ThingDef>();
						List<BodyPartRecord> list2 = new List<BodyPartRecord>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						Pawn pawn = CombatLogTester.GenerateRandom();
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), pawn, pawn, weaponDef, null, ThingDefOf.Wall);
						battleLogEntry_RangedImpact.FillTargets(list2, list3, Rand.Chance(0.5f));
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString(), false);
				});
			}));
			list.Add(new DebugMenuOption("Ranged impact miss", DebugMenuOptionMode.Action, delegate()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon
					select td).RandomElement<ThingDef>();
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), null, CombatLogTester.GenerateRandom(), weaponDef, null, ThingDefOf.Wall);
					battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			}));
			list.Add(new DebugMenuOption("Ranged impact hit incorrect", DebugMenuOptionMode.Action, delegate()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon
						select td).RandomElement<ThingDef>();
						List<BodyPartRecord> list2 = new List<BodyPartRecord>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), weaponDef, null, ThingDefOf.Wall);
						battleLogEntry_RangedImpact.FillTargets(list2, list3, Rand.Chance(0.5f));
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString(), false);
				});
			}));
			using (IEnumerator<RulePackDef> enumerator2 = (from def in DefDatabase<RulePackDef>.AllDefsListForReading
			where def.defName.Contains("Transition") && !def.defName.Contains("Include")
			select def).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RulePackDef transition = enumerator2.Current;
					list.Add(new DebugMenuOption(transition.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < 100; i++)
						{
							Pawn pawn = CombatLogTester.GenerateRandom();
							Pawn initiator = CombatLogTester.GenerateRandom();
							BodyPartRecord partRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).RandomElement<BodyPartRecord>();
							BattleLogEntry_StateTransition battleLogEntry_StateTransition = new BattleLogEntry_StateTransition(pawn, transition, initiator, HediffMaker.MakeHediff(DefDatabase<HediffDef>.AllDefsListForReading.RandomElement<HediffDef>(), pawn, partRecord), pawn.RaceProps.body.AllParts.RandomElement<BodyPartRecord>());
							battleLogEntry_StateTransition.Debug_OverrideTicks(Rand.Int);
							stringBuilder.AppendLine(battleLogEntry_StateTransition.ToGameStringFromPOV(null, false));
						}
						Log.Message(stringBuilder.ToString(), false);
					}));
				}
			}
			using (IEnumerator<RulePackDef> enumerator3 = (from def in DefDatabase<RulePackDef>.AllDefsListForReading
			where def.defName.Contains("DamageEvent") && !def.defName.Contains("Include")
			select def).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					RulePackDef damageEvent = enumerator3.Current;
					list.Add(new DebugMenuOption(damageEvent.defName, DebugMenuOptionMode.Action, delegate()
					{
						DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < 100; i++)
							{
								List<BodyPartRecord> list2 = new List<BodyPartRecord>();
								List<bool> list3 = new List<bool>();
								bodyPartCreator(list2, list3);
								Pawn recipient = CombatLogTester.GenerateRandom();
								BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(recipient, damageEvent, null);
								battleLogEntry_DamageTaken.FillTargets(list2, list3, false);
								battleLogEntry_DamageTaken.Debug_OverrideTicks(Rand.Int);
								stringBuilder.AppendLine(battleLogEntry_DamageTaken.ToGameStringFromPOV(null, false));
							}
							Log.Message(stringBuilder.ToString(), false);
						});
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		private static void CreateDamagedDestroyedMenu(Action<Action<List<BodyPartRecord>, List<bool>>> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<int> damagedes = Enumerable.Range(0, 5);
			IEnumerable<int> destroyedes = Enumerable.Range(0, 5);
			using (IEnumerator<Pair<int, int>> enumerator = damagedes.Concat(-1).Cross(destroyedes.Concat(-1)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pair<int, int> damageddestroyed = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(string.Format("{0} damaged/{1} destroyed", (damageddestroyed.First != -1) ? damageddestroyed.First.ToString() : "(random)", (damageddestroyed.Second != -1) ? damageddestroyed.Second.ToString() : "(random)"), DebugMenuOptionMode.Action, delegate()
					{
						callback(delegate(List<BodyPartRecord> bodyparts, List<bool> flags)
						{
							int num = damageddestroyed.First;
							int destroyed = damageddestroyed.Second;
							if (num == -1)
							{
								num = damagedes.RandomElement<int>();
							}
							if (destroyed == -1)
							{
								destroyed = destroyedes.RandomElement<int>();
							}
							Pair<BodyPartRecord, bool>[] source = (from idx in Enumerable.Range(0, num + destroyed)
							select new Pair<BodyPartRecord, bool>(BodyDefOf.Human.AllParts.RandomElement<BodyPartRecord>(), idx < destroyed)).InRandomOrder(null).ToArray<Pair<BodyPartRecord, bool>>();
							bodyparts.Clear();
							flags.Clear();
							bodyparts.AddRange(from part in source
							select part.First);
							flags.AddRange(from part in source
							select part.Second);
						});
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[Category("Text generation")]
		[DebugOutput]
		public static void ArtDescsSpecificTale()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TaleDef localDef2 in from def in DefDatabase<TaleDef>.AllDefs
			orderby def.defName
			select def)
			{
				TaleDef localDef = localDef2;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate()
				{
					DebugOutputsTextGen.LogSpecificTale(localDef, 40);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[Category("Text generation")]
		[DebugOutput]
		public static void NamesFromRulepack()
		{
			IEnumerable<RulePackDef> first = from f in DefDatabase<FactionDef>.AllDefsListForReading
			select f.factionNameMaker;
			IEnumerable<RulePackDef> second = from f in DefDatabase<FactionDef>.AllDefsListForReading
			select f.settlementNameMaker;
			IEnumerable<RulePackDef> second2 = from f in DefDatabase<FactionDef>.AllDefsListForReading
			select f.playerInitialSettlementNameMaker;
			IEnumerable<RulePackDef> second3 = from f in DefDatabase<FactionDef>.AllDefsListForReading
			select f.pawnNameMaker;
			IEnumerable<RulePackDef> second4 = from d in DefDatabase<RulePackDef>.AllDefsListForReading
			where d.defName.Contains("Namer")
			select d;
			IOrderedEnumerable<RulePackDef> orderedEnumerable = from d in (from d in first.Concat(second).Concat(second2).Concat(second3).Concat(second4)
			where d != null
			select d).Distinct<RulePackDef>()
			orderby d.defName
			select d;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (RulePackDef localNamer3 in orderedEnumerable)
			{
				RulePackDef localNamer = localNamer3;
				FloatMenuOption item = new FloatMenuOption(localNamer.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Testing RulePack " + localNamer.defName + " as  a name generator:");
					for (int i = 0; i < 200; i++)
					{
						string text = (i % 2 != 0) ? null : "Smithee";
						StringBuilder stringBuilder2 = stringBuilder;
						RulePackDef localNamer2 = localNamer;
						string testPawnNameSymbol = text;
						stringBuilder2.AppendLine(NameGenerator.GenerateName(localNamer2, null, false, null, testPawnNameSymbol));
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[Category("Text generation")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void DatabaseTalesList()
		{
			Find.TaleManager.LogTales();
		}

		[Category("Text generation")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void DatabaseTalesInterest()
		{
			Find.TaleManager.LogTaleInterestSummary();
		}

		[Category("Text generation")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void ArtDescsDatabaseTales()
		{
			DebugOutputsTextGen.LogTales(from t in Find.TaleManager.AllTalesListForReading
			where t.def.usableForArt
			select t);
		}

		[Category("Text generation")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void ArtDescsRandomTales()
		{
			int num = 40;
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < num; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(null));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		[Category("Text generation")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void ArtDescsTaleless()
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < 20; i++)
			{
				list.Add(null);
			}
			DebugOutputsTextGen.LogTales(list);
		}

		[Category("Text generation")]
		[DebugOutput]
		public static void InteractionLogs()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<InteractionDef>.Enumerator enumerator = DefDatabase<InteractionDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InteractionDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null);
						Pawn recipient = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null);
						for (int i = 0; i < 100; i++)
						{
							PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(def, pawn, recipient, null);
							stringBuilder.AppendLine(playLogEntry_Interaction.ToGameStringFromPOV(pawn, false));
						}
						Log.Message(stringBuilder.ToString(), false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		private static void LogSpecificTale(TaleDef def, int count)
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < count; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(def));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		private static void LogTales(IEnumerable<Tale> tales)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (Tale tale in tales)
			{
				TaleReference tr = new TaleReference(tale);
				stringBuilder.AppendLine(DebugOutputsTextGen.RandomArtworkName(tr));
				stringBuilder.AppendLine(DebugOutputsTextGen.RandomArtworkDescription(tr));
				stringBuilder.AppendLine();
				num++;
				if (num % 20 == 0)
				{
					Log.Message(stringBuilder.ToString(), false);
					stringBuilder = new StringBuilder();
				}
			}
			if (!stringBuilder.ToString().NullOrEmpty())
			{
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		private static string RandomArtworkName(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.NamerArtSculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.NamerArtWeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.NamerArtWeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.NamerArtFurniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.NamerArtSarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtName, extraInclude);
		}

		private static string RandomArtworkDescription(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.ArtDescription_Sculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.ArtDescription_WeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.ArtDescription_WeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.ArtDescription_Furniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.ArtDescription_SarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtDescription, extraInclude);
		}

		[CompilerGenerated]
		private static RulePackDef <FlavorfulCombatTest>m__0(ManeuverDef m)
		{
			return new RulePackDef[]
			{
				m.combatLogRulesHit,
				m.combatLogRulesDeflect,
				m.combatLogRulesMiss,
				m.combatLogRulesDodge
			}.RandomElement<RulePackDef>();
		}

		[CompilerGenerated]
		private static RulePackDef <FlavorfulCombatTest>m__1(ManeuverDef m)
		{
			return m.combatLogRulesHit;
		}

		[CompilerGenerated]
		private static RulePackDef <FlavorfulCombatTest>m__2(ManeuverDef m)
		{
			return m.combatLogRulesDeflect;
		}

		[CompilerGenerated]
		private static RulePackDef <FlavorfulCombatTest>m__3(ManeuverDef m)
		{
			return m.combatLogRulesMiss;
		}

		[CompilerGenerated]
		private static RulePackDef <FlavorfulCombatTest>m__4(ManeuverDef m)
		{
			return m.combatLogRulesDodge;
		}

		[CompilerGenerated]
		private static void <FlavorfulCombatTest>m__5()
		{
			DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon
					select td).RandomElement<ThingDef>();
					List<BodyPartRecord> list = new List<BodyPartRecord>();
					List<bool> list2 = new List<bool>();
					bodyPartCreator(list, list2);
					Pawn pawn = CombatLogTester.GenerateRandom();
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), pawn, pawn, weaponDef, null, ThingDefOf.Wall);
					battleLogEntry_RangedImpact.FillTargets(list, list2, Rand.Chance(0.5f));
					battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			});
		}

		[CompilerGenerated]
		private static void <FlavorfulCombatTest>m__6()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
				where td.IsRangedWeapon
				select td).RandomElement<ThingDef>();
				BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), null, CombatLogTester.GenerateRandom(), weaponDef, null, ThingDefOf.Wall);
				battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
				stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[CompilerGenerated]
		private static void <FlavorfulCombatTest>m__7()
		{
			DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon
					select td).RandomElement<ThingDef>();
					List<BodyPartRecord> list = new List<BodyPartRecord>();
					List<bool> list2 = new List<bool>();
					bodyPartCreator(list, list2);
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), weaponDef, null, ThingDefOf.Wall);
					battleLogEntry_RangedImpact.FillTargets(list, list2, Rand.Chance(0.5f));
					battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			});
		}

		[CompilerGenerated]
		private static bool <FlavorfulCombatTest>m__8(RulePackDef def)
		{
			return def.defName.Contains("Transition") && !def.defName.Contains("Include");
		}

		[CompilerGenerated]
		private static bool <FlavorfulCombatTest>m__9(RulePackDef def)
		{
			return def.defName.Contains("DamageEvent") && !def.defName.Contains("Include");
		}

		[CompilerGenerated]
		private static string <ArtDescsSpecificTale>m__A(TaleDef def)
		{
			return def.defName;
		}

		[CompilerGenerated]
		private static RulePackDef <NamesFromRulepack>m__B(FactionDef f)
		{
			return f.factionNameMaker;
		}

		[CompilerGenerated]
		private static RulePackDef <NamesFromRulepack>m__C(FactionDef f)
		{
			return f.settlementNameMaker;
		}

		[CompilerGenerated]
		private static RulePackDef <NamesFromRulepack>m__D(FactionDef f)
		{
			return f.playerInitialSettlementNameMaker;
		}

		[CompilerGenerated]
		private static RulePackDef <NamesFromRulepack>m__E(FactionDef f)
		{
			return f.pawnNameMaker;
		}

		[CompilerGenerated]
		private static bool <NamesFromRulepack>m__F(RulePackDef d)
		{
			return d.defName.Contains("Namer");
		}

		[CompilerGenerated]
		private static bool <NamesFromRulepack>m__10(RulePackDef d)
		{
			return d != null;
		}

		[CompilerGenerated]
		private static string <NamesFromRulepack>m__11(RulePackDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static bool <ArtDescsDatabaseTales>m__12(Tale t)
		{
			return t.def.usableForArt;
		}

		[CompilerGenerated]
		private static void <FlavorfulCombatTest>m__13(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
				where td.IsRangedWeapon
				select td).RandomElement<ThingDef>();
				List<BodyPartRecord> list = new List<BodyPartRecord>();
				List<bool> list2 = new List<bool>();
				bodyPartCreator(list, list2);
				Pawn pawn = CombatLogTester.GenerateRandom();
				BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), pawn, pawn, weaponDef, null, ThingDefOf.Wall);
				battleLogEntry_RangedImpact.FillTargets(list, list2, Rand.Chance(0.5f));
				battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
				stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[CompilerGenerated]
		private static bool <FlavorfulCombatTest>m__14(ThingDef td)
		{
			return td.IsRangedWeapon;
		}

		[CompilerGenerated]
		private static void <FlavorfulCombatTest>m__15(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
				where td.IsRangedWeapon
				select td).RandomElement<ThingDef>();
				List<BodyPartRecord> list = new List<BodyPartRecord>();
				List<bool> list2 = new List<bool>();
				bodyPartCreator(list, list2);
				BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), weaponDef, null, ThingDefOf.Wall);
				battleLogEntry_RangedImpact.FillTargets(list, list2, Rand.Chance(0.5f));
				battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
				stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[CompilerGenerated]
		private static bool <FlavorfulCombatTest>m__16(ThingDef td)
		{
			return td.IsRangedWeapon;
		}

		[CompilerGenerated]
		private static bool <FlavorfulCombatTest>m__17(ThingDef td)
		{
			return td.IsRangedWeapon;
		}

		[CompilerGenerated]
		private sealed class <FlavorfulCombatTest>c__AnonStorey1
		{
			internal IEnumerable<ManeuverDef> maneuvers;

			internal Func<ManeuverDef, RulePackDef>[] results;

			public <FlavorfulCombatTest>c__AnonStorey1()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <FlavorfulCombatTest>c__AnonStorey0
		{
			internal Pair<ManeuverDef, int> maneuverresult;

			internal DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1 <>f__ref$1;

			private static Func<ThingDef, bool> <>f__am$cache0;

			private static Func<ThingDef, IEnumerable<Pair<ThingDef, Tool>>> <>f__am$cache1;

			public <FlavorfulCombatTest>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1 <>f__ref$1 = this.<>f__ref$1;
						DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey0 <>f__ref$0 = this;
						ManeuverDef maneuver = this.maneuverresult.First;
						if (maneuver == null)
						{
							maneuver = this.<>f__ref$1.maneuvers.RandomElement<ManeuverDef>();
						}
						RulePackDef rulePackDef = this.<>f__ref$1.results[this.maneuverresult.Second](maneuver);
						List<BodyPartRecord> list = null;
						List<bool> list2 = null;
						if (rulePackDef == maneuver.combatLogRulesHit)
						{
							list = new List<BodyPartRecord>();
							list2 = new List<bool>();
							bodyPartCreator(list, list2);
						}
						Pair<ThingDef, Tool> pair = (from ttp in (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsMeleeWeapon && !td.tools.NullOrEmpty<Tool>()
						select td).SelectMany((ThingDef td) => from tool in td.tools
						select new Pair<ThingDef, Tool>(td, tool))
						where ttp.Second.capacities.Contains(maneuver.requiredCapacity)
						select ttp).RandomElement<Pair<ThingDef, Tool>>();
						BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackDef, false, CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), (pair.Second == null) ? ImplementOwnerTypeDefOf.Bodypart : ImplementOwnerTypeDefOf.Weapon, (pair.Second == null) ? "body part" : pair.Second.label, pair.First, null, null);
						battleLogEntry_MeleeCombat.FillTargets(list, list2, battleLogEntry_MeleeCombat.RuleDef.defName.Contains("Deflect"));
						battleLogEntry_MeleeCombat.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_MeleeCombat.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString(), false);
				});
			}

			internal void <>m__1(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1 <>f__ref$1 = this.<>f__ref$1;
					DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey0 <>f__ref$0 = this;
					ManeuverDef maneuver = this.maneuverresult.First;
					if (maneuver == null)
					{
						maneuver = this.<>f__ref$1.maneuvers.RandomElement<ManeuverDef>();
					}
					RulePackDef rulePackDef = this.<>f__ref$1.results[this.maneuverresult.Second](maneuver);
					List<BodyPartRecord> list = null;
					List<bool> list2 = null;
					if (rulePackDef == maneuver.combatLogRulesHit)
					{
						list = new List<BodyPartRecord>();
						list2 = new List<bool>();
						bodyPartCreator(list, list2);
					}
					Pair<ThingDef, Tool> pair = (from ttp in (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsMeleeWeapon && !td.tools.NullOrEmpty<Tool>()
					select td).SelectMany((ThingDef td) => from tool in td.tools
					select new Pair<ThingDef, Tool>(td, tool))
					where ttp.Second.capacities.Contains(maneuver.requiredCapacity)
					select ttp).RandomElement<Pair<ThingDef, Tool>>();
					BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackDef, false, CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), (pair.Second == null) ? ImplementOwnerTypeDefOf.Bodypart : ImplementOwnerTypeDefOf.Weapon, (pair.Second == null) ? "body part" : pair.Second.label, pair.First, null, null);
					battleLogEntry_MeleeCombat.FillTargets(list, list2, battleLogEntry_MeleeCombat.RuleDef.defName.Contains("Deflect"));
					battleLogEntry_MeleeCombat.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_MeleeCombat.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			}

			private static bool <>m__2(ThingDef td)
			{
				return td.IsMeleeWeapon && !td.tools.NullOrEmpty<Tool>();
			}

			private static IEnumerable<Pair<ThingDef, Tool>> <>m__3(ThingDef td)
			{
				return from tool in td.tools
				select new Pair<ThingDef, Tool>(td, tool);
			}

			private sealed class <FlavorfulCombatTest>c__AnonStorey3
			{
				internal ManeuverDef maneuver;

				internal DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey1 <>f__ref$1;

				internal DebugOutputsTextGen.<FlavorfulCombatTest>c__AnonStorey0 <>f__ref$0;

				public <FlavorfulCombatTest>c__AnonStorey3()
				{
				}

				internal bool <>m__0(Pair<ThingDef, Tool> ttp)
				{
					return ttp.Second.capacities.Contains(this.maneuver.requiredCapacity);
				}
			}

			private sealed class <FlavorfulCombatTest>c__AnonStorey2
			{
				internal ThingDef td;

				public <FlavorfulCombatTest>c__AnonStorey2()
				{
				}

				internal Pair<ThingDef, Tool> <>m__0(Tool tool)
				{
					return new Pair<ThingDef, Tool>(this.td, tool);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <FlavorfulCombatTest>c__AnonStorey4
		{
			internal int rf;

			private static Func<ThingDef, bool> <>f__am$cache0;

			public <FlavorfulCombatTest>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					ThingDef thingDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon
					select td).RandomElement<ThingDef>();
					bool flag = Rand.Value < 0.2f;
					bool flag2 = !flag && Rand.Value < 0.95f;
					BattleLogEntry_RangedFire battleLogEntry_RangedFire = new BattleLogEntry_RangedFire(CombatLogTester.GenerateRandom(), (!flag) ? CombatLogTester.GenerateRandom() : null, (!flag2) ? thingDef : null, null, this.rf != 0);
					battleLogEntry_RangedFire.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_RangedFire.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			}

			private static bool <>m__1(ThingDef td)
			{
				return td.IsRangedWeapon;
			}
		}

		[CompilerGenerated]
		private sealed class <FlavorfulCombatTest>c__AnonStorey5
		{
			internal RulePackDef transition;

			public <FlavorfulCombatTest>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					Pawn pawn = CombatLogTester.GenerateRandom();
					Pawn initiator = CombatLogTester.GenerateRandom();
					BodyPartRecord partRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).RandomElement<BodyPartRecord>();
					BattleLogEntry_StateTransition battleLogEntry_StateTransition = new BattleLogEntry_StateTransition(pawn, this.transition, initiator, HediffMaker.MakeHediff(DefDatabase<HediffDef>.AllDefsListForReading.RandomElement<HediffDef>(), pawn, partRecord), pawn.RaceProps.body.AllParts.RandomElement<BodyPartRecord>());
					battleLogEntry_StateTransition.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_StateTransition.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		[CompilerGenerated]
		private sealed class <FlavorfulCombatTest>c__AnonStorey6
		{
			internal RulePackDef damageEvent;

			public <FlavorfulCombatTest>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						List<BodyPartRecord> list = new List<BodyPartRecord>();
						List<bool> list2 = new List<bool>();
						bodyPartCreator(list, list2);
						Pawn recipient = CombatLogTester.GenerateRandom();
						BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(recipient, this.damageEvent, null);
						battleLogEntry_DamageTaken.FillTargets(list, list2, false);
						battleLogEntry_DamageTaken.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_DamageTaken.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString(), false);
				});
			}

			internal void <>m__1(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					List<BodyPartRecord> list = new List<BodyPartRecord>();
					List<bool> list2 = new List<bool>();
					bodyPartCreator(list, list2);
					Pawn recipient = CombatLogTester.GenerateRandom();
					BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(recipient, this.damageEvent, null);
					battleLogEntry_DamageTaken.FillTargets(list, list2, false);
					battleLogEntry_DamageTaken.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_DamageTaken.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		[CompilerGenerated]
		private sealed class <CreateDamagedDestroyedMenu>c__AnonStorey7
		{
			internal Action<Action<List<BodyPartRecord>, List<bool>>> callback;

			internal IEnumerable<int> damagedes;

			internal IEnumerable<int> destroyedes;

			public <CreateDamagedDestroyedMenu>c__AnonStorey7()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <CreateDamagedDestroyedMenu>c__AnonStorey8
		{
			internal Pair<int, int> damageddestroyed;

			internal DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey7 <>f__ref$7;

			private static Func<Pair<BodyPartRecord, bool>, BodyPartRecord> <>f__am$cache0;

			private static Func<Pair<BodyPartRecord, bool>, bool> <>f__am$cache1;

			public <CreateDamagedDestroyedMenu>c__AnonStorey8()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$7.callback(delegate(List<BodyPartRecord> bodyparts, List<bool> flags)
				{
					DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey7 <>f__ref$7 = this.<>f__ref$7;
					DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey8 <>f__ref$8 = this;
					int num = this.damageddestroyed.First;
					int destroyed = this.damageddestroyed.Second;
					if (num == -1)
					{
						num = this.<>f__ref$7.damagedes.RandomElement<int>();
					}
					if (destroyed == -1)
					{
						destroyed = this.<>f__ref$7.destroyedes.RandomElement<int>();
					}
					Pair<BodyPartRecord, bool>[] source = (from idx in Enumerable.Range(0, num + destroyed)
					select new Pair<BodyPartRecord, bool>(BodyDefOf.Human.AllParts.RandomElement<BodyPartRecord>(), idx < destroyed)).InRandomOrder(null).ToArray<Pair<BodyPartRecord, bool>>();
					bodyparts.Clear();
					flags.Clear();
					bodyparts.AddRange(from part in source
					select part.First);
					flags.AddRange(from part in source
					select part.Second);
				});
			}

			internal void <>m__1(List<BodyPartRecord> bodyparts, List<bool> flags)
			{
				DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey7 <>f__ref$7 = this.<>f__ref$7;
				DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey8 <>f__ref$8 = this;
				int num = this.damageddestroyed.First;
				int destroyed = this.damageddestroyed.Second;
				if (num == -1)
				{
					num = this.<>f__ref$7.damagedes.RandomElement<int>();
				}
				if (destroyed == -1)
				{
					destroyed = this.<>f__ref$7.destroyedes.RandomElement<int>();
				}
				Pair<BodyPartRecord, bool>[] source = (from idx in Enumerable.Range(0, num + destroyed)
				select new Pair<BodyPartRecord, bool>(BodyDefOf.Human.AllParts.RandomElement<BodyPartRecord>(), idx < destroyed)).InRandomOrder(null).ToArray<Pair<BodyPartRecord, bool>>();
				bodyparts.Clear();
				flags.Clear();
				bodyparts.AddRange(from part in source
				select part.First);
				flags.AddRange(from part in source
				select part.Second);
			}

			private static BodyPartRecord <>m__2(Pair<BodyPartRecord, bool> part)
			{
				return part.First;
			}

			private static bool <>m__3(Pair<BodyPartRecord, bool> part)
			{
				return part.Second;
			}

			private sealed class <CreateDamagedDestroyedMenu>c__AnonStorey9
			{
				internal int destroyed;

				internal DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey7 <>f__ref$7;

				internal DebugOutputsTextGen.<CreateDamagedDestroyedMenu>c__AnonStorey8 <>f__ref$8;

				public <CreateDamagedDestroyedMenu>c__AnonStorey9()
				{
				}

				internal Pair<BodyPartRecord, bool> <>m__0(int idx)
				{
					return new Pair<BodyPartRecord, bool>(BodyDefOf.Human.AllParts.RandomElement<BodyPartRecord>(), idx < this.destroyed);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ArtDescsSpecificTale>c__AnonStoreyA
		{
			internal TaleDef localDef;

			public <ArtDescsSpecificTale>c__AnonStoreyA()
			{
			}

			internal void <>m__0()
			{
				DebugOutputsTextGen.LogSpecificTale(this.localDef, 40);
			}
		}

		[CompilerGenerated]
		private sealed class <NamesFromRulepack>c__AnonStoreyB
		{
			internal RulePackDef localNamer;

			public <NamesFromRulepack>c__AnonStoreyB()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Testing RulePack " + this.localNamer.defName + " as  a name generator:");
				for (int i = 0; i < 200; i++)
				{
					string text = (i % 2 != 0) ? null : "Smithee";
					StringBuilder stringBuilder2 = stringBuilder;
					RulePackDef rootPack = this.localNamer;
					string testPawnNameSymbol = text;
					stringBuilder2.AppendLine(NameGenerator.GenerateName(rootPack, null, false, null, testPawnNameSymbol));
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		[CompilerGenerated]
		private sealed class <InteractionLogs>c__AnonStoreyC
		{
			internal InteractionDef def;

			public <InteractionLogs>c__AnonStoreyC()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null);
				Pawn recipient = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null);
				for (int i = 0; i < 100; i++)
				{
					PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(this.def, pawn, recipient, null);
					stringBuilder.AppendLine(playLogEntry_Interaction.ToGameStringFromPOV(pawn, false));
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}
	}
}
