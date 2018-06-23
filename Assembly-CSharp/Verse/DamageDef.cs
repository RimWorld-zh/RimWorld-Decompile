using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B04 RID: 2820
	public class DamageDef : Def
	{
		// Token: 0x040027B0 RID: 10160
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x040027B1 RID: 10161
		public bool externalViolence = false;

		// Token: 0x040027B2 RID: 10162
		public bool hasForcefulImpact = true;

		// Token: 0x040027B3 RID: 10163
		public bool harmsHealth = true;

		// Token: 0x040027B4 RID: 10164
		public bool makesBlood = true;

		// Token: 0x040027B5 RID: 10165
		public bool canInterruptJobs = true;

		// Token: 0x040027B6 RID: 10166
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x040027B7 RID: 10167
		public ImpactSoundTypeDef impactSoundType = null;

		// Token: 0x040027B8 RID: 10168
		public DamageArmorCategoryDef armorCategory = null;

		// Token: 0x040027B9 RID: 10169
		public int minDamageToFragment = 99999;

		// Token: 0x040027BA RID: 10170
		public bool execution = false;

		// Token: 0x040027BB RID: 10171
		public RulePackDef combatLogRules = null;

		// Token: 0x040027BC RID: 10172
		public int defaultDamage = -1;

		// Token: 0x040027BD RID: 10173
		public bool isRanged;

		// Token: 0x040027BE RID: 10174
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x040027BF RID: 10175
		public bool isExplosive = false;

		// Token: 0x040027C0 RID: 10176
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x040027C1 RID: 10177
		public float explosionBuildingDamageFactor = 1f;

		// Token: 0x040027C2 RID: 10178
		public float explosionPlantDamageFactor = 1f;

		// Token: 0x040027C3 RID: 10179
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x040027C4 RID: 10180
		public ThingDef explosionCellMote = null;

		// Token: 0x040027C5 RID: 10181
		public Color explosionColorCenter = Color.white;

		// Token: 0x040027C6 RID: 10182
		public Color explosionColorEdge = Color.white;

		// Token: 0x040027C7 RID: 10183
		public ThingDef explosionInteriorMote;

		// Token: 0x040027C8 RID: 10184
		public float explosionHeatEnergyPerCell = 0f;

		// Token: 0x040027C9 RID: 10185
		public SoundDef soundExplosion = null;

		// Token: 0x040027CA RID: 10186
		public bool harmAllLayersUntilOutside = false;

		// Token: 0x040027CB RID: 10187
		public HediffDef hediff = null;

		// Token: 0x040027CC RID: 10188
		public HediffDef hediffSkin = null;

		// Token: 0x040027CD RID: 10189
		public HediffDef hediffSolid = null;

		// Token: 0x040027CE RID: 10190
		public float stabChanceOfForcedInternal = 0f;

		// Token: 0x040027CF RID: 10191
		public float stabPierceBonus = 0f;

		// Token: 0x040027D0 RID: 10192
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x040027D1 RID: 10193
		public float cutCleaveBonus;

		// Token: 0x040027D2 RID: 10194
		public float bluntInnerHitChance = 0f;

		// Token: 0x040027D3 RID: 10195
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x040027D4 RID: 10196
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x040027D5 RID: 10197
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve = null;

		// Token: 0x040027D6 RID: 10198
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve = null;

		// Token: 0x040027D7 RID: 10199
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x040027D8 RID: 10200
		public float biteDamageMultiplier = 1f;

		// Token: 0x040027D9 RID: 10201
		public List<DamageDefAdditionalHediff> additionalHediffs = null;

		// Token: 0x040027DA RID: 10202
		[Unsaved]
		private DamageWorker workerInt = null;

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x0020F3C8 File Offset: 0x0020D7C8
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
