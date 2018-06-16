using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000ECF RID: 3791
	public static class BackCompatibility
	{
		// Token: 0x060059A6 RID: 22950 RVA: 0x002DD4A0 File Offset: 0x002DB8A0
		public static bool IsSaveCompatibleWith(string version)
		{
			bool result;
			if (VersionControl.BuildFromVersionString(version) == VersionControl.CurrentBuild)
			{
				result = true;
			}
			else if (VersionControl.MajorFromVersionString(version) != 0 || VersionControl.CurrentMajor != 0)
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
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x002DD544 File Offset: 0x002DB944
		public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false)
		{
			string result;
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
			{
				result = defName;
			}
			else
			{
				if (defType == typeof(ThingDef))
				{
					if (defName == "Gun_SurvivalRifle")
					{
						return "Gun_BoltActionRifle";
					}
					if (defName == "Bullet_SurvivalRifle")
					{
						return "Bullet_BoltActionRifle";
					}
					if (defName == "Neurotrainer")
					{
						return "MechSerumNeurotrainer";
					}
					if (defName == "FueledGenerator")
					{
						return "WoodFiredGenerator";
					}
					if (defName == "Gun_Pistol")
					{
						return "Gun_Revolver";
					}
					if (defName == "Bullet_Pistol")
					{
						return "Bullet_Revolver";
					}
					if (defName == "TableShort")
					{
						return "Table2x2c";
					}
					if (defName == "TableLong")
					{
						return "Table2x4c";
					}
					if (defName == "TableShort_Blueprint")
					{
						return "Table2x2c_Blueprint";
					}
					if (defName == "TableLong_Blueprint")
					{
						return "Table2x4c_Blueprint";
					}
					if (defName == "TableShort_Frame")
					{
						return "Table2x2c_Frame";
					}
					if (defName == "TableLong_Frame")
					{
						return "Table2x4c_Frame";
					}
					if (defName == "TableShort_Install")
					{
						return "Table2x2c_Install";
					}
					if (defName == "TableLong_Install")
					{
						return "Table2x4c_Install";
					}
					if (defName == "Turret_MortarBomb")
					{
						return "Turret_Mortar";
					}
					if (defName == "Turret_Incendiary")
					{
						return "Turret_Mortar";
					}
					if (defName == "Turret_MortarIncendiary")
					{
						return "Turret_Mortar";
					}
					if (defName == "Turret_EMP")
					{
						return "Turret_Mortar";
					}
					if (defName == "Turret_MortarEMP")
					{
						return "Turret_Mortar";
					}
					if (defName == "Turret_MortarBomb_Blueprint")
					{
						return "Turret_Mortar_Blueprint";
					}
					if (defName == "Turret_Incendiary_Blueprint")
					{
						return "Turret_Mortar_Blueprint";
					}
					if (defName == "Turret_MortarIncendiary_Blueprint")
					{
						return "Turret_Mortar_Blueprint";
					}
					if (defName == "Turret_EMP_Blueprint")
					{
						return "Turret_Mortar_Blueprint";
					}
					if (defName == "Turret_MortarEMP_Blueprint")
					{
						return "Turret_Mortar_Blueprint";
					}
					if (defName == "Turret_MortarBomb_Frame")
					{
						return "Turret_Mortar_Frame";
					}
					if (defName == "Turret_Incendiary_Frame")
					{
						return "Turret_Mortar_Frame";
					}
					if (defName == "Turret_MortarIncendiary_Frame")
					{
						return "Turret_Mortar_Frame";
					}
					if (defName == "Turret_EMP_Frame")
					{
						return "Turret_Mortar_Frame";
					}
					if (defName == "Turret_MortarEMP_Frame")
					{
						return "Turret_Mortar_Frame";
					}
					if (defName == "Turret_MortarBomb_Install")
					{
						return "Turret_Mortar_Install";
					}
					if (defName == "Turret_Incendiary_Install")
					{
						return "Turret_Mortar_Install";
					}
					if (defName == "Turret_MortarIncendiary_Install")
					{
						return "Turret_Mortar_Install";
					}
					if (defName == "Turret_EMP_Install")
					{
						return "Turret_Mortar_Install";
					}
					if (defName == "Turret_MortarEMP_Install")
					{
						return "Turret_Mortar_Install";
					}
					if (defName == "Artillery_MortarBomb")
					{
						return "Artillery_Mortar";
					}
					if (defName == "Artillery_MortarIncendiary")
					{
						return "Artillery_Mortar";
					}
					if (defName == "Artillery_MortarEMP")
					{
						return "Artillery_Mortar";
					}
					if (defName == "TrapIEDBomb")
					{
						return "TrapIED_HighExplosive";
					}
					if (defName == "TrapIEDIncendiary")
					{
						return "TrapIED_Incendiary";
					}
					if (defName == "TrapIEDBomb_Blueprint")
					{
						return "TrapIED_HighExplosive_Blueprint";
					}
					if (defName == "TrapIEDIncendiary_Blueprint")
					{
						return "TrapIED_Incendiary_Blueprint";
					}
					if (defName == "TrapIEDBomb_Frame")
					{
						return "TrapIED_HighExplosive_Frame";
					}
					if (defName == "TrapIEDIncendiary_Frame")
					{
						return "TrapIED_Incendiary_Frame";
					}
					if (defName == "TrapIEDBomb_Install")
					{
						return "TrapIED_HighExplosive_Install";
					}
					if (defName == "TrapIEDIncendiary_Install")
					{
						return "TrapIED_Incendiary_Install";
					}
					if (defName == "Bullet_MortarBomb")
					{
						return "Bullet_Shell_HighExplosive";
					}
					if (defName == "Bullet_MortarIncendiary")
					{
						return "Bullet_Shell_Incendiary";
					}
					if (defName == "Bullet_MortarEMP")
					{
						return "Bullet_Shell_EMP";
					}
					if (defName == "MortarShell")
					{
						return "Shell_HighExplosive";
					}
					if (defName == "GrizzlyBear_Leather")
					{
						return "Leather_Bear";
					}
					if (defName == "PolarBear_Leather")
					{
						return "Leather_Bear";
					}
					if (defName == "Cassowary_Leather")
					{
						return "Leather_Bird";
					}
					if (defName == "Emu_Leather")
					{
						return "Leather_Bird";
					}
					if (defName == "Ostrich_Leather")
					{
						return "Leather_Bird";
					}
					if (defName == "Turkey_Leather")
					{
						return "Leather_Bird";
					}
					if (defName == "Muffalo_Leather")
					{
						return "Leather_Bluefur";
					}
					if (defName == "Dromedary_Leather")
					{
						return "Leather_Camel";
					}
					if (defName == "Alpaca_Leather")
					{
						return "Leather_Camel";
					}
					if (defName == "Chinchilla_Leather")
					{
						return "Leather_Chinchilla";
					}
					if (defName == "Boomalope_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "Cow_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "Gazelle_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "Ibex_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "Deer_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "Elk_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "Caribou_Leather")
					{
						return "Leather_Plain";
					}
					if (defName == "YorkshireTerrier_Leather")
					{
						return "Leather_Dog";
					}
					if (defName == "Husky_Leather")
					{
						return "Leather_Dog";
					}
					if (defName == "LabradorRetriever_Leather")
					{
						return "Leather_Dog";
					}
					if (defName == "Elephant_Leather")
					{
						return "Leather_Elephant";
					}
					if (defName == "FoxFennec_Leather")
					{
						return "Leather_Fox";
					}
					if (defName == "FoxRed_Leather")
					{
						return "Leather_Fox";
					}
					if (defName == "FoxArctic_Leather")
					{
						return "Leather_Fox";
					}
					if (defName == "Megasloth_Leather")
					{
						return "Leather_Heavy";
					}
					if (defName == "Human_Leather")
					{
						return "Leather_Human";
					}
					if (defName == "Boomrat_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Cat_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Hare_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Snowhare_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Squirrel_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Alphabeaver_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Capybara_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Raccoon_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Rat_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Monkey_Leather")
					{
						return "Leather_Light";
					}
					if (defName == "Iguana_Leather")
					{
						return "Leather_Lizard";
					}
					if (defName == "Tortoise_Leather")
					{
						return "Leather_Lizard";
					}
					if (defName == "Cobra_Leather")
					{
						return "Leather_Lizard";
					}
					if (defName == "Cougar_Leather")
					{
						return "Leather_Panthera";
					}
					if (defName == "Panther_Leather")
					{
						return "Leather_Panthera";
					}
					if (defName == "Lynx_Leather")
					{
						return "Leather_Panthera";
					}
					if (defName == "Pig_Leather")
					{
						return "Leather_Pig";
					}
					if (defName == "Rhinoceros_Leather")
					{
						return "Leather_Rhinoceros";
					}
					if (defName == "Thrumbo_Leather")
					{
						return "Leather_Thrumbo";
					}
					if (defName == "Warg_Leather")
					{
						return "Leather_Wolf";
					}
					if (defName == "WolfTimber_Leather")
					{
						return "Leather_Wolf";
					}
					if (defName == "WolfArctic_Leather")
					{
						return "Leather_Wolf";
					}
					if (defName == "ScytherBlade")
					{
						return "PowerClaw";
					}
					if (defName == "GrizzlyBear")
					{
						return "Bear_Grizzly";
					}
					if (defName == "PolarBear")
					{
						return "Bear_Polar";
					}
					if (defName == "WolfTimber")
					{
						return "Wolf_Timber";
					}
					if (defName == "WolfArctic")
					{
						return "Wolf_Arctic";
					}
					if (defName == "FoxFennec")
					{
						return "Fox_Fennec";
					}
					if (defName == "FoxRed")
					{
						return "Fox_Red";
					}
					if (defName == "FoxArctic")
					{
						return "Fox_Arctic";
					}
					if (defName == "GrizzlyBear_Meat")
					{
						return "Meat_Bear_Grizzly";
					}
					if (defName == "WolfTimber_Meat")
					{
						return "Meat_Wolf_Timber";
					}
					if (defName == "FoxFennec_Meat")
					{
						return "Meat_Fox_Fennec";
					}
					if (defName == "GrizzlyBear_Corpse")
					{
						return "Corpse_Bear_Grizzly";
					}
					if (defName == "PolarBear_Corpse")
					{
						return "Corpse_Bear_Polar";
					}
					if (defName == "WolfTimber_Corpse")
					{
						return "Corpse_Wolf_Timber";
					}
					if (defName == "WolfArctic_Corpse")
					{
						return "Corpse_Wolf_Arctic";
					}
					if (defName == "FoxFennec_Corpse")
					{
						return "Corpse_Fox_Fennec";
					}
					if (defName == "FoxRed_Corpse")
					{
						return "Corpse_Fox_Red";
					}
					if (defName == "FoxArctic_Corpse")
					{
						return "Corpse_Fox_Arctic";
					}
					if (defName == "TurretGun")
					{
						return "Turret_MiniTurret";
					}
					if (defName == "Gun_TurretImprovised")
					{
						return "Gun_MiniTurret";
					}
					if (defName == "Bullet_TurretImprovised")
					{
						return "Bullet_MiniTurret";
					}
					if (defName == "MinifiedFurniture")
					{
						return "MinifiedThing";
					}
					if (defName == "MinifiedSculpture")
					{
						return "MinifiedThing";
					}
					if (defName == "HerbalMedicine")
					{
						return "MedicineHerbal";
					}
					if (defName == "Medicine")
					{
						return "MedicineIndustrial";
					}
					if (defName == "GlitterworldMedicine")
					{
						return "MedicineUltratech";
					}
					if (defName == "Component")
					{
						return "ComponentIndustrial";
					}
					if (defName == "AdvancedComponent")
					{
						return "ComponentSpacer";
					}
					if (defName == "MineableComponents")
					{
						return "MineableComponentsIndustrial";
					}
					if (defName.EndsWith("_Meat"))
					{
						return BackCompatibility.MeatSuffixExtract.Replace(defName, "Meat_$1");
					}
					if (defName.EndsWith("_Corpse"))
					{
						return BackCompatibility.CorpseSuffixExtract.Replace(defName, "Corpse_$1");
					}
					if (defName.EndsWith("_Blueprint"))
					{
						return BackCompatibility.BlueprintSuffixExtract.Replace(defName, "Blueprint_$1");
					}
					if (defName.EndsWith("_Blueprint_Install"))
					{
						return BackCompatibility.BlueprintInstallSuffixExtract.Replace(defName, "Blueprint_Install_$1");
					}
					if (defName.EndsWith("_Frame"))
					{
						return BackCompatibility.FrameSuffixExtract.Replace(defName, "Frame_$1");
					}
					if (defName == "MeleeWeapon_Shiv")
					{
						return "MeleeWeapon_Knife";
					}
					if (defName == "Apparel_VestPlate")
					{
						return "Apparel_FlakVest";
					}
					if (defName == "Refinery")
					{
						return "BiofuelRefinery";
					}
					if (defName == "PlantRose")
					{
						return "Plant_Rose";
					}
					if (defName == "PlantDaylily")
					{
						return "Plant_Daylily";
					}
					if (defName == "PlantRice")
					{
						return "Plant_Rice";
					}
					if (defName == "PlantPotato")
					{
						return "Plant_Potato";
					}
					if (defName == "PlantCorn")
					{
						return "Plant_Corn";
					}
					if (defName == "PlantStrawberry")
					{
						return "Plant_Strawberry";
					}
					if (defName == "PlantHaygrass")
					{
						return "Plant_Haygrass";
					}
					if (defName == "PlantCotton")
					{
						return "Plant_Cotton";
					}
					if (defName == "PlantDevilstrand")
					{
						return "Plant_Devilstrand";
					}
					if (defName == "PlantHealroot")
					{
						return "Plant_Healroot";
					}
					if (defName == "PlantHops")
					{
						return "Plant_Hops";
					}
					if (defName == "PlantSmokeleaf")
					{
						return "Plant_Smokeleaf";
					}
					if (defName == "PlantPsychoid")
					{
						return "Plant_Psychoid";
					}
					if (defName == "PlantAmbrosia")
					{
						return "Plant_Ambrosia";
					}
					if (defName == "PlantAgave")
					{
						return "Plant_Agave";
					}
					if (defName == "PlantPincushionCactus")
					{
						return "Plant_PincushionCactus";
					}
					if (defName == "PlantSaguaroCactus")
					{
						return "Plant_SaguaroCactus";
					}
					if (defName == "PlantTreeDrago")
					{
						return "Plant_TreeDrago";
					}
					if (defName == "PlantGrass")
					{
						return "Plant_Grass";
					}
					if (defName == "PlantTallGrass")
					{
						return "Plant_TallGrass";
					}
					if (defName == "PlantBush")
					{
						return "Plant_Bush";
					}
					if (defName == "PlantBrambles")
					{
						return "Plant_Brambles";
					}
					if (defName == "PlantWildHealroot")
					{
						return "Plant_HealrootWild";
					}
					if (defName == "PlantTreeWillow")
					{
						return "Plant_TreeWillow";
					}
					if (defName == "PlantTreeCypress")
					{
						return "Plant_TreeCypress";
					}
					if (defName == "PlantTreeMaple")
					{
						return "Plant_TreeMaple";
					}
					if (defName == "PlantChokevine")
					{
						return "Plant_Chokevine";
					}
					if (defName == "PlantDandelion")
					{
						return "Plant_Dandelion";
					}
					if (defName == "PlantAstragalus")
					{
						return "Plant_Astragalus";
					}
					if (defName == "PlantMoss")
					{
						return "Plant_Moss";
					}
					if (defName == "PlantRaspberry")
					{
						return "Plant_Raspberry";
					}
					if (defName == "PlantTreeOak")
					{
						return "Plant_TreeOak";
					}
					if (defName == "PlantTreePoplar")
					{
						return "Plant_TreePoplar";
					}
					if (defName == "PlantTreePine")
					{
						return "Plant_TreePine";
					}
					if (defName == "PlantTreeBirch")
					{
						return "Plant_TreeBirch";
					}
					if (defName == "PlantShrubLow")
					{
						return "Plant_ShrubLow";
					}
					if (defName == "PlantAlocasia")
					{
						return "Plant_Alocasia";
					}
					if (defName == "PlantClivia")
					{
						return "Plant_Clivia";
					}
					if (defName == "PlantRafflesia")
					{
						return "Plant_Rafflesia";
					}
					if (defName == "PlantTreeTeak")
					{
						return "Plant_TreeTeak";
					}
					if (defName == "PlantTreeCecropia")
					{
						return "Plant_TreeCecropia";
					}
					if (defName == "PlantTreePalm")
					{
						return "Plant_TreePalm";
					}
					if (defName == "PlantTreeBamboo")
					{
						return "Plant_TreeBamboo";
					}
					if (defName == "FilthDirt")
					{
						return "Filth_Dirt";
					}
					if (defName == "FilthAnimalFilth")
					{
						return "Filth_AnimalFilth";
					}
					if (defName == "FilthHumanFilth")
					{
						return "Filth_HumanFilth";
					}
					if (defName == "FilthSand")
					{
						return "Filth_Sand";
					}
					if (defName == "FilthBlood")
					{
						return "Filth_Blood";
					}
					if (defName == "FilthBloodInsect")
					{
						return "Filth_BloodInsect";
					}
					if (defName == "FilthAmnioticFluid")
					{
						return "Filth_AmnioticFluid";
					}
					if (defName == "FilthSlime")
					{
						return "Filth_Slime";
					}
					if (defName == "FilthVomit")
					{
						return "Filth_Vomit";
					}
					if (defName == "FilthFireFoam")
					{
						return "Filth_FireFoam";
					}
					if (defName == "FilthFuel")
					{
						return "Filth_Fuel";
					}
					if (defName == "FilthCorpseBile")
					{
						return "Filth_CorpseBile";
					}
					if (defName == "FilthAsh")
					{
						return "Filth_Ash";
					}
					if (defName == "RockRubble")
					{
						return "Filth_RubbleRock";
					}
					if (defName == "BuildingRubble")
					{
						return "Filth_RubbleBuilding";
					}
				}
				else if (defType == typeof(HediffDef))
				{
					if (defName == "ScytherBlade")
					{
						return "PowerClaw";
					}
				}
				else if (defType == typeof(ResearchProjectDef))
				{
					if (defName == "IEDBomb")
					{
						return "IEDs";
					}
					if (defName == "Greatbows")
					{
						return "Greatbow";
					}
					if (defName == "Refining")
					{
						return "BiofuelRefining";
					}
					if (defName == "ComponentAssembly")
					{
						return "Fabrication";
					}
					if (defName == "AdvancedAssembly")
					{
						return "AdvancedFabrication";
					}
				}
				else if (defType == typeof(RecipeDef))
				{
					if (defName == "Make_Gun_SurvivalRifle")
					{
						return "Make_Gun_BoltActionRifle";
					}
					if (defName == "Make_Gun_Pistol")
					{
						return "Make_Gun_Revolver";
					}
					if (defName == "Make_TableShort")
					{
						return "Make_Table2x2c";
					}
					if (defName == "Make_TableLong")
					{
						return "Make_Table2x4c";
					}
					if (defName == "MakeMortarShell")
					{
						return "Make_Shell_HighExplosive";
					}
					if (defName == "MakeWort")
					{
						return "Make_Wort";
					}
					if (defName == "MakeKibble")
					{
						return "Make_Kibble";
					}
					if (defName == "MakePemmican")
					{
						return "Make_Pemmican";
					}
					if (defName == "MakePemmicanCampfire")
					{
						return "Make_PemmicanCampfire";
					}
					if (defName == "MakeStoneBlocksAny")
					{
						return "Make_StoneBlocksAny";
					}
					if (defName == "MakeChemfuelFromWood")
					{
						return "Make_ChemfuelFromWood";
					}
					if (defName == "MakeChemfuelFromOrganics")
					{
						return "Make_ChemfuelFromOrganics";
					}
					if (defName == "MakeComponent")
					{
						return "Make_ComponentIndustrial";
					}
					if (defName == "MakeAdvancedComponent")
					{
						return "Make_ComponentSpacer";
					}
					if (defName == "MakePatchleather")
					{
						return "Make_Patchleather";
					}
					if (defName == "MakeStoneBlocksSandstone")
					{
						return "Make_StoneBlocksSandstone";
					}
					if (defName == "MakeStoneBlocksGranite")
					{
						return "Make_StoneBlocksGranite";
					}
					if (defName == "MakeStoneBlocksLimestone")
					{
						return "Make_StoneBlocksLimestone";
					}
					if (defName == "MakeStoneBlocksSlate")
					{
						return "Make_StoneBlocksSlate";
					}
					if (defName == "MakeStoneBlocksMarble")
					{
						return "Make_StoneBlocksMarble";
					}
					if (defName == "Make_Component")
					{
						return "Make_ComponentIndustrial";
					}
					if (defName == "Make_AdvancedComponent")
					{
						return "Make_ComponentSpacer";
					}
				}
				else if (defType == typeof(StatDef))
				{
					if (defName == "GiftImpact")
					{
						return "DiplomacyPower";
					}
				}
				else if (defType == typeof(LetterDef))
				{
					if (defName == "BadUrgent")
					{
						return "ThreatBig";
					}
					if (defName == "BadNonUrgent")
					{
						return "NegativeEvent";
					}
					if (defName == "Good")
					{
						return "PositiveEvent";
					}
				}
				else if (defType == typeof(WorldObjectDef))
				{
					if (defName == "JourneyDestination")
					{
						return "EscapeShip";
					}
					if (defName == "AttackedCaravan")
					{
						return "AttackedNonPlayerCaravan";
					}
					if (defName == "AbandonedFactionBase")
					{
						return "AbandonedBase";
					}
				}
				else if (defType == typeof(HistoryAutoRecorderDef))
				{
					if (defName == "WealthTotal")
					{
						return "Wealth_Total";
					}
					if (defName == "WealthItems")
					{
						return "Wealth_Items";
					}
					if (defName == "WealthBuildings")
					{
						return "Wealth_Buildings";
					}
				}
				else if (defType == typeof(InspirationDef))
				{
					if (defName == "GoFrenzy")
					{
						return "Frenzy_Go";
					}
					if (defName == "InspiredArt")
					{
						return "Inspired_Creativity";
					}
					if (defName == "InspiredRecruitment")
					{
						return "Inspired_Recruitment";
					}
					if (defName == "InspiredSurgery")
					{
						return "Inspired_Surgery";
					}
					if (defName == "InspiredTrade")
					{
						return "Inspired_Trade";
					}
					if (defName == "ShootFrenzy")
					{
						return "Frenzy_Shoot";
					}
					if (defName == "WorkFrenzy")
					{
						return "Frenzy_Work";
					}
				}
				else if (defType == typeof(JobDef))
				{
					if (defName == "PlayBilliards")
					{
						return "Play_Billiards";
					}
					if (defName == "PlayChess")
					{
						return "Play_Chess";
					}
					if (defName == "PlayHoopstone")
					{
						return "Play_Hoopstone";
					}
					if (defName == "PlayHorseshoes")
					{
						return "Play_Horseshoes";
					}
					if (defName == "PlayPoker")
					{
						return "Play_Poker";
					}
					if (defName == "WaitCombat")
					{
						return "Wait_Combat";
					}
					if (defName == "WaitDowned")
					{
						return "Wait_Downed";
					}
					if (defName == "WaitMaintainPosture")
					{
						return "Wait_MaintainPosture";
					}
					if (defName == "WaitSafeTemperature")
					{
						return "Wait_SafeTemperature";
					}
					if (defName == "WaitWander")
					{
						return "Wait_Wander";
					}
				}
				else if (defType == typeof(JoyKindDef))
				{
					if (defName == "GamingDexterity")
					{
						return "Gaming_Dexterity";
					}
					if (defName == "GamingCerebral")
					{
						return "Gaming_Cerebral";
					}
				}
				else if (defType == typeof(KeyBindingDef))
				{
					if (defName == "MapDollyUp")
					{
						return "MapDolly_Up";
					}
					if (defName == "MapDollyDown")
					{
						return "MapDolly_Down";
					}
					if (defName == "MapDollyLeft")
					{
						return "MapDolly_Left";
					}
					if (defName == "MapDollyRight")
					{
						return "MapDolly_Right";
					}
					if (defName == "MapZoomIn")
					{
						return "MapZoom_In";
					}
					if (defName == "MapZoomOut")
					{
						return "MapZoom_Out";
					}
					if (defName == "TimeSpeedNormal")
					{
						return "TimeSpeed_Normal";
					}
					if (defName == "TimeSpeedFast")
					{
						return "TimeSpeed_Fast";
					}
					if (defName == "TimeSpeedSuperfast")
					{
						return "TimeSpeed_Superfast";
					}
					if (defName == "TimeSpeedUltrafast")
					{
						return "TimeSpeed_Ultrafast";
					}
					if (defName == "CommandTogglePower")
					{
						return "Command_TogglePower";
					}
					if (defName == "CommandItemForbid")
					{
						return "Command_ItemForbid";
					}
					if (defName == "CommandColonistDraft")
					{
						return "Command_ColonistDraft";
					}
					if (defName == "DesignatorCancel")
					{
						return "Designator_Cancel";
					}
					if (defName == "DesignatorDeconstruct")
					{
						return "Designator_Deconstruct";
					}
					if (defName == "DesignatorRotateLeft")
					{
						return "Designator_RotateLeft";
					}
					if (defName == "DesignatorRotateRight")
					{
						return "Designator_RotateRight";
					}
					if (defName == "ModifierIncrement10x")
					{
						return "ModifierIncrement_10x";
					}
					if (defName == "ModifierIncrement100x")
					{
						return "ModifierIncrement_100x";
					}
					if (defName == "TickOnce")
					{
						return "Dev_TickOnce";
					}
					if (defName == "ToggleGodMode")
					{
						return "Dev_ToggleGodMode";
					}
					if (defName == "ToggleDebugLog")
					{
						return "Dev_ToggleDebugLog";
					}
					if (defName == "ToggleDebugActionsMenu")
					{
						return "Dev_ToggleDebugActionsMenu";
					}
					if (defName == "ToggleDebugActionsMenu")
					{
						return "Dev_ToggleDebugActionsMenu";
					}
					if (defName == "ToggleDebugLogMenu")
					{
						return "Dev_ToggleDebugLogMenu";
					}
					if (defName == "ToggleDebugInspector")
					{
						return "Dev_ToggleDebugInspector";
					}
					if (defName == "ToggleDebugSettingsMenu")
					{
						return "Dev_ToggleDebugSettingsMenu";
					}
				}
				else if (defType == typeof(MentalBreakDef))
				{
					if (defName == "BingingDrugExtreme")
					{
						return "Binging_DrugExtreme";
					}
					if (defName == "BingingDrugMajor")
					{
						return "Binging_DrugMajor";
					}
					if (defName == "BingingFood")
					{
						return "Binging_Food";
					}
					if (defName == "WanderOwnRoom")
					{
						return "Wander_OwnRoom";
					}
					if (defName == "WanderPsychotic")
					{
						return "Wander_Psychotic";
					}
					if (defName == "WanderSad")
					{
						return "Wander_Sad";
					}
				}
				else if (defType == typeof(MentalStateDef))
				{
					if (defName == "BingingDrugExtreme")
					{
						return "Binging_DrugExtreme";
					}
					if (defName == "BingingDrugMajor")
					{
						return "Binging_DrugMajor";
					}
					if (defName == "BingingFood")
					{
						return "Binging_Food";
					}
					if (defName == "WanderOwnRoom")
					{
						return "Wander_OwnRoom";
					}
					if (defName == "WanderPsychotic")
					{
						return "Wander_Psychotic";
					}
					if (defName == "WanderSad")
					{
						return "Wander_Sad";
					}
				}
				else if (defType == typeof(MentalStateDef))
				{
					if (defName == "BingingDrugExtreme")
					{
						return "Binging_DrugExtreme";
					}
					if (defName == "BingingDrugMajor")
					{
						return "Binging_DrugMajor";
					}
					if (defName == "BingingFood")
					{
						return "Binging_Food";
					}
					if (defName == "WanderOwnRoom")
					{
						return "Wander_OwnRoom";
					}
					if (defName == "WanderPsychotic")
					{
						return "Wander_Psychotic";
					}
					if (defName == "WanderSad")
					{
						return "Wander_Sad";
					}
				}
				else if (defType == typeof(PawnKindDef))
				{
					if (defName == "GrenadierDestructive")
					{
						return "Grenadier_Destructive";
					}
					if (defName == "GrenadierEMP")
					{
						return "Grenadier_EMP";
					}
					if (defName == "MercenaryGunner")
					{
						return "Mercenary_Gunner";
					}
					if (defName == "MercenarySniper")
					{
						return "Mercenary_Sniper";
					}
					if (defName == "MercenarySlasher")
					{
						return "Mercenary_Slasher";
					}
					if (defName == "MercenaryHeavy")
					{
						return "Mercenary_Heavy";
					}
					if (defName == "MercenaryElite")
					{
						return "Mercenary_Elite";
					}
					if (defName == "TownCouncilman")
					{
						return "Town_Councilman";
					}
					if (defName == "TownTrader")
					{
						return "Town_Trader";
					}
					if (defName == "TownGuard")
					{
						return "Town_Guard";
					}
					if (defName == "TribalWarrior")
					{
						return "Tribal_Warrior";
					}
					if (defName == "TribalTrader")
					{
						return "Tribal_Trader";
					}
					if (defName == "TribalArcher")
					{
						return "Tribal_Archer";
					}
					if (defName == "TribalHunter")
					{
						return "Tribal_Hunter";
					}
					if (defName == "TribalBerserker")
					{
						return "Tribal_Berserker";
					}
					if (defName == "TribalChief")
					{
						return "Tribal_ChiefRanged";
					}
					if (defName == "GrizzlyBear")
					{
						return "Bear_Grizzly";
					}
					if (defName == "PolarBear")
					{
						return "Bear_Polar";
					}
					if (defName == "WolfTimber")
					{
						return "Wolf_Timber";
					}
					if (defName == "WolfArctic")
					{
						return "Wolf_Arctic";
					}
					if (defName == "FoxFennec")
					{
						return "Fox_Fennec";
					}
					if (defName == "FoxRed")
					{
						return "Fox_Red";
					}
					if (defName == "FoxArctic")
					{
						return "Fox_Arctic";
					}
					if (defName == "SpaceSoldier")
					{
						return "AncientSoldier";
					}
				}
				else if (defType == typeof(IncidentDef))
				{
					if (defName == "QuestBanditCamp")
					{
						return "Quest_BanditCamp";
					}
					if (defName == "QuestItemStash")
					{
						return "Quest_ItemStash";
					}
					if (defName == "QuestItemStashGuaranteedCore")
					{
						return "Quest_ItemStashAICore";
					}
					if (defName == "QuestDownedRefugee")
					{
						return "Quest_DownedRefugee";
					}
					if (defName == "QuestPrisonerWillingToJoin")
					{
						return "Quest_PrisonerRescue";
					}
					if (defName == "QuestPeaceTalks")
					{
						return "Quest_PeaceTalks";
					}
					if (defName == "JourneyOffer")
					{
						return "Quest_JourneyOffer";
					}
					if (defName == "CaravanRequest")
					{
						return "Quest_TradeRequest";
					}
				}
				else if (defType == typeof(DesignationDef))
				{
					if (defName == "SmoothFloor")
					{
						return "SmoothSurface";
					}
				}
				else if (defType == typeof(FactionDef))
				{
					if (defName == "Outlander")
					{
						return "OutlanderCivil";
					}
					if (defName == "Tribe")
					{
						return "TribeCivil";
					}
					if (defName == "Spacer")
					{
						return "Ancients";
					}
					if (defName == "SpacerHostile")
					{
						return "AncientsHostile";
					}
				}
				else if (defType == typeof(TerrainDef))
				{
					if (defName == "WaterMovingDeep")
					{
						return "WaterMovingChestDeep";
					}
				}
				else if (defType == typeof(RulePackDef))
				{
					if (defName == "NamerFactionBasePlayerColony")
					{
						return "NamerInitialSettlementColony";
					}
					if (defName == "NamerFactionBasePlayerTribe")
					{
						return "NamerInitialSettlementTribe";
					}
					if (defName == "NamerFactionBasePlayerTribe")
					{
						return "NamerSettlementPlayerTribe";
					}
					if (defName == "NamerFactionBasePlayerColonyRandomized")
					{
						return "NamerSettlementPlayerColonyRandomized";
					}
					if (defName == "NamerFactionBasePirate")
					{
						return "NamerSettlementPirate";
					}
					if (defName == "NamerFactionBaseOutlander")
					{
						return "NamerSettlementOutlander";
					}
					if (defName == "NamerFactionBaseTribal")
					{
						return "NamerSettlementTribal";
					}
				}
				else if (defType == typeof(TraitDef))
				{
					if (defName == "Prosthophile")
					{
						return "Transhumanist";
					}
					if (defName == "Prosthophobe")
					{
						return "BodyPurist";
					}
				}
				else if (defType == typeof(BodyPartDef))
				{
					if (!forDefInjections)
					{
						if (defName == "LeftAntenna")
						{
							return "Antenna";
						}
						if (defName == "RightAntenna")
						{
							return "Antenna";
						}
						if (defName == "LeftElytra")
						{
							return "Elytra";
						}
						if (defName == "RightElytra")
						{
							return "Elytra";
						}
						if (defName == "FrontLeftLeg")
						{
							return "Leg";
						}
						if (defName == "FrontRightLeg")
						{
							return "Leg";
						}
						if (defName == "MiddleLeftLeg")
						{
							return "Leg";
						}
						if (defName == "MiddleRightLeg")
						{
							return "Leg";
						}
						if (defName == "RearLeftLeg")
						{
							return "Leg";
						}
						if (defName == "RearRightLeg")
						{
							return "Leg";
						}
						if (defName == "FrontLeftInsectLeg")
						{
							return "InsectLeg";
						}
						if (defName == "FrontRightInsectLeg")
						{
							return "InsectLeg";
						}
						if (defName == "MiddleLeftInsectLeg")
						{
							return "InsectLeg";
						}
						if (defName == "MiddleRightInsectLeg")
						{
							return "InsectLeg";
						}
						if (defName == "RearLeftInsectLeg")
						{
							return "InsectLeg";
						}
						if (defName == "RearRightInsectLeg")
						{
							return "InsectLeg";
						}
						if (defName == "FrontLeftPaw")
						{
							return "Paw";
						}
						if (defName == "FrontRightPaw")
						{
							return "Paw";
						}
						if (defName == "RearLeftPaw")
						{
							return "Paw";
						}
						if (defName == "RearRightPaw")
						{
							return "Paw";
						}
						if (defName == "FrontLeftHoof")
						{
							return "Hoof";
						}
						if (defName == "FrontRightHoof")
						{
							return "Hoof";
						}
						if (defName == "RearLeftHoof")
						{
							return "Hoof";
						}
						if (defName == "RearRightHoof")
						{
							return "Hoof";
						}
						if (defName == "FrontLeftLegFirstClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontLeftLegSecondClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontLeftLegThirdClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontLeftLegFourthClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontLeftLegFifthClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontRightLegFirstClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontRightLegSecondClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontRightLegThirdClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontRightLegFourthClaw")
						{
							return "FrontClaw";
						}
						if (defName == "FrontRightLegFifthClaw")
						{
							return "FrontClaw";
						}
						if (defName == "RearLeftLegFirstClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearLeftLegSecondClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearLeftLegThirdClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearLeftLegFourthClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearLeftLegFifthClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearRightLegFirstClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearRightLegSecondClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearRightLegThirdClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearRightLegFourthClaw")
						{
							return "RearClaw";
						}
						if (defName == "RearRightLegFifthClaw")
						{
							return "RearClaw";
						}
						if (defName == "LeftEye")
						{
							return "Eye";
						}
						if (defName == "RightEye")
						{
							return "Eye";
						}
						if (defName == "LeftEar")
						{
							return "Ear";
						}
						if (defName == "RightEar")
						{
							return "Ear";
						}
						if (defName == "LeftLeg")
						{
							return "Leg";
						}
						if (defName == "RightLeg")
						{
							return "Leg";
						}
						if (defName == "LeftFoot")
						{
							return "Foot";
						}
						if (defName == "RightFoot")
						{
							return "Foot";
						}
						if (defName == "LeftShoulder")
						{
							return "Shoulder";
						}
						if (defName == "RightShoulder")
						{
							return "Shoulder";
						}
						if (defName == "LeftArm")
						{
							return "Arm";
						}
						if (defName == "RightArm")
						{
							return "Arm";
						}
						if (defName == "LeftHand")
						{
							return "Hand";
						}
						if (defName == "RightHand")
						{
							return "Hand";
						}
						if (defName == "LeftHandPinky")
						{
							return "Finger";
						}
						if (defName == "LeftHandRingFinger")
						{
							return "Finger";
						}
						if (defName == "LeftHandMiddleFinger")
						{
							return "Finger";
						}
						if (defName == "LeftHandIndexFinger")
						{
							return "Finger";
						}
						if (defName == "LeftHandThumb")
						{
							return "Finger";
						}
						if (defName == "RightHandPinky")
						{
							return "Finger";
						}
						if (defName == "RightHandRingFinger")
						{
							return "Finger";
						}
						if (defName == "RightHandMiddleFinger")
						{
							return "Finger";
						}
						if (defName == "RightHandIndexFinger")
						{
							return "Finger";
						}
						if (defName == "RightHandThumb")
						{
							return "Finger";
						}
						if (defName == "LeftFootLittleToe")
						{
							return "Toe";
						}
						if (defName == "LeftFootFourthToe")
						{
							return "Toe";
						}
						if (defName == "LeftFootMiddleToe")
						{
							return "Toe";
						}
						if (defName == "LeftFootSecondToe")
						{
							return "Toe";
						}
						if (defName == "LeftFootBigToe")
						{
							return "Toe";
						}
						if (defName == "RightFootLittleToe")
						{
							return "Toe";
						}
						if (defName == "RightFootFourthToe")
						{
							return "Toe";
						}
						if (defName == "RightFootMiddleToe")
						{
							return "Toe";
						}
						if (defName == "RightFootSecondToe")
						{
							return "Toe";
						}
						if (defName == "RightFootBigToe")
						{
							return "Toe";
						}
						if (defName == "LeftClavicle")
						{
							return "Clavicle";
						}
						if (defName == "RightClavicle")
						{
							return "Clavicle";
						}
						if (defName == "LeftHumerus")
						{
							return "Humerus";
						}
						if (defName == "RightHumerus")
						{
							return "Humerus";
						}
						if (defName == "LeftRadius")
						{
							return "Radius";
						}
						if (defName == "RightRadius")
						{
							return "Radius";
						}
						if (defName == "LeftFemur")
						{
							return "Femur";
						}
						if (defName == "RightFemur")
						{
							return "Femur";
						}
						if (defName == "LeftTibia")
						{
							return "Tibia";
						}
						if (defName == "RightTibia")
						{
							return "Tibia";
						}
						if (defName == "LeftSightSensor")
						{
							return "SightSensor";
						}
						if (defName == "RightSightSensor")
						{
							return "SightSensor";
						}
						if (defName == "LeftHearingSensor")
						{
							return "HearingSensor";
						}
						if (defName == "RightHearingSensor")
						{
							return "HearingSensor";
						}
						if (defName == "LeftMechanicalShoulder")
						{
							return "MechanicalShoulder";
						}
						if (defName == "RightMechanicalShoulder")
						{
							return "MechanicalShoulder";
						}
						if (defName == "LeftMechanicalArm")
						{
							return "MechanicalArm";
						}
						if (defName == "RightMechanicalArm")
						{
							return "MechanicalArm";
						}
						if (defName == "LeftMechanicalHand")
						{
							return "MechanicalHand";
						}
						if (defName == "RightMechanicalHand")
						{
							return "MechanicalHand";
						}
						if (defName == "LeftHandMechanicalPinky")
						{
							return "MechanicalFinger";
						}
						if (defName == "LeftHandMechanicalMiddleFinger")
						{
							return "MechanicalFinger";
						}
						if (defName == "LeftHandMechanicalIndexFinger")
						{
							return "MechanicalFinger";
						}
						if (defName == "LeftHandMechanicalThumb")
						{
							return "MechanicalFinger";
						}
						if (defName == "RightHandMechanicalPinky")
						{
							return "MechanicalFinger";
						}
						if (defName == "RightHandMechanicalMiddleFinger")
						{
							return "MechanicalFinger";
						}
						if (defName == "RightHandMechanicalIndexFinger")
						{
							return "MechanicalFinger";
						}
						if (defName == "RightHandMechanicalThumb")
						{
							return "MechanicalFinger";
						}
						if (defName == "LeftMechanicalLeg")
						{
							return "MechanicalLeg";
						}
						if (defName == "RightMechanicalLeg")
						{
							return "MechanicalLeg";
						}
						if (defName == "LeftMechanicalFoot")
						{
							return "MechanicalFoot";
						}
						if (defName == "RightMechanicalFoot")
						{
							return "MechanicalFoot";
						}
						if (defName == "LeftBlade")
						{
							return "Blade";
						}
						if (defName == "RightBlade")
						{
							return "Blade";
						}
						if (defName == "LeftLung")
						{
							return "Lung";
						}
						if (defName == "RightLung")
						{
							return "Lung";
						}
						if (defName == "LeftKidney")
						{
							return "Kidney";
						}
						if (defName == "RightKidney")
						{
							return "Kidney";
						}
						if (defName == "LeftTusk")
						{
							return "Tusk";
						}
						if (defName == "RightTusk")
						{
							return "Tusk";
						}
					}
				}
				result = defName;
			}
			return result;
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x002E0824 File Offset: 0x002DEC24
		public static object BackCompatibleEnum(Type enumType, string enumName)
		{
			if (enumType == typeof(QualityCategory))
			{
				if (enumName == "Shoddy")
				{
					return QualityCategory.Poor;
				}
				if (enumName == "Superior")
				{
					return QualityCategory.Excellent;
				}
			}
			return null;
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x002E0884 File Offset: 0x002DEC84
		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (baseType == typeof(WorldObject))
			{
				if (providedClassName == "RimWorld.Planet.WorldObject" && node["def"] != null && node["def"].InnerText == "JourneyDestination")
				{
					return WorldObjectDefOf.EscapeShip.worldObjectClass;
				}
			}
			else if (baseType == typeof(Thing))
			{
				if (providedClassName == "Building_PoisonShipPart" && node["def"] != null && node["def"].InnerText == "CrashedPoisonShipPart")
				{
					return ThingDefOf.CrashedPoisonShipPart.thingClass;
				}
				if (providedClassName == "Building_PsychicEmanator" && node["def"] != null && node["def"].InnerText == "CrashedPsychicEmanatorShipPart")
				{
					return ThingDefOf.CrashedPsychicEmanatorShipPart.thingClass;
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName);
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x002E09B8 File Offset: 0x002DEDB8
		public static string BackCompatibleModifiedTranslationPath(Type defType, string path)
		{
			if (defType == typeof(ConceptDef))
			{
				if (path.Contains("helpTexts.0"))
				{
					return path.Replace("helpTexts.0", "helpText");
				}
			}
			return path;
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x002E0A08 File Offset: 0x002DEE08
		public static void MapPostLoadInit(Map map)
		{
			if (map.pawnDestinationReservationManager == null)
			{
				map.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			}
			if (map.retainedCaravanData == null)
			{
				map.retainedCaravanData = new RetainedCaravanData(map);
			}
			if (map.wildAnimalSpawner == null)
			{
				map.wildAnimalSpawner = new WildAnimalSpawner(map);
			}
			if (map.wildPlantSpawner == null)
			{
				map.wildPlantSpawner = new WildPlantSpawner(map);
			}
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x002E0A71 File Offset: 0x002DEE71
		public static void CaravanPostLoadInit(Caravan caravan)
		{
			if (caravan.forage == null)
			{
				caravan.forage = new Caravan_ForageTracker(caravan);
			}
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x002E0A8C File Offset: 0x002DEE8C
		public static void RecordsTrackerPostLoadInit(Pawn_RecordsTracker recordTracker)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 17)
			{
				if (Find.TaleManager.AnyTaleConcerns(recordTracker.pawn))
				{
					recordTracker.AccumulateStoryEvent(StoryEventDefOf.TaleCreated);
				}
			}
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x002E0ADC File Offset: 0x002DEEDC
		public static void TurretPostLoadInit(Building_TurretGun turret)
		{
			if (turret.gun == null)
			{
				turret.MakeGun();
			}
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x002E0AF0 File Offset: 0x002DEEF0
		public static void ImportantPawnCompPostLoadInit(ImportantPawnComp c)
		{
			if (c.pawn == null)
			{
				c.pawn = new ThingOwner<Pawn>(c, true, LookMode.Deep);
			}
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x002E0B0C File Offset: 0x002DEF0C
		public static void PawnPostLoadInit(Pawn p)
		{
			if (p.Spawned && p.rotationTracker == null)
			{
				p.rotationTracker = new Pawn_RotationTracker(p);
			}
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x002E0B34 File Offset: 0x002DEF34
		public static void PawnTrainingTrackerPostLoadInit(Pawn_TrainingTracker tracker, ref DefMap<TrainableDef, bool> wantedTrainables, ref DefMap<TrainableDef, int> steps, ref DefMap<TrainableDef, bool> learned)
		{
			if (wantedTrainables == null)
			{
				wantedTrainables = new DefMap<TrainableDef, bool>();
			}
			if (steps == null)
			{
				steps = new DefMap<TrainableDef, int>();
			}
			if (learned == null)
			{
				learned = new DefMap<TrainableDef, bool>();
			}
			if (tracker.GetSteps(TrainableDefOf.Tameness) == 0 && DefDatabase<TrainableDef>.AllDefsListForReading.Any((TrainableDef td) => tracker.GetSteps(td) != 0))
			{
				tracker.Train(TrainableDefOf.Tameness, null, true);
			}
			foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefsListForReading)
			{
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x002E0C2C File Offset: 0x002DF02C
		public static void WorldPawnPostLoadInit(WorldPawns wp, ref HashSet<Pawn> pawnsMothballed)
		{
			if (wp.gc == null)
			{
				wp.gc = new WorldPawnGC();
			}
			if (pawnsMothballed == null)
			{
				pawnsMothballed = new HashSet<Pawn>();
			}
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x002E0C53 File Offset: 0x002DF053
		public static void MindStatePostLoadInit(Pawn_MindState mindState)
		{
			if (mindState.inspirationHandler == null)
			{
				mindState.inspirationHandler = new InspirationHandler(mindState.pawn);
			}
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x002E0C72 File Offset: 0x002DF072
		public static void GameConditionPostLoadInit(GameCondition gameCondition)
		{
			if (!gameCondition.Permanent && gameCondition.Duration > 1000000000)
			{
				gameCondition.Permanent = true;
			}
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x002E0C97 File Offset: 0x002DF097
		public static void GameLoadingVars(Game game)
		{
			if (game.battleLog == null)
			{
				game.battleLog = new BattleLog();
			}
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x002E0CB0 File Offset: 0x002DF0B0
		public static void HistoryLoadingVars(History history)
		{
			if (history.archive == null)
			{
				history.archive = new Archive();
			}
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x002E0CCC File Offset: 0x002DF0CC
		public static void WorldLoadingVars()
		{
			UniqueIDsManager uniqueIDsManager = null;
			Scribe_Deep.Look<UniqueIDsManager>(ref uniqueIDsManager, "uniqueIDsManager", new object[0]);
			if (uniqueIDsManager != null)
			{
				Current.Game.uniqueIDsManager = uniqueIDsManager;
			}
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x002E0D00 File Offset: 0x002DF100
		public static void PlaySettingsLoadingVars(PlaySettings playSettings)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 18)
			{
				playSettings.defaultCareForColonyHumanlike = MedicalCareCategory.Best;
				playSettings.defaultCareForColonyAnimal = MedicalCareCategory.HerbalOrWorse;
				playSettings.defaultCareForColonyPrisoner = MedicalCareCategory.HerbalOrWorse;
				playSettings.defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;
				playSettings.defaultCareForNeutralAnimal = MedicalCareCategory.HerbalOrWorse;
				playSettings.defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
			}
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x002E0D5C File Offset: 0x002DF15C
		public static void BattleLogEntry_MeleeCombat_LoadingVars(BattleLogEntry_MeleeCombat log)
		{
			if (log.RuleDef == null)
			{
				RulePackDef rulePackDef = null;
				RulePackDef rulePackDef2 = null;
				Scribe_Defs.Look<RulePackDef>(ref rulePackDef, "outcomeRuleDef");
				Scribe_Defs.Look<RulePackDef>(ref rulePackDef2, "maneuverRuleDef");
				if (rulePackDef != null && rulePackDef2 != null)
				{
					foreach (RulePackDef rulePackDef3 in DefDatabase<RulePackDef>.AllDefsListForReading)
					{
						if (!rulePackDef3.include.NullOrEmpty<RulePackDef>() && rulePackDef3.include.Count == 2)
						{
							if ((rulePackDef3.include[0] == rulePackDef && rulePackDef3.include[1] == rulePackDef2) || (rulePackDef3.include[1] == rulePackDef && rulePackDef3.include[0] == rulePackDef2))
							{
								log.RuleDef = rulePackDef3;
								break;
							}
						}
					}
				}
			}
			if (log.def == null)
			{
				log.def = LogEntryDefOf.MeleeAttack;
			}
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x002E0E7C File Offset: 0x002DF27C
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x002E0EA5 File Offset: 0x002DF2A5
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x002E0EB3 File Offset: 0x002DF2B3
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x002E0EC4 File Offset: 0x002DF2C4
		public static void HediffSetResolvingCrossRefs(Hediff hediff, List<Hediff> hediffs)
		{
			try
			{
				BodyPartRecord part = hediff.Part;
			}
			catch (Exception)
			{
				hediffs.Remove(hediff);
			}
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x002E0F00 File Offset: 0x002DF300
		public static void HediffLoadingVars(Hediff hediff)
		{
			Scribe_Values.Look<int>(ref hediff.temp_partIndexToSetLater, "partIndex", -1, false);
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x002E0F15 File Offset: 0x002DF315
		public static void HediffResolvingCrossRefs(Hediff hediff)
		{
			if (hediff.temp_partIndexToSetLater >= 0)
			{
				hediff.Part = hediff.pawn.RaceProps.body.GetPartAtIndex(hediff.temp_partIndexToSetLater);
				hediff.temp_partIndexToSetLater = -1;
			}
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x002E0F4E File Offset: 0x002DF34E
		public static void BillMedicalLoadingVars(Bill_Medical bill)
		{
			Scribe_Values.Look<int>(ref bill.temp_partIndexToSetLater, "partIndex", -1, false);
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x002E0F64 File Offset: 0x002DF364
		public static void BillMedicalResolvingCrossRefs(Bill_Medical bill)
		{
			if (bill.temp_partIndexToSetLater >= 0 && bill.GiverPawn != null)
			{
				bill.Part = bill.GiverPawn.RaceProps.body.GetPartAtIndex(bill.temp_partIndexToSetLater);
				bill.temp_partIndexToSetLater = -1;
			}
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x002E0FB3 File Offset: 0x002DF3B3
		public static void TradeRequestCompPostLoadInit(TradeRequestComp comp)
		{
			if (!comp.rewards.Any)
			{
				comp.rewards = new ThingOwner<Thing>(comp);
			}
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x002E0FD4 File Offset: 0x002DF3D4
		public static void FactionRelationLoadingVars(FactionRelation factionRelation)
		{
			bool flag = false;
			Scribe_Values.Look<bool>(ref flag, "hostile", false, false);
			if (flag || factionRelation.goodwill <= -75)
			{
				factionRelation.kind = FactionRelationKind.Hostile;
			}
			else if (factionRelation.goodwill >= 75)
			{
				factionRelation.kind = FactionRelationKind.Ally;
			}
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x002E1028 File Offset: 0x002DF428
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			TerrainDef result;
			if (hash == 16442)
			{
				result = TerrainDefOf.WaterMovingChestDeep;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x002E1054 File Offset: 0x002DF454
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			ThingDef result;
			if (hash == 62520)
			{
				result = ThingDefOf.MineableComponentsIndustrial;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x002E1080 File Offset: 0x002DF480
		public static void WorldInfoPostLoadInit(WorldInfo info)
		{
			if (info.randomValue == 0)
			{
				info.randomValue = Rand.Int;
			}
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x002E109C File Offset: 0x002DF49C
		public static void HediffComp_GetsPermanentLoadingVars(HediffComp_GetsPermanent hediffComp)
		{
			bool flag = false;
			Scribe_Values.Look<bool>(ref flag, "isOld", false, false);
			if (flag)
			{
				hediffComp.isPermanentInt = true;
			}
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x002E10C8 File Offset: 0x002DF4C8
		public static void WorldFeatureLoadingVars(WorldFeature feature)
		{
			if (feature.maxDrawSizeInTiles == 0f)
			{
				Vector2 zero = Vector2.zero;
				Scribe_Values.Look<Vector2>(ref zero, "maxDrawSizeInTiles", default(Vector2), false);
				feature.maxDrawSizeInTiles = zero.x;
			}
		}

		// Token: 0x04003BF1 RID: 15345
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x04003BF2 RID: 15346
		private static readonly Regex MeatSuffixExtract = new Regex("^(.+)_Meat$");

		// Token: 0x04003BF3 RID: 15347
		private static readonly Regex CorpseSuffixExtract = new Regex("^(.+)_Corpse$");

		// Token: 0x04003BF4 RID: 15348
		private static readonly Regex BlueprintSuffixExtract = new Regex("^(.+)_Blueprint$");

		// Token: 0x04003BF5 RID: 15349
		private static readonly Regex BlueprintInstallSuffixExtract = new Regex("^(.+)_Blueprint_Install$");

		// Token: 0x04003BF6 RID: 15350
		private static readonly Regex FrameSuffixExtract = new Regex("^(.+)_Frame$");
	}
}
