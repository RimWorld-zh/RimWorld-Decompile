using RimWorld;
using System;
using UnityEngine;

namespace Verse
{
	public class VerbProperties
	{
		private enum RangeCategory : byte
		{
			Touch = 0,
			Short = 1,
			Medium = 2,
			Long = 3
		}

		public VerbCategory category = VerbCategory.Nonnative;

		public Type verbClass = typeof(Verb);

		public string label = (string)null;

		public bool isPrimary = true;

		public float minRange = 0f;

		public float range = 1f;

		public int burstShotCount = 1;

		public int ticksBetweenBurstShots = 15;

		public float noiseRadius = 3f;

		public bool hasStandardCommand = false;

		public bool targetable = true;

		public TargetingParameters targetParams = new TargetingParameters();

		public bool requireLineOfSight = true;

		public bool mustCastOnOpenGround = false;

		public bool forceNormalTimeSpeed = true;

		public bool onlyManualCast = false;

		public bool stopBurstWithoutLos = true;

		public SurpriseAttackProps surpriseAttack;

		public float commonality = 1f;

		public float warmupTime = 0f;

		public float defaultCooldownTime = 0f;

		public SoundDef soundCast = null;

		public SoundDef soundCastTail = null;

		public SoundDef soundAiming;

		public float muzzleFlashScale = 0f;

		public BodyPartGroupDef linkedBodyPartsGroup = null;

		public DamageDef meleeDamageDef = null;

		public int meleeDamageBaseAmount = 1;

		public bool ai_IsWeapon = true;

		public bool ai_IsBuildingDestroyer = false;

		public float ai_AvoidFriendlyFireRadius;

		public ThingDef defaultProjectile;

		public float forcedMissRadius = 0f;

		public float accuracyTouch = 1f;

		public float accuracyShort = 1f;

		public float accuracyMedium = 1f;

		public float accuracyLong = 1f;

		public bool meleeShoot = false;

		private const float DistTouch = 4f;

		private const float DistShort = 15f;

		private const float DistMedium = 30f;

		private const float DistLong = 50f;

		private const float MeleeGunfireWeighting = 0.25f;

		public bool MeleeRange
		{
			get
			{
				return this.range < 1.1000000238418579;
			}
		}

		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		public string AccuracySummaryString
		{
			get
			{
				return this.accuracyTouch.ToStringPercent() + " - " + this.accuracyShort.ToStringPercent() + " - " + this.accuracyMedium.ToStringPercent() + " - " + this.accuracyLong.ToStringPercent();
			}
		}

		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		public float BaseMeleeSelectionWeight
		{
			get
			{
				return this.AdjustedMeleeSelectionWeight(null, null, null);
			}
		}

		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			if ((ownerVerb == null) ? (!typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass)) : (!(ownerVerb is Verb_MeleeAttack)))
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
			}
			float num = (ownerVerb == null || ownerVerb.tool == null) ? ((float)this.meleeDamageBaseAmount) : ownerVerb.tool.AdjustedMeleeDamageAmount(ownerVerb.ownerEquipment, this.meleeDamageDef);
			if (attacker != null)
			{
				num *= ownerVerb.GetDamageFactorFor(attacker);
			}
			return num;
		}

		private float AdjustedExpectedMeleeDamage(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return (float)((!this.IsMeleeAttack) ? ((!this.LaunchesProjectile || this.defaultProjectile == null) ? 0.0 : ((float)this.defaultProjectile.projectile.damageAmountBase)) : this.AdjustedMeleeDamageAmount(ownerVerb, attacker, equipment));
		}

		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float num = (float)(this.AdjustedExpectedMeleeDamage(ownerVerb, attacker, equipment) * this.commonality * ((ownerVerb.tool != null) ? ownerVerb.tool.commonality : 1.0));
			return (float)((!this.IsMeleeAttack) ? ((!this.meleeShoot) ? 0.0 : (num * 0.25 / equipment.GetStatValue(StatDefOf.Weapon_Bulk, true))) : num);
		}

		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return (ownerVerb.tool == null) ? ((equipment == null || this.MeleeRange) ? this.defaultCooldownTime : equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true)) : ownerVerb.tool.AdjustedCooldown(equipment);
		}

		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.AdjustedCooldown(ownerVerb, attacker, equipment).SecondsToTicks();
		}

		private float AdjustedAccuracy(RangeCategory cat, Thing equipment)
		{
			float statValue;
			if (equipment == null)
			{
				switch (cat)
				{
				case RangeCategory.Touch:
				{
					statValue = this.accuracyTouch;
					break;
				}
				case RangeCategory.Short:
				{
					statValue = this.accuracyShort;
					break;
				}
				case RangeCategory.Medium:
				{
					statValue = this.accuracyMedium;
					break;
				}
				case RangeCategory.Long:
				{
					statValue = this.accuracyLong;
					break;
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
			}
			else
			{
				StatDef stat = null;
				switch (cat)
				{
				case RangeCategory.Touch:
				{
					stat = StatDefOf.AccuracyTouch;
					break;
				}
				case RangeCategory.Short:
				{
					stat = StatDefOf.AccuracyShort;
					break;
				}
				case RangeCategory.Medium:
				{
					stat = StatDefOf.AccuracyMedium;
					break;
				}
				case RangeCategory.Long:
				{
					stat = StatDefOf.AccuracyLong;
					break;
				}
				}
				statValue = equipment.GetStatValue(stat, true);
			}
			return statValue;
		}

		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float num = (!(dist <= 4.0)) ? ((!(dist <= 15.0)) ? ((!(dist <= 30.0)) ? ((!(dist <= 50.0)) ? this.AdjustedAccuracy(RangeCategory.Long, equipment) : Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Medium, equipment), this.AdjustedAccuracy(RangeCategory.Long, equipment), (float)((dist - 30.0) / 20.0))) : Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Short, equipment), this.AdjustedAccuracy(RangeCategory.Medium, equipment), (float)((dist - 15.0) / 15.0))) : Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Touch, equipment), this.AdjustedAccuracy(RangeCategory.Short, equipment), (float)((dist - 4.0) / 11.0))) : this.AdjustedAccuracy(RangeCategory.Touch, equipment);
			if (num < 0.0099999997764825821)
			{
				num = 0.01f;
			}
			if (num > 1.0)
			{
				num = 1f;
			}
			return num;
		}

		public override string ToString()
		{
			string str = this.label.NullOrEmpty() ? ("range=" + this.range + ", defaultProjectile=" + this.defaultProjectile.ToStringSafe()) : this.label;
			return "VerbProperties(" + str + ")";
		}

		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}
	}
}
