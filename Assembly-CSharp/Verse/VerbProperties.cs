using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class VerbProperties
	{
		public VerbCategory category = VerbCategory.Nonnative;

		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		[MustTranslate]
		public string label = null;

		[TranslationHandle(Priority = 100)]
		[Unsaved]
		public string untranslatedLabel = null;

		public bool isPrimary = true;

		public float minRange = 0f;

		public float range = 1.42f;

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

		public Intelligence minIntelligence = Intelligence.Animal;

		public float consumeFuelPerShot = 0f;

		public float warmupTime = 0f;

		public float defaultCooldownTime = 0f;

		public SoundDef soundCast = null;

		public SoundDef soundCastTail = null;

		public SoundDef soundAiming;

		public float muzzleFlashScale = 0f;

		public ThingDef impactMote = null;

		public BodyPartGroupDef linkedBodyPartsGroup = null;

		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		public DamageDef meleeDamageDef = null;

		public int meleeDamageBaseAmount = 1;

		public float meleeArmorPenetrationBase = -1f;

		public bool ai_IsWeapon = true;

		public bool ai_IsBuildingDestroyer = false;

		public float ai_AvoidFriendlyFireRadius;

		public ThingDef defaultProjectile;

		public float forcedMissRadius = 0f;

		public float accuracyTouch = 1f;

		public float accuracyShort = 1f;

		public float accuracyMedium = 1f;

		public float accuracyLong = 1f;

		public ThingDef spawnDef;

		public TaleDef colonyWideTaleDef;

		public BodyPartTagDef bodypartTagTarget;

		public RulePackDef rangedFireRulepack = null;

		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		public VerbProperties()
		{
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

		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass));
			}
		}

		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			if ((ownerVerb == null) ? (!this.IsMeleeAttack) : (!ownerVerb.IsMeleeAttack))
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238, false);
			}
			float num;
			if (ownerVerb != null && ownerVerb.tool != null)
			{
				num = ownerVerb.tool.AdjustedBaseMeleeDamageAmount(ownerVerb.ownerEquipment, this.meleeDamageDef);
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

		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float num;
			if (ownerVerb != null && ownerVerb.tool != null)
			{
				num = ownerVerb.tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				float num2 = this.AdjustedMeleeDamageAmount(ownerVerb, attacker, equipment);
				num = num2 * 0.015f;
			}
			return num;
		}

		private float AdjustedExpectedDamageForVerbWhichCanBeUsedInMelee(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float result;
			if (this.IsMeleeAttack)
			{
				result = this.AdjustedMeleeDamageAmount(ownerVerb, attacker, equipment);
			}
			else if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				result = (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, null);
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			float result;
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				result = 0f;
			}
			else
			{
				float num = 1f;
				float num2 = this.AdjustedExpectedDamageForVerbWhichCanBeUsedInMelee(ownerVerb, attacker, equipment);
				if (num2 >= 0.001f || !(ownerVerb is Verb_MeleeApplyHediff))
				{
					num *= num2 * num2;
				}
				num *= this.commonality;
				if (ownerVerb.tool != null)
				{
					num *= ownerVerb.tool.commonality;
				}
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

		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.AdjustedCooldown(ownerVerb, attacker, equipment).SecondsToTicks();
		}

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

		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker, Thing equipment)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker, equipment) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float value;
			if (dist <= 3f)
			{
				value = this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment);
			}
			else if (dist <= 12f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Touch, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), (dist - 3f) / 9f);
			}
			else if (dist <= 25f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Short, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), (dist - 12f) / 13f);
			}
			else if (dist <= 40f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(VerbProperties.RangeCategory.Medium, equipment), this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment), (dist - 25f) / 15f);
			}
			else
			{
				value = this.AdjustedAccuracy(VerbProperties.RangeCategory.Long, equipment);
			}
			return Mathf.Clamp(value, 0.01f, 1f);
		}

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

		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

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

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		private enum RangeCategory : byte
		{
			Touch,
			Short,
			Medium,
			Long
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal ThingDef parent;

			internal VerbProperties $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (parent.race != null && this.linkedBodyPartsGroup != null && !parent.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.linkedBodyPartsGroup)))
					{
						this.$current = string.Concat(new object[]
						{
							"has verb with linkedBodyPartsGroup ",
							this.linkedBodyPartsGroup,
							" but body ",
							parent.race.body,
							" has no parts with that group."
						});
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_135;
				default:
					return false;
				}
				if (base.LaunchesProjectile && this.defaultProjectile != null)
				{
					if (this.forcedMissRadius > 0f != base.CausesExplosion)
					{
						this.$current = "has incorrect forcedMiss settings; explosive projectiles and only explosive projectiles should have forced miss enabled";
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
				}
				IL_135:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				VerbProperties.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new VerbProperties.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.parent = parent;
				return <ConfigErrors>c__Iterator;
			}

			internal bool <>m__0(BodyPartRecord part)
			{
				return part.groups.Contains(this.linkedBodyPartsGroup);
			}
		}
	}
}
