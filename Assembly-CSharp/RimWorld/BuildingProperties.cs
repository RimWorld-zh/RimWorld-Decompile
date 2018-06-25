using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023D RID: 573
	public class BuildingProperties
	{
		// Token: 0x04000406 RID: 1030
		public bool isEdifice = true;

		// Token: 0x04000407 RID: 1031
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		// Token: 0x04000408 RID: 1032
		public bool isInert = false;

		// Token: 0x04000409 RID: 1033
		private bool deconstructible = true;

		// Token: 0x0400040A RID: 1034
		public bool alwaysDeconstructible = false;

		// Token: 0x0400040B RID: 1035
		public bool claimable = true;

		// Token: 0x0400040C RID: 1036
		public bool isSittable = false;

		// Token: 0x0400040D RID: 1037
		public SoundDef soundAmbient;

		// Token: 0x0400040E RID: 1038
		public ConceptDef spawnedConceptLearnOpportunity = null;

		// Token: 0x0400040F RID: 1039
		public ConceptDef boughtConceptLearnOpportunity = null;

		// Token: 0x04000410 RID: 1040
		public bool expandHomeArea = true;

		// Token: 0x04000411 RID: 1041
		public Type blueprintClass = typeof(Blueprint_Build);

		// Token: 0x04000412 RID: 1042
		public GraphicData blueprintGraphicData;

		// Token: 0x04000413 RID: 1043
		public bool wantsHopperAdjacent = false;

		// Token: 0x04000414 RID: 1044
		public bool allowWireConnection = true;

		// Token: 0x04000415 RID: 1045
		public bool shipPart = false;

		// Token: 0x04000416 RID: 1046
		public bool canPlaceOverImpassablePlant = true;

		// Token: 0x04000417 RID: 1047
		public float heatPerTickWhileWorking = 0f;

		// Token: 0x04000418 RID: 1048
		public bool canBuildNonEdificesUnder = true;

		// Token: 0x04000419 RID: 1049
		public bool canPlaceOverWall = false;

		// Token: 0x0400041A RID: 1050
		public bool allowAutoroof = true;

		// Token: 0x0400041B RID: 1051
		public bool preventDeteriorationOnTop = false;

		// Token: 0x0400041C RID: 1052
		public bool preventDeteriorationInside = false;

		// Token: 0x0400041D RID: 1053
		public bool isMealSource = false;

		// Token: 0x0400041E RID: 1054
		public bool isNaturalRock = false;

		// Token: 0x0400041F RID: 1055
		public bool isResourceRock = false;

		// Token: 0x04000420 RID: 1056
		public bool repairable = true;

		// Token: 0x04000421 RID: 1057
		public float roofCollapseDamageMultiplier = 1f;

		// Token: 0x04000422 RID: 1058
		public bool hasFuelingPort;

		// Token: 0x04000423 RID: 1059
		public ThingDef smoothedThing = null;

		// Token: 0x04000424 RID: 1060
		[Unsaved]
		public ThingDef unsmoothedThing;

		// Token: 0x04000425 RID: 1061
		public TerrainDef naturalTerrain;

		// Token: 0x04000426 RID: 1062
		public TerrainDef leaveTerrain;

		// Token: 0x04000427 RID: 1063
		public bool isPlayerEjectable = false;

		// Token: 0x04000428 RID: 1064
		public GraphicData fullGraveGraphicData = null;

		// Token: 0x04000429 RID: 1065
		public float bed_healPerDay = 0f;

		// Token: 0x0400042A RID: 1066
		public bool bed_defaultMedical = false;

		// Token: 0x0400042B RID: 1067
		public bool bed_showSleeperBody = false;

		// Token: 0x0400042C RID: 1068
		public bool bed_humanlike = true;

		// Token: 0x0400042D RID: 1069
		public float bed_maxBodySize = 9999f;

		// Token: 0x0400042E RID: 1070
		public float nutritionCostPerDispense;

		// Token: 0x0400042F RID: 1071
		public SoundDef soundDispense;

		// Token: 0x04000430 RID: 1072
		public ThingDef turretGunDef;

		// Token: 0x04000431 RID: 1073
		public float turretBurstWarmupTime = 0f;

		// Token: 0x04000432 RID: 1074
		public float turretBurstCooldownTime = -1f;

		// Token: 0x04000433 RID: 1075
		[NoTranslate]
		public string turretTopGraphicPath = null;

		// Token: 0x04000434 RID: 1076
		[Unsaved]
		public Material turretTopMat;

		// Token: 0x04000435 RID: 1077
		public float turretTopDrawSize = 2f;

		// Token: 0x04000436 RID: 1078
		public Vector2 turretTopOffset;

		// Token: 0x04000437 RID: 1079
		public bool ai_combatDangerous = false;

		// Token: 0x04000438 RID: 1080
		public bool ai_chillDestination = true;

		// Token: 0x04000439 RID: 1081
		public SoundDef soundDoorOpenPowered;

		// Token: 0x0400043A RID: 1082
		public SoundDef soundDoorClosePowered;

		// Token: 0x0400043B RID: 1083
		public SoundDef soundDoorOpenManual;

		// Token: 0x0400043C RID: 1084
		public SoundDef soundDoorCloseManual;

		// Token: 0x0400043D RID: 1085
		[NoTranslate]
		public string sowTag = null;

		// Token: 0x0400043E RID: 1086
		public ThingDef defaultPlantToGrow = null;

		// Token: 0x0400043F RID: 1087
		public ThingDef mineableThing = null;

		// Token: 0x04000440 RID: 1088
		public int mineableYield = 1;

		// Token: 0x04000441 RID: 1089
		public float mineableNonMinedEfficiency = 0.7f;

		// Token: 0x04000442 RID: 1090
		public float mineableDropChance = 1f;

		// Token: 0x04000443 RID: 1091
		public bool mineableYieldWasteable = true;

		// Token: 0x04000444 RID: 1092
		public float mineableScatterCommonality = 0f;

		// Token: 0x04000445 RID: 1093
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		// Token: 0x04000446 RID: 1094
		public StorageSettings fixedStorageSettings = null;

		// Token: 0x04000447 RID: 1095
		public StorageSettings defaultStorageSettings = null;

		// Token: 0x04000448 RID: 1096
		public bool ignoreStoredThingsBeauty;

		// Token: 0x04000449 RID: 1097
		public bool isTrap = false;

		// Token: 0x0400044A RID: 1098
		public DamageArmorCategoryDef trapDamageCategory;

		// Token: 0x0400044B RID: 1099
		public GraphicData trapUnarmedGraphicData;

		// Token: 0x0400044C RID: 1100
		[Unsaved]
		public Graphic trapUnarmedGraphic;

		// Token: 0x0400044D RID: 1101
		public float unpoweredWorkTableWorkSpeedFactor = 0f;

		// Token: 0x0400044E RID: 1102
		public bool workSpeedPenaltyOutdoors = false;

		// Token: 0x0400044F RID: 1103
		public bool workSpeedPenaltyTemperature = false;

		// Token: 0x04000450 RID: 1104
		public IntRange watchBuildingStandDistanceRange = IntRange.one;

		// Token: 0x04000451 RID: 1105
		public int watchBuildingStandRectWidth = 3;

		// Token: 0x04000452 RID: 1106
		public JoyKindDef joyKind;

		// Token: 0x04000453 RID: 1107
		public int haulToContainerDuration;

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0005DEE4 File Offset: 0x0005C2E4
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0005DF08 File Offset: 0x0005C308
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0005DF2C File Offset: 0x0005C32C
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0005DF64 File Offset: 0x0005C364
		public bool IsMortar
		{
			get
			{
				bool result;
				if (!this.IsTurret)
				{
					result = false;
				}
				else
				{
					List<VerbProperties> verbs = this.turretGunDef.Verbs;
					for (int i = 0; i < verbs.Count; i++)
					{
						if (verbs[i].isPrimary && verbs[i].defaultProjectile != null && verbs[i].defaultProjectile.projectile.flyOverhead)
						{
							return true;
						}
					}
					if (this.turretGunDef.HasComp(typeof(CompChangeableProjectile)))
					{
						if (this.turretGunDef.building.fixedStorageSettings.filter.Allows(ThingDefOf.Shell_HighExplosive))
						{
							return true;
						}
						foreach (ThingDef thingDef in this.turretGunDef.building.fixedStorageSettings.filter.AllowedThingDefs)
						{
							if (thingDef.projectileWhenLoaded != null && thingDef.projectileWhenLoaded.projectile.flyOverhead)
							{
								return true;
							}
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0005E0C4 File Offset: 0x0005C4C4
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.isTrap && !this.isEdifice)
			{
				yield return "isTrap but is not edifice. Code will break.";
			}
			if (this.alwaysDeconstructible && !this.deconstructible)
			{
				yield return "alwaysDeconstructible=true but deconstructible=false";
			}
			if (parent.holdsRoof && !this.isEdifice)
			{
				yield return "holds roof but is not an edifice.";
			}
			yield break;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0005E0F5 File Offset: 0x0005C4F5
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0005E0F8 File Offset: 0x0005C4F8
		public void ResolveReferencesSpecial()
		{
			if (this.soundDoorOpenPowered == null)
			{
				this.soundDoorOpenPowered = SoundDefOf.Door_OpenPowered;
			}
			if (this.soundDoorClosePowered == null)
			{
				this.soundDoorClosePowered = SoundDefOf.Door_ClosePowered;
			}
			if (this.soundDoorOpenManual == null)
			{
				this.soundDoorOpenManual = SoundDefOf.Door_OpenManual;
			}
			if (this.soundDoorCloseManual == null)
			{
				this.soundDoorCloseManual = SoundDefOf.Door_CloseManual;
			}
			if (!this.turretTopGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopMat = MaterialPool.MatFrom(this.turretTopGraphicPath);
				});
			}
			if (this.fixedStorageSettings != null)
			{
				this.fixedStorageSettings.filter.ResolveReferences();
			}
			if (this.defaultStorageSettings == null)
			{
				if (this.fixedStorageSettings != null)
				{
					this.defaultStorageSettings = new StorageSettings();
					this.defaultStorageSettings.CopyFrom(this.fixedStorageSettings);
				}
			}
			if (this.defaultStorageSettings != null)
			{
				this.defaultStorageSettings.filter.ResolveReferences();
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0005E1EC File Offset: 0x0005C5EC
		public static void FinalizeInit()
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingDef.building != null)
				{
					if (thingDef.building.smoothedThing != null)
					{
						ThingDef thingDef2 = thingDef.building.smoothedThing;
						if (thingDef2.building == null)
						{
							Log.Error(string.Format("{0} is smoothable to non-building {1}", thingDef, thingDef2), false);
						}
						else if (thingDef2.building.unsmoothedThing == null || thingDef2.building.unsmoothedThing == thingDef)
						{
							thingDef2.building.unsmoothedThing = thingDef;
						}
						else
						{
							Log.Error(string.Format("{0} and {1} both smooth to {2}", thingDef, thingDef2.building.unsmoothedThing, thingDef2), false);
						}
					}
				}
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0005E2C4 File Offset: 0x0005C6C4
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (this.joyKind != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_JoyKind".Translate(), this.joyKind.LabelCap, 0, "");
			}
			yield break;
		}
	}
}
