using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E23 RID: 3619
	[HasDebugOutput]
	internal static class DebugOutputsTextGen
	{
		// Token: 0x060054A8 RID: 21672 RVA: 0x002B662C File Offset: 0x002B4A2C
		[DebugOutput]
		[Category("Text generation")]
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

		// Token: 0x060054A9 RID: 21673 RVA: 0x002B6A38 File Offset: 0x002B4E38
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

		// Token: 0x060054AA RID: 21674 RVA: 0x002B6B84 File Offset: 0x002B4F84
		[DebugOutput]
		[Category("Text generation")]
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

		// Token: 0x060054AB RID: 21675 RVA: 0x002B6C50 File Offset: 0x002B5050
		[DebugOutput]
		[Category("Text generation")]
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
			foreach (RulePackDef localNamer2 in orderedEnumerable)
			{
				RulePackDef localNamer = localNamer2;
				FloatMenuOption item = new FloatMenuOption(localNamer.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Testing RulePack " + localNamer.defName + " as  a name generator:");
					for (int i = 0; i < 200; i++)
					{
						stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, null, false, null));
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x002B6E34 File Offset: 0x002B5234
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("Text generation")]
		public static void DatabaseTalesList()
		{
			Find.TaleManager.LogTales();
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x002B6E41 File Offset: 0x002B5241
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("Text generation")]
		public static void DatabaseTalesInterest()
		{
			Find.TaleManager.LogTaleInterestSummary();
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x002B6E4E File Offset: 0x002B524E
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("Text generation")]
		public static void ArtDescsDatabaseTales()
		{
			DebugOutputsTextGen.LogTales(from t in Find.TaleManager.AllTalesListForReading
			where t.def.usableForArt
			select t);
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x002B6E84 File Offset: 0x002B5284
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("Text generation")]
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

		// Token: 0x060054B0 RID: 21680 RVA: 0x002B6EC4 File Offset: 0x002B52C4
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("Text generation")]
		public static void ArtDescsTaleless()
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < 20; i++)
			{
				list.Add(null);
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x002B6EF8 File Offset: 0x002B52F8
		[DebugOutput]
		[Category("Text generation")]
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

		// Token: 0x060054B2 RID: 21682 RVA: 0x002B6F94 File Offset: 0x002B5394
		private static void LogSpecificTale(TaleDef def, int count)
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < count; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(def));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x002B6FD0 File Offset: 0x002B53D0
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

		// Token: 0x060054B4 RID: 21684 RVA: 0x002B7094 File Offset: 0x002B5494
		private static string RandomArtworkName(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.ArtName_Sculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.ArtName_WeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.ArtName_WeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.ArtName_Furniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.ArtName_SarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtName, extraInclude);
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x002B7114 File Offset: 0x002B5514
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
	}
}
