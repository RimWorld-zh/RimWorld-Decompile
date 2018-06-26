using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class DamageDef : Def
	{
		public Type workerClass = typeof(DamageWorker);

		public bool externalViolence = false;

		public bool hasForcefulImpact = true;

		public bool harmsHealth = true;

		public bool makesBlood = true;

		public bool canInterruptJobs = true;

		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		public ImpactSoundTypeDef impactSoundType = null;

		public DamageArmorCategoryDef armorCategory = null;

		public int minDamageToFragment = 99999;

		public bool execution = false;

		public RulePackDef combatLogRules = null;

		public int defaultDamage = -1;

		public float defaultArmorPenetration = -1f;

		public bool isRanged;

		public bool canUseDeflectMetalEffect = true;

		public bool isExplosive = false;

		public float explosionSnowMeltAmount = 1f;

		public float explosionBuildingDamageFactor = 1f;

		public float explosionPlantDamageFactor = 1f;

		public bool explosionAffectOutsidePartsOnly = true;

		public ThingDef explosionCellMote = null;

		public Color explosionColorCenter = Color.white;

		public Color explosionColorEdge = Color.white;

		public ThingDef explosionInteriorMote;

		public float explosionHeatEnergyPerCell = 0f;

		public SoundDef soundExplosion = null;

		public bool harmAllLayersUntilOutside = false;

		public HediffDef hediff = null;

		public HediffDef hediffSkin = null;

		public HediffDef hediffSolid = null;

		public float stabChanceOfForcedInternal = 0f;

		public float stabPierceBonus = 0f;

		public SimpleCurve cutExtraTargetsCurve;

		public float cutCleaveBonus;

		public float bluntInnerHitChance = 0f;

		public FloatRange bluntInnerHitDamageFractionToConvert;

		public FloatRange bluntInnerHitDamageFractionToAdd;

		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve = null;

		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve = null;

		public float scratchSplitPercentage = 0.5f;

		public float biteDamageMultiplier = 1f;

		public List<DamageDefAdditionalHediff> additionalHediffs = null;

		[Unsaved]
		private DamageWorker workerInt = null;

		public DamageDef()
		{
		}

		public DamageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (DamageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
