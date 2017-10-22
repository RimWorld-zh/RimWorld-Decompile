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

		private const float DistTouch = 4f;

		private const float DistShort = 15f;

		private const float DistMedium = 30f;

		private const float DistLong = 50f;

		public VerbCategory category = VerbCategory.Nonnative;

		public Type verbClass = typeof(Verb);

		public string label;

		public bool isPrimary = true;

		public float minRange;

		public float range = 1f;

		public int burstShotCount = 1;

		public int ticksBetweenBurstShots = 15;

		public float noiseRadius = 3f;

		public bool hasStandardCommand;

		public bool targetable = true;

		public TargetingParameters targetParams = new TargetingParameters();

		public bool requireLineOfSight = true;

		public bool mustCastOnOpenGround;

		public bool forceNormalTimeSpeed = true;

		public bool onlyManualCast;

		public bool stopBurstWithoutLos = true;

		public SurpriseAttackProps surpriseAttack;

		public float commonality = 1f;

		public float warmupTime;

		public float defaultCooldownTime;

		public SoundDef soundCast;

		public SoundDef soundCastTail;

		public float muzzleFlashScale;

		public BodyPartGroupDef linkedBodyPartsGroup;

		public DamageDef meleeDamageDef;

		public int meleeDamageBaseAmount = 1;

		public bool ai_IsWeapon = true;

		public bool ai_IsIncendiary;

		public bool ai_IsBuildingDestroyer;

		public float ai_AvoidFriendlyFireRadius;

		public ThingDef projectileDef;

		public float forcedMissRadius;

		public float accuracyTouch = 1f;

		public float accuracyShort = 1f;

		public float accuracyMedium = 1f;

		public float accuracyLong = 1f;

		public bool MeleeRange
		{
			get
			{
				return this.range < 1.1000000238418579;
			}
		}

		public bool NeedsLineOfSight
		{
			get
			{
				return !this.projectileDef.projectile.flyOverhead;
			}
		}

		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		public string AccuracySummaryString
		{
			get
			{
				return this.accuracyTouch.ToStringPercent() + " - " + this.accuracyShort.ToStringPercent() + " - " + this.accuracyMedium.ToStringPercent() + " - " + this.accuracyLong.ToStringPercent();
			}
		}

		public float BaseSelectionWeight
		{
			get
			{
				return this.AdjustedSelectionWeight(null, null, null);
			}
		}

		public int AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float num = (ownerVerb == null || ownerVerb.ownerEquipment == null) ? ((float)this.meleeDamageBaseAmount) : ownerVerb.ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageAmount, true);
			if (attacker != null)
			{
				num *= ownerVerb.GetDamageFactorFor(attacker);
			}
			return Mathf.Max(1, Mathf.RoundToInt(num));
		}

		public float AdjustedSelectionWeight(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return (float)this.AdjustedMeleeDamageAmount(ownerVerb, attacker, equipment) * this.commonality;
		}

		public int AdjustedCooldownTicks(Thing equipment)
		{
			if (equipment != null)
			{
				if (this.MeleeRange)
				{
					return equipment.GetStatValue(StatDefOf.MeleeWeapon_Cooldown, true).SecondsToTicks();
				}
				return equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true).SecondsToTicks();
			}
			return this.defaultCooldownTime.SecondsToTicks();
		}

		private float AdjustedAccuracy(RangeCategory cat, Thing equipment)
		{
			if (equipment == null)
			{
				switch (cat)
				{
				case RangeCategory.Touch:
				{
					return this.accuracyTouch;
				}
				case RangeCategory.Short:
				{
					return this.accuracyShort;
				}
				case RangeCategory.Medium:
				{
					return this.accuracyMedium;
				}
				case RangeCategory.Long:
				{
					return this.accuracyLong;
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
			}
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
			return equipment.GetStatValue(stat, true);
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
			string str = this.label.NullOrEmpty() ? ("range=" + this.range + ", projectile=" + ((this.projectileDef == null) ? "null" : this.projectileDef.defName)) : this.label;
			return "VerbProperties(" + str + ")";
		}
	}
}
