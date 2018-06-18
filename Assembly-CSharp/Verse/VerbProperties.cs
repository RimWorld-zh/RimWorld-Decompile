using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B34 RID: 2868
	public class VerbProperties
	{
		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x00213198 File Offset: 0x00211598
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x002131C4 File Offset: 0x002115C4
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003F0F RID: 16143 RVA: 0x002131F0 File Offset: 0x002115F0
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

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003F10 RID: 16144 RVA: 0x00213260 File Offset: 0x00211660
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06003F11 RID: 16145 RVA: 0x0021328C File Offset: 0x0021168C
		public bool CanBeUsedInMelee
		{
			get
			{
				return this.IsMeleeAttack || this.meleeShoot;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x002132B8 File Offset: 0x002116B8
		public float BaseMeleeSelectionWeight
		{
			get
			{
				return this.AdjustedMeleeSelectionWeight(null, null, null);
			}
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x002132D8 File Offset: 0x002116D8
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

		// Token: 0x06003F14 RID: 16148 RVA: 0x00213370 File Offset: 0x00211770
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

		// Token: 0x06003F15 RID: 16149 RVA: 0x002133D4 File Offset: 0x002117D4
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
				else if (this.meleeShoot)
				{
					result = num * 0.25f / equipment.GetStatValue(StatDefOf.Weapon_Bulk, true);
				}
				else
				{
					result = 0f;
				}
			}
			return result;
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x002134E8 File Offset: 0x002118E8
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

		// Token: 0x06003F17 RID: 16151 RVA: 0x00213544 File Offset: 0x00211944
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.AdjustedCooldown(ownerVerb, attacker, equipment).SecondsToTicks();
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x00213568 File Offset: 0x00211968
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

		// Token: 0x06003F19 RID: 16153 RVA: 0x00213628 File Offset: 0x00211A28
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker, equipment) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00213664 File Offset: 0x00211A64
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

		// Token: 0x06003F1B RID: 16155 RVA: 0x00213760 File Offset: 0x00211B60
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

		// Token: 0x06003F1C RID: 16156 RVA: 0x00213804 File Offset: 0x00211C04
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

		// Token: 0x06003F1D RID: 16157 RVA: 0x00213880 File Offset: 0x00211C80
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x040028FB RID: 10491
		public VerbCategory category = VerbCategory.Nonnative;

		// Token: 0x040028FC RID: 10492
		public Type verbClass = typeof(Verb);

		// Token: 0x040028FD RID: 10493
		[MustTranslate]
		public string label = null;

		// Token: 0x040028FE RID: 10494
		public bool isPrimary = true;

		// Token: 0x040028FF RID: 10495
		public float minRange = 0f;

		// Token: 0x04002900 RID: 10496
		public float range = 1.42f;

		// Token: 0x04002901 RID: 10497
		public int burstShotCount = 1;

		// Token: 0x04002902 RID: 10498
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04002903 RID: 10499
		public float noiseRadius = 3f;

		// Token: 0x04002904 RID: 10500
		public bool hasStandardCommand = false;

		// Token: 0x04002905 RID: 10501
		public bool targetable = true;

		// Token: 0x04002906 RID: 10502
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x04002907 RID: 10503
		public bool requireLineOfSight = true;

		// Token: 0x04002908 RID: 10504
		public bool mustCastOnOpenGround = false;

		// Token: 0x04002909 RID: 10505
		public bool forceNormalTimeSpeed = true;

		// Token: 0x0400290A RID: 10506
		public bool onlyManualCast = false;

		// Token: 0x0400290B RID: 10507
		public bool stopBurstWithoutLos = true;

		// Token: 0x0400290C RID: 10508
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x0400290D RID: 10509
		public float commonality = 1f;

		// Token: 0x0400290E RID: 10510
		public Intelligence minIntelligence = Intelligence.Animal;

		// Token: 0x0400290F RID: 10511
		public float consumeFuelPerShot = 0f;

		// Token: 0x04002910 RID: 10512
		public float warmupTime = 0f;

		// Token: 0x04002911 RID: 10513
		public float defaultCooldownTime = 0f;

		// Token: 0x04002912 RID: 10514
		public SoundDef soundCast = null;

		// Token: 0x04002913 RID: 10515
		public SoundDef soundCastTail = null;

		// Token: 0x04002914 RID: 10516
		public SoundDef soundAiming;

		// Token: 0x04002915 RID: 10517
		public float muzzleFlashScale = 0f;

		// Token: 0x04002916 RID: 10518
		public ThingDef impactMote = null;

		// Token: 0x04002917 RID: 10519
		public BodyPartGroupDef linkedBodyPartsGroup = null;

		// Token: 0x04002918 RID: 10520
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04002919 RID: 10521
		public DamageDef meleeDamageDef = null;

		// Token: 0x0400291A RID: 10522
		public int meleeDamageBaseAmount = 1;

		// Token: 0x0400291B RID: 10523
		public bool ai_IsWeapon = true;

		// Token: 0x0400291C RID: 10524
		public bool ai_IsBuildingDestroyer = false;

		// Token: 0x0400291D RID: 10525
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x0400291E RID: 10526
		public ThingDef defaultProjectile;

		// Token: 0x0400291F RID: 10527
		public float forcedMissRadius = 0f;

		// Token: 0x04002920 RID: 10528
		public float accuracyTouch = 1f;

		// Token: 0x04002921 RID: 10529
		public float accuracyShort = 1f;

		// Token: 0x04002922 RID: 10530
		public float accuracyMedium = 1f;

		// Token: 0x04002923 RID: 10531
		public float accuracyLong = 1f;

		// Token: 0x04002924 RID: 10532
		public ThingDef spawnDef;

		// Token: 0x04002925 RID: 10533
		public TaleDef colonyWideTaleDef;

		// Token: 0x04002926 RID: 10534
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x04002927 RID: 10535
		[Unsaved]
		public bool meleeShoot;

		// Token: 0x04002928 RID: 10536
		public RulePackDef rangedFireRulepack = null;

		// Token: 0x04002929 RID: 10537
		public const float DistTouch = 4f;

		// Token: 0x0400292A RID: 10538
		public const float DistShort = 15f;

		// Token: 0x0400292B RID: 10539
		public const float DistMedium = 30f;

		// Token: 0x0400292C RID: 10540
		public const float DistLong = 50f;

		// Token: 0x0400292D RID: 10541
		public const float MeleeRange = 1.42f;

		// Token: 0x0400292E RID: 10542
		private const float MeleeGunfireWeighting = 0.25f;

		// Token: 0x0400292F RID: 10543
		private const float BodypartVerbWeighting = 0.3f;

		// Token: 0x02000B35 RID: 2869
		private enum RangeCategory : byte
		{
			// Token: 0x04002931 RID: 10545
			Touch,
			// Token: 0x04002932 RID: 10546
			Short,
			// Token: 0x04002933 RID: 10547
			Medium,
			// Token: 0x04002934 RID: 10548
			Long
		}
	}
}
