using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Xml;
using Verse.AI;

namespace Verse
{
	public static class BackCompatibility
	{
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[1]
		{
			new Pair<int, int>(17, 18)
		};

		public static bool IsSaveCompatibleWith(string version)
		{
			bool result;
			if (VersionControl.BuildFromVersionString(version) == VersionControl.CurrentBuild)
			{
				result = true;
			}
			else if (((VersionControl.MajorFromVersionString(version) != 0) ? 1 : VersionControl.CurrentMajor) != 0)
			{
				result = false;
			}
			else
			{
				int num = VersionControl.MinorFromVersionString(version);
				int currentMinor = VersionControl.CurrentMinor;
				for (int i = 0; i < BackCompatibility.SaveCompatibleMinorVersions.Length; i++)
				{
					if (BackCompatibility.SaveCompatibleMinorVersions[i].First == num && BackCompatibility.SaveCompatibleMinorVersions[i].Second == currentMinor)
						goto IL_0075;
				}
				result = false;
			}
			goto IL_0095;
			IL_0095:
			return result;
			IL_0075:
			result = true;
			goto IL_0095;
		}

		public static string BackCompatibleDefName(Type defType, string defName)
		{
			string result;
			if (defType == typeof(ThingDef))
			{
				if (defName == "Gun_PDW")
				{
					result = "Gun_MachinePistol";
					goto IL_0c6b;
				}
				if (defName == "Bullet_PDW")
				{
					result = "Bullet_MachinePistol";
					goto IL_0c6b;
				}
				if (defName == "Components")
				{
					result = "Component";
					goto IL_0c6b;
				}
				if (defName == "Megatherium")
				{
					result = "Megasloth";
					goto IL_0c6b;
				}
				if (defName == "MegatheriumWool")
				{
					result = "MegaslothWool";
					goto IL_0c6b;
				}
				if (defName == "MalariBlock")
				{
					result = "Penoxycyline";
					goto IL_0c6b;
				}
				if (defName == "ArtilleryShell")
				{
					result = "MortarShell";
					goto IL_0c6b;
				}
				if (defName == "EquipmentRack")
				{
					result = "Shelf";
					goto IL_0c6b;
				}
				if (defName == "Apparel_MilitaryHelmet")
				{
					result = "Apparel_SimpleHelmet";
					goto IL_0c6b;
				}
				if (defName == "Apparel_KevlarHelmet")
				{
					result = "Apparel_AdvancedHelmet";
					goto IL_0c6b;
				}
				if (defName == "Apparel_PersonalShield")
				{
					result = "Apparel_ShieldBelt";
					goto IL_0c6b;
				}
				if (defName == "MuffaloWool")
				{
					result = "WoolMuffalo";
					goto IL_0c6b;
				}
				if (defName == "MegaslothWool")
				{
					result = "WoolMegasloth";
					goto IL_0c6b;
				}
				if (defName == "AlpacaWool")
				{
					result = "WoolAlpaca";
					goto IL_0c6b;
				}
				if (defName == "CamelHair")
				{
					result = "WoolCamel";
					goto IL_0c6b;
				}
				if (defName == "Gun_SurvivalRifle")
				{
					result = "Gun_BoltActionRifle";
					goto IL_0c6b;
				}
				if (defName == "Bullet_SurvivalRifle")
				{
					result = "Bullet_BoltActionRifle";
					goto IL_0c6b;
				}
				if (defName == "Neurotrainer")
				{
					result = "MechSerumNeurotrainer";
					goto IL_0c6b;
				}
				if (defName == "FueledGenerator")
				{
					result = "WoodFiredGenerator";
					goto IL_0c6b;
				}
				if (defName == "Gun_Pistol")
				{
					result = "Gun_Revolver";
					goto IL_0c6b;
				}
				if (defName == "Bullet_Pistol")
				{
					result = "Bullet_Revolver";
					goto IL_0c6b;
				}
				if (defName == "TableShort")
				{
					result = "Table2x2c";
					goto IL_0c6b;
				}
				if (defName == "TableLong")
				{
					result = "Table2x4c";
					goto IL_0c6b;
				}
				if (defName == "TableShort_Blueprint")
				{
					result = "Table2x2c_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "TableLong_Blueprint")
				{
					result = "Table2x4c_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "TableShort_Frame")
				{
					result = "Table2x2c_Frame";
					goto IL_0c6b;
				}
				if (defName == "TableLong_Frame")
				{
					result = "Table2x4c_Frame";
					goto IL_0c6b;
				}
				if (defName == "TableShort_Install")
				{
					result = "Table2x2c_Install";
					goto IL_0c6b;
				}
				if (defName == "TableLong_Install")
				{
					result = "Table2x4c_Install";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarBomb")
				{
					result = "Turret_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Turret_Incendiary")
				{
					result = "Turret_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarIncendiary")
				{
					result = "Turret_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Turret_EMP")
				{
					result = "Turret_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarEMP")
				{
					result = "Turret_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarBomb_Blueprint")
				{
					result = "Turret_Mortar_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "Turret_Incendiary_Blueprint")
				{
					result = "Turret_Mortar_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarIncendiary_Blueprint")
				{
					result = "Turret_Mortar_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "Turret_EMP_Blueprint")
				{
					result = "Turret_Mortar_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarEMP_Blueprint")
				{
					result = "Turret_Mortar_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarBomb_Frame")
				{
					result = "Turret_Mortar_Frame";
					goto IL_0c6b;
				}
				if (defName == "Turret_Incendiary_Frame")
				{
					result = "Turret_Mortar_Frame";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarIncendiary_Frame")
				{
					result = "Turret_Mortar_Frame";
					goto IL_0c6b;
				}
				if (defName == "Turret_EMP_Frame")
				{
					result = "Turret_Mortar_Frame";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarEMP_Frame")
				{
					result = "Turret_Mortar_Frame";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarBomb_Install")
				{
					result = "Turret_Mortar_Install";
					goto IL_0c6b;
				}
				if (defName == "Turret_Incendiary_Install")
				{
					result = "Turret_Mortar_Install";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarIncendiary_Install")
				{
					result = "Turret_Mortar_Install";
					goto IL_0c6b;
				}
				if (defName == "Turret_EMP_Install")
				{
					result = "Turret_Mortar_Install";
					goto IL_0c6b;
				}
				if (defName == "Turret_MortarEMP_Install")
				{
					result = "Turret_Mortar_Install";
					goto IL_0c6b;
				}
				if (defName == "Artillery_MortarBomb")
				{
					result = "Artillery_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Artillery_MortarIncendiary")
				{
					result = "Artillery_Mortar";
					goto IL_0c6b;
				}
				if (defName == "Artillery_MortarEMP")
				{
					result = "Artillery_Mortar";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDBomb")
				{
					result = "TrapIED_HighExplosive";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDIncendiary")
				{
					result = "TrapIED_Incendiary";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDBomb_Blueprint")
				{
					result = "TrapIED_HighExplosive_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDIncendiary_Blueprint")
				{
					result = "TrapIED_Incendiary_Blueprint";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDBomb_Frame")
				{
					result = "TrapIED_HighExplosive_Frame";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDIncendiary_Frame")
				{
					result = "TrapIED_Incendiary_Frame";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDBomb_Install")
				{
					result = "TrapIED_HighExplosive_Install";
					goto IL_0c6b;
				}
				if (defName == "TrapIEDIncendiary_Install")
				{
					result = "TrapIED_Incendiary_Install";
					goto IL_0c6b;
				}
				if (defName == "Bullet_MortarBomb")
				{
					result = "Bullet_Shell_HighExplosive";
					goto IL_0c6b;
				}
				if (defName == "Bullet_MortarIncendiary")
				{
					result = "Bullet_Shell_Incendiary";
					goto IL_0c6b;
				}
				if (defName == "Bullet_MortarEMP")
				{
					result = "Bullet_Shell_EMP";
					goto IL_0c6b;
				}
				if (defName == "MortarShell")
				{
					result = "Shell_HighExplosive";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(ConceptDef))
			{
				if (defName == "PersonalShields")
				{
					result = "ShieldBelts";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(PawnKindDef))
			{
				if (defName == "Megatherium")
				{
					result = "Megasloth";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(ThoughtDef))
			{
				if (defName == "ComfortLevel")
				{
					result = "NeedComfort";
					goto IL_0c6b;
				}
				if (defName == "JoyLevel")
				{
					result = "NeedJoy";
					goto IL_0c6b;
				}
				if (defName == "Tired")
				{
					result = "NeedRest";
					goto IL_0c6b;
				}
				if (defName == "Hungry")
				{
					result = "NeedFood";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(ResearchProjectDef))
			{
				if (defName == "MalariBlockProduction")
				{
					result = "PenoxycylineProduction";
					goto IL_0c6b;
				}
				if (defName == "IEDBomb")
				{
					result = "IEDs";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(MentalStateDef))
			{
				if (defName == "ConfusedWander")
				{
					result = "WanderConfused";
					goto IL_0c6b;
				}
				if (defName == "DazedWander")
				{
					result = "WanderPsychotic";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(RulePackDef))
			{
				if (defName == "NamerAnimalGeneric")
				{
					result = "NamerAnimalGenericMale";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(TraderKindDef))
			{
				if (defName == "Caravan_Neolithic_SlavesMerchant")
				{
					result = "Caravan_Neolithic_Slaver";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(DifficultyDef))
			{
				if (defName == "FreePlay")
				{
					result = "VeryEasy";
					goto IL_0c6b;
				}
				if (defName == "Basebuilder")
				{
					result = "Easy";
					goto IL_0c6b;
				}
				if (defName == "Rough")
				{
					result = "Medium";
					goto IL_0c6b;
				}
				if (defName == "Challenge")
				{
					result = "Hard";
					goto IL_0c6b;
				}
				if (defName == "Extreme")
				{
					result = "VeryHard";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(RecipeDef))
			{
				if (defName == "MakeArtilleryShell")
				{
					result = "Make_Shell_HighExplosive";
					goto IL_0c6b;
				}
				if (defName == "Make_MalariBlock")
				{
					result = "Make_Penoxycyline";
					goto IL_0c6b;
				}
				if (defName == "Make_Apparel_MilitaryHelmet")
				{
					result = "Make_Apparel_SimpleHelmet";
					goto IL_0c6b;
				}
				if (defName == "Make_Apparel_KevlarHelmet")
				{
					result = "Make_Apparel_AdvancedHelmet";
					goto IL_0c6b;
				}
				if (defName == "Make_Gun_SurvivalRifle")
				{
					result = "Make_Gun_BoltActionRifle";
					goto IL_0c6b;
				}
				if (defName == "Make_Gun_Pistol")
				{
					result = "Make_Gun_Revolver";
					goto IL_0c6b;
				}
				if (defName == "Make_TableShort")
				{
					result = "Make_Table2x2c";
					goto IL_0c6b;
				}
				if (defName == "Make_TableLong")
				{
					result = "Make_Table2x4c";
					goto IL_0c6b;
				}
				if (defName == "MakeMortarShell")
				{
					result = "Make_Shell_HighExplosive";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(HediffDef))
			{
				if (defName == "Euthanasia")
				{
					result = "ShutDown";
					goto IL_0c6b;
				}
				if (defName == "ChemicalDamageBrain")
				{
					result = "ChemicalDamageModerate";
					goto IL_0c6b;
				}
				if (defName == "ChemicalDamageKidney")
				{
					result = "ChemicalDamageSevere";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(TraderKindDef))
			{
				if (defName == "Caravan_Neolithic_CombatSupplier")
				{
					result = "Caravan_Neolithic_WarMerchant";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(StatDef))
			{
				if (defName == "HarvestYield")
				{
					result = "PlantHarvestYield";
					goto IL_0c6b;
				}
				if (defName == "SurgerySuccessChance")
				{
					result = "MedicalSurgerySuccessChance";
					goto IL_0c6b;
				}
				if (defName == "HealingQuality")
				{
					result = "MedicalTendQuality";
					goto IL_0c6b;
				}
				if (defName == "HealingSpeed")
				{
					result = "MedicalTendSpeed";
					goto IL_0c6b;
				}
				if (defName == "GiftImpact")
				{
					result = "DiplomacyPower";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(SkillDef))
			{
				if (defName == "Research")
				{
					result = "Intellectual";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(LetterDef))
			{
				if (defName == "BadUrgent")
				{
					result = "ThreatBig";
					goto IL_0c6b;
				}
				if (defName == "BadNonUrgent")
				{
					result = "NegativeEvent";
					goto IL_0c6b;
				}
				if (defName == "Good")
				{
					result = "PositiveEvent";
					goto IL_0c6b;
				}
			}
			else if (defType == typeof(WorldObjectDef) && defName == "JourneyDestination")
			{
				result = "EscapeShip";
				goto IL_0c6b;
			}
			result = defName;
			goto IL_0c6b;
			IL_0c6b:
			return result;
		}

		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			Type result;
			if (baseType == typeof(WorldObject))
			{
				if (providedClassName == "RimWorld.Planet.WorldObject" && node["def"] != null && node["def"].InnerText == "JourneyDestination")
				{
					result = WorldObjectDefOf.EscapeShip.worldObjectClass;
					goto IL_0123;
				}
			}
			else if (baseType == typeof(Thing))
			{
				if (providedClassName == "Building_PoisonShipPart" && node["def"] != null && node["def"].InnerText == "CrashedPoisonShipPart")
				{
					result = ThingDefOf.CrashedPoisonShipPart.thingClass;
					goto IL_0123;
				}
				if (providedClassName == "Building_PsychicEmanator" && node["def"] != null && node["def"].InnerText == "CrashedPsychicEmanatorShipPart")
				{
					result = ThingDefOf.CrashedPsychicEmanatorShipPart.thingClass;
					goto IL_0123;
				}
			}
			result = GenTypes.GetTypeInAnyAssembly(providedClassName);
			goto IL_0123;
			IL_0123:
			return result;
		}

		public static string BackCompatibleModifiedTranslationPath(Type defType, string path)
		{
			return (defType != typeof(ConceptDef) || !path.Contains("helpTexts.0")) ? path : path.Replace("helpTexts.0", "helpText");
		}

		public static void AfterLoadingSmallGameClassComponents(Game game)
		{
			if (game.dateNotifier == null)
			{
				game.dateNotifier = new DateNotifier();
			}
			if (game.components == null)
			{
				game.components = new List<GameComponent>();
			}
		}

		public static void FactionBasePostLoadInit(FactionBase factionBase)
		{
			if (factionBase.trader == null)
			{
				factionBase.trader = new FactionBase_TraderTracker(factionBase);
			}
		}

		public static void MapPostLoadInit(Map map)
		{
			if (map.storyState == null)
			{
				map.storyState = new StoryState(map);
			}
			if (map.pawnDestinationReservationManager == null)
			{
				map.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			}
		}

		public static void CaravanPostLoadInit(Caravan caravan)
		{
			if (caravan.storyState == null)
			{
				caravan.storyState = new StoryState(caravan);
			}
		}

		public static void SettlementPostLoadInit(Settlement settlement)
		{
			if (settlement.previouslyGeneratedInhabitants == null)
			{
				settlement.previouslyGeneratedInhabitants = new List<Pawn>();
			}
		}

		public static void JobTrackerPostLoadInit(Pawn_JobTracker jobTracker)
		{
			if (jobTracker.jobQueue != null)
			{
				bool flag = false;
				int num = 0;
				while (num < jobTracker.jobQueue.Count)
				{
					if (jobTracker.jobQueue[num].job != null)
					{
						num++;
						continue;
					}
					flag = true;
					break;
				}
				if (flag)
				{
					jobTracker.ClearQueuedJobs();
				}
			}
		}

		public static void RecordsTrackerPostLoadInit(Pawn_RecordsTracker recordTracker)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 17 && Find.TaleManager.AnyTaleConcerns(recordTracker.pawn))
			{
				recordTracker.AccumulateStoryEvent(StoryEventDefOf.TaleCreated);
			}
		}

		public static void WorldLoadingVars(World world)
		{
			if (world.components == null)
			{
				world.components = new List<WorldComponent>();
			}
			if (world.settings == null)
			{
				world.settings = new WorldSettings();
			}
		}

		public static void TurretPostLoadInit(Building_TurretGun turret)
		{
			if (turret.gun == null)
			{
				turret.MakeGun();
			}
		}

		public static void ImportantPawnCompPostLoadInit(ImportantPawnComp c)
		{
			if (c.pawn == null)
			{
				c.pawn = new ThingOwner<Pawn>(c, true, LookMode.Deep);
			}
		}

		public static void PawnPostLoadInit(Pawn p)
		{
			if (p.Spawned && p.rotationTracker == null)
			{
				p.rotationTracker = new Pawn_RotationTracker(p);
			}
		}

		public static void WorldPawnPostLoadInit(WorldPawns wp)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 17)
			{
				wp.UnpinAllForcefullyKeptPawns();
			}
			if (wp.gc == null)
			{
				wp.gc = new WorldPawnGC();
			}
		}

		public static void MindStatePostLoadInit(Pawn_MindState mindState)
		{
			if (mindState.inspirationHandler == null)
			{
				mindState.inspirationHandler = new InspirationHandler(mindState.pawn);
			}
		}

		public static void GameConditionPostLoadInit(GameCondition gameCondition)
		{
			if (!gameCondition.Permanent && gameCondition.Duration > 1000000000)
			{
				gameCondition.Permanent = true;
			}
		}

		public static void GameLoadingVars(Game game)
		{
			if (game.battleLog == null)
			{
				game.battleLog = new BattleLog();
			}
		}
	}
}
