using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class BuildingProperties
	{
		public bool isEdifice = true;

		public List<string> buildingTags = new List<string>();

		public bool isInert = false;

		private bool deconstructible = true;

		public bool alwaysDeconstructible = false;

		public bool claimable = true;

		public bool isSittable = false;

		public SoundDef soundAmbient;

		public ConceptDef spawnedConceptLearnOpportunity = null;

		public ConceptDef boughtConceptLearnOpportunity = null;

		public bool expandHomeArea = true;

		public bool wantsHopperAdjacent = false;

		public bool allowWireConnection = true;

		public bool shipPart = false;

		public bool canPlaceOverImpassablePlant = true;

		public float heatPerTickWhileWorking = 0f;

		public bool canBuildNonEdificesUnder = true;

		public bool canPlaceOverWall = false;

		public bool allowAutoroof = true;

		public bool preventDeteriorationOnTop = false;

		public bool preventDeteriorationInside = false;

		public bool isMealSource = false;

		public bool isJoySource = false;

		public bool isNaturalRock = false;

		public bool isResourceRock = false;

		public bool repairable = true;

		public float roofCollapseDamageMultiplier = 1f;

		public bool hasFuelingPort;

		public bool isPlayerEjectable = false;

		public GraphicData fullGraveGraphicData = null;

		public float bed_healPerDay = 0f;

		public bool bed_defaultMedical = false;

		public bool bed_showSleeperBody = false;

		public bool bed_humanlike = true;

		public float bed_maxBodySize = 9999f;

		public float nutritionCostPerDispense;

		public SoundDef soundDispense;

		public ThingDef turretGunDef;

		public float turretBurstWarmupTime = 0f;

		public float turretBurstCooldownTime = -1f;

		public string turretTopGraphicPath = (string)null;

		[Unsaved]
		public Material turretTopMat;

		public bool ai_combatDangerous = false;

		public bool ai_chillDestination = true;

		public SoundDef soundDoorOpenPowered = SoundDefOf.DoorOpen;

		public SoundDef soundDoorClosePowered = SoundDefOf.DoorClose;

		public SoundDef soundDoorOpenManual = SoundDefOf.DoorOpenManual;

		public SoundDef soundDoorCloseManual = SoundDefOf.DoorCloseManual;

		public string sowTag = (string)null;

		public ThingDef defaultPlantToGrow = null;

		public ThingDef mineableThing = null;

		public int mineableYield = 1;

		public float mineableNonMinedEfficiency = 0.7f;

		public float mineableDropChance = 1f;

		public bool mineableYieldWasteable = true;

		public float mineableScatterCommonality = 0f;

		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		public StorageSettings fixedStorageSettings = null;

		public StorageSettings defaultStorageSettings = null;

		public bool ignoreStoredThingsBeauty;

		public bool isTrap = false;

		public DamageArmorCategoryDef trapDamageCategory;

		public GraphicData trapUnarmedGraphicData;

		[Unsaved]
		public Graphic trapUnarmedGraphic;

		public float unpoweredWorkTableWorkSpeedFactor = 0f;

		public bool workSpeedPenaltyOutdoors = false;

		public bool workSpeedPenaltyTemperature = false;

		public IntRange watchBuildingStandDistanceRange = IntRange.one;

		public int watchBuildingStandRectWidth = 3;

		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		public bool IsMortar
		{
			get
			{
				return this.IsTurret && this.turretGunDef.HasComp(typeof(CompChangeableProjectile)) && this.turretGunDef.building.fixedStorageSettings.filter.Allows(ThingDefOf.Shell_HighExplosive);
			}
		}

		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.isTrap && !this.isEdifice)
			{
				yield return "isTrap but is not edifice. Code will break.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.alwaysDeconstructible && !this.deconstructible)
			{
				yield return "alwaysDeconstructible=true but deconstructible=false";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!parent.holdsRoof)
				yield break;
			if (this.isEdifice)
				yield break;
			yield return "holds roof but is not an edifice.";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public void PostLoadSpecial(ThingDef parent)
		{
		}

		public void ResolveReferencesSpecial()
		{
			if (!this.turretTopGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate
				{
					this.turretTopMat = MaterialPool.MatFrom(this.turretTopGraphicPath);
				});
			}
			if (this.fixedStorageSettings != null)
			{
				this.fixedStorageSettings.filter.ResolveReferences();
			}
			if (this.defaultStorageSettings == null && this.fixedStorageSettings != null)
			{
				this.defaultStorageSettings = new StorageSettings();
				this.defaultStorageSettings.CopyFrom(this.fixedStorageSettings);
			}
			if (this.defaultStorageSettings != null)
			{
				this.defaultStorageSettings.filter.ResolveReferences();
			}
		}
	}
}
