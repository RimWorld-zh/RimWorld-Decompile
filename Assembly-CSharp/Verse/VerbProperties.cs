using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B33 RID: 2867
	public class VerbProperties
	{
		// Token: 0x040028FF RID: 10495
		public VerbCategory category = VerbCategory.Nonnative;

		// Token: 0x04002900 RID: 10496
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x04002901 RID: 10497
		[MustTranslate]
		public string label = null;

		// Token: 0x04002902 RID: 10498
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel = null;

		// Token: 0x04002903 RID: 10499
		public bool isPrimary = true;

		// Token: 0x04002904 RID: 10500
		public float minRange = 0f;

		// Token: 0x04002905 RID: 10501
		public float range = 1.42f;

		// Token: 0x04002906 RID: 10502
		public int burstShotCount = 1;

		// Token: 0x04002907 RID: 10503
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04002908 RID: 10504
		public float noiseRadius = 3f;

		// Token: 0x04002909 RID: 10505
		public bool hasStandardCommand = false;

		// Token: 0x0400290A RID: 10506
		public bool targetable = true;

		// Token: 0x0400290B RID: 10507
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x0400290C RID: 10508
		public bool requireLineOfSight = true;

		// Token: 0x0400290D RID: 10509
		public bool mustCastOnOpenGround = false;

		// Token: 0x0400290E RID: 10510
		public bool forceNormalTimeSpeed = true;

		// Token: 0x0400290F RID: 10511
		public bool onlyManualCast = false;

		// Token: 0x04002910 RID: 10512
		public bool stopBurstWithoutLos = true;

		// Token: 0x04002911 RID: 10513
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04002912 RID: 10514
		public float commonality = 1f;

		// Token: 0x04002913 RID: 10515
		public Intelligence minIntelligence = Intelligence.Animal;

		// Token: 0x04002914 RID: 10516
		public float consumeFuelPerShot = 0f;

		// Token: 0x04002915 RID: 10517
		public float warmupTime = 0f;

		// Token: 0x04002916 RID: 10518
		public float defaultCooldownTime = 0f;

		// Token: 0x04002917 RID: 10519
		public SoundDef soundCast = null;

		// Token: 0x04002918 RID: 10520
		public SoundDef soundCastTail = null;

		// Token: 0x04002919 RID: 10521
		public SoundDef soundAiming;

		// Token: 0x0400291A RID: 10522
		public float muzzleFlashScale = 0f;

		// Token: 0x0400291B RID: 10523
		public ThingDef impactMote = null;

		// Token: 0x0400291C RID: 10524
		public BodyPartGroupDef linkedBodyPartsGroup = null;

		// Token: 0x0400291D RID: 10525
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x0400291E RID: 10526
		public DamageDef meleeDamageDef = null;

		// Token: 0x0400291F RID: 10527
		public int meleeDamageBaseAmount = 1;

		// Token: 0x04002920 RID: 10528
		public bool ai_IsWeapon = true;

		// Token: 0x04002921 RID: 10529
		public bool ai_IsBuildingDestroyer = false;

		// Token: 0x04002922 RID: 10530
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x04002923 RID: 10531
		public ThingDef defaultProjectile;

		// Token: 0x04002924 RID: 10532
		public float forcedMissRadius = 0f;

		// Token: 0x04002925 RID: 10533
		public float accuracyTouch = 1f;

		// Token: 0x04002926 RID: 10534
		public float accuracyShort = 1f;

		// Token: 0x04002927 RID: 10535
		public float accuracyMedium = 1f;

		// Token: 0x04002928 RID: 10536
		public float accuracyLong = 1f;

		// Token: 0x04002929 RID: 10537
		public ThingDef spawnDef;

		// Token: 0x0400292A RID: 10538
		public TaleDef colonyWideTaleDef;

		// Token: 0x0400292B RID: 10539
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x0400292C RID: 10540
		public RulePackDef rangedFireRulepack = null;

		// Token: 0x0400292D RID: 10541
		public const float DistTouch = 4f;

		// Token: 0x0400292E RID: 10542
		public const float DistShort = 15f;

		// Token: 0x0400292F RID: 10543
		public const float DistMedium = 30f;

		// Token: 0x04002930 RID: 10544
		public const float DistLong = 50f;

		// Token: 0x04002931 RID: 10545
		public const float MeleeRange = 1.42f;

		// Token: 0x04002932 RID: 10546
		private const float MeleeGunfireWeighting = 0.25f;

		// Token: 0x04002933 RID: 10547
		private const float BodypartVerbWeighting = 0.3f;

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x002138E8 File Offset: 0x00211CE8
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x00213914 File Offset: 0x00211D14
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003F0F RID: 16143 RVA: 0x00213940 File Offset: 0x00211D40
		public string AccuracySummaryString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.accuracyTouch.ToStringPercent(),
					" - ",
					this.accuracyShort.ToStringPercent(),
					" - ",
					this.accuracyMedium.ToStringPercent(),
					" - ",
					this.accuracyLong.ToStringPercent()
				});
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06003F10 RID: 16144 RVA: 0x002139B0 File Offset: 0x00211DB0
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06003F11 RID: 16145 RVA: 0x002139DC File Offset: 0x00211DDC
		public float BaseMeleeSelectionWeight
		{
			get
			{
				return this.AdjustedMeleeSelectionWeight(null, null, null);
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x002139FC File Offset: 0x00211DFC
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass));
			}
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x00213A60 File Offset: 0x00211E60
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			if ((ownerVerb == null) ? (!this.IsMeleeAttack) : (!ownerVerb.IsMeleeAttack))
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238, false);
			}
			float num;
			if (ownerVerb != null && ownerVerb.tool != null)
			{
				num = ownerVerb.tool.AdjustedMeleeDamageAmount(ownerVerb.ownerEquipment, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= ownerVerb.GetDamageFactorFor(attacker);
			}
			return num;
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x00213AF8 File Offset: 0x00211EF8
		private float AdjustedExpectedMeleeDamage(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float result;
			if (this.IsMeleeAttack)
			{
				result = this.AdjustedMeleeDamageAmount(ownerVerb, attacker, equipment);
			}
			else if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				result = (float)this.defaultProjectile.projectile.DamageAmount;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x00213B5C File Offset: 0x00211F5C
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float result;
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				result = 0f;
			}
			else
			{
				float num = this.AdjustedExpectedMeleeDamage(ownerVerb, attacker, equipment) * this.commonality * ((ownerVerb.tool != null) ? ownerVerb.tool.commonality : 1f);
				if (this.IsMeleeAttack && equipment != null)
				{
					result = num;
				}
				else if (this.IsMeleeAttack && ownerVerb.terrainDef != null)
				{
					result = num;
				}
				else if (this.IsMeleeAttack && ownerVerb.tool != null && ownerVerb.tool.alwaysTreatAsWeapon)
				{
					result = num;
				}
				else if (this.IsMeleeAttack)
				{
					result = num * 0.3f;
				}
				else
				{
					result = 0f;
				}
			}
			return result;
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x00213C4C File Offset: 0x0021204C
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float result;
			if (ownerVerb.tool != null)
			{
				result = ownerVerb.tool.AdjustedCooldown(equipment);
			}
			else if (equipment != null && !this.IsMeleeAttack)
			{
				result = equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true);
			}
			else
			{
				result = this.defaultCooldownTime;
			}
			return result;
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x00213CA8 File Offset: 0x002120A8
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.AdjustedCooldown(ownerVerb, attacker, equipment).SecondsToTicks();
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x00213CCC File Offset: 0x002120CC
		private float AdjustedAccuracy(VerbProperties.RangeCategory cat, Thing equipment)
		{
			float statValue;
			if (equipment == null)
			{
				switch (cat)
				{
				case VerbProperties.RangeCategory.Touch:
					statValue = this.accuracyTouch;
					break;
				case VerbProperties.RangeCategory.Short:
					statValue = this.accuracyShort;
					break;
				case VerbProperties.RangeCategory.Medium:
					statValue = this.accuracyMedium;
					break;
				case VerbProperties.RangeCategory.Long:
					statValue = this.accuracyLong;
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			else
			{
				StatDef stat = null;
				switch (cat)
				{
				case VerbProperties.RangeCategory.Touch:
					stat = StatDefOf.AccuracyTouch;
					break;
				case VerbProperties.RangeCategory.Short:
					stat = StatDefOf.AccuracyShort;
					break;
				case VerbProperties.RangeCategory.Medium:
					stat = StatDefOf.AccuracyMedium;
					break;
				case VerbProperties.RangeCategory.Long:
					stat = StatDefOf.AccuracyLong;
					break;
				}
				statValue = equipment.GetStatValue(stat, true);
			}
			return statValue;
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x00213D8C File Offset: 0x0021218C
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker, equipment) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00213DC8 File Offset: 0x002121C8
		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float num;
			if (dist <= 4f)
			{
				num = this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment);
			}
			else if (dist <= 15f)
			{
				num = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), (dist - 4f) / 11f);
			}
			else if (dist <= 30f)
			{
				num = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), (dist - 15f) / 15f);
			}
			else if (dist <= 50f)
			{
				num = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment), (dist - 30f) / 20f);
			}
			else
			{
				num = this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment);
			}
			if (num < 0.01f)
			{
				num = 0.01f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x00213EC4 File Offset: 0x002122C4
		public void DrawRadiusRing(IntVec3 center)
		{
			if (Find.CurrentMap != null)
			{
				if (!this.IsMeleeAttack)
				{
					if (this.minRange > 0f && this.minRange < GenRadial.MaxRadialPatternRadius)
					{
						GenDraw.DrawRadiusRing(center, this.minRange);
					}
					if (this.range < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && this.range < GenRadial.MaxRadialPatternRadius)
					{
						GenDraw.DrawRadiusRing(center, this.range);
					}
				}
			}
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x00213F68 File Offset: 0x00212368
		public override string ToString()
		{
			string str;
			if (!this.label.NullOrEmpty())
			{
				str = this.label;
			}
			else
			{
				str = string.Concat(new object[]
				{
					"range=",
					this.range,
					", defaultProjectile=",
					this.defaultProjectile.ToStringSafe<ThingDef>()
				});
			}
			return "VerbProperties(" + str + ")";
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x00213FE4 File Offset: 0x002123E4
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x00214004 File Offset: 0x00212404
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (parent.race != null && this.linkedBodyPartsGroup != null && !parent.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.linkedBodyPartsGroup)))
			{
				yield return string.Concat(new object[]
				{
					"has verb with linkedBodyPartsGroup ",
					this.linkedBodyPartsGroup,
					" but body ",
					parent.race.body,
					" has no parts with that group."
				});
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				if (this.forcedMissRadius > 0f != this.CausesExplosion)
				{
					yield return "has incorrect forcedMiss settings; explosive projectiles and only explosive projectiles should have forced miss enabled";
				}
			}
			yield break;
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x00214035 File Offset: 0x00212435
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x02000B34 RID: 2868
		private enum RangeCategory : byte
		{
			// Token: 0x04002935 RID: 10549
			Touch,
			// Token: 0x04002936 RID: 10550
			Short,
			// Token: 0x04002937 RID: 10551
			Medium,
			// Token: 0x04002938 RID: 10552
			Long
		}
	}
}
