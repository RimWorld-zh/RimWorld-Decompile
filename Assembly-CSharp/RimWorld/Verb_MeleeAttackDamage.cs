using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Verb_MeleeAttackDamage : Verb_MeleeAttack
	{
		public Verb_MeleeAttackDamage()
		{
		}

		private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
		{
			float damAmount = this.verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn, this.ownerEquipment);
			DamageDef damDef = this.verbProps.meleeDamageDef;
			BodyPartGroupDef bodyPartGroupDef = null;
			HediffDef hediffDef = null;
			damAmount = Rand.Range(damAmount * 0.8f, damAmount * 1.2f);
			if (base.CasterIsPawn)
			{
				bodyPartGroupDef = base.LinkedBodyPartsGroup;
				if (damAmount >= 1f)
				{
					if (this.ownerHediffComp != null)
					{
						hediffDef = this.ownerHediffComp.Def;
					}
				}
				else
				{
					damAmount = 1f;
					damDef = DamageDefOf.Blunt;
				}
			}
			ThingDef source;
			if (this.ownerEquipment != null)
			{
				source = this.ownerEquipment.def;
			}
			else
			{
				source = base.CasterPawn.def;
			}
			Vector3 direction = (target.Thing.Position - base.CasterPawn.Position).ToVector3();
			DamageDef def = damDef;
			float amount2 = (float)GenMath.RoundRandom(damAmount);
			Thing caster = this.caster;
			DamageInfo mainDinfo = new DamageInfo(def, amount2, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null);
			mainDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			mainDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
			mainDinfo.SetWeaponHediff(hediffDef);
			mainDinfo.SetAngle(direction);
			yield return mainDinfo;
			if (this.surpriseAttack && ((this.verbProps.surpriseAttack != null && !this.verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraMeleeDamage>()) || this.tool == null || this.tool.surpriseAttack == null || this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraMeleeDamage>()))
			{
				IEnumerable<ExtraMeleeDamage> extraDamages = Enumerable.Empty<ExtraMeleeDamage>();
				if (this.verbProps.surpriseAttack != null && this.verbProps.surpriseAttack.extraMeleeDamages != null)
				{
					extraDamages = extraDamages.Concat(this.verbProps.surpriseAttack.extraMeleeDamages);
				}
				if (this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraMeleeDamage>())
				{
					extraDamages = extraDamages.Concat(this.tool.surpriseAttack.extraMeleeDamages);
				}
				foreach (ExtraMeleeDamage extraDamage in extraDamages)
				{
					int amount = GenMath.RoundRandom((float)extraDamage.amount * base.GetDamageFactorFor(base.CasterPawn));
					def = extraDamage.def;
					amount2 = (float)amount;
					caster = this.caster;
					DamageInfo extraDinfo = new DamageInfo(def, amount2, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null);
					extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
					extraDinfo.SetWeaponHediff(hediffDef);
					extraDinfo.SetAngle(direction);
					yield return extraDinfo;
				}
			}
			yield break;
		}

		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			foreach (DamageInfo dinfo in this.DamageInfosToApply(target))
			{
				if (target.ThingDestroyed)
				{
					break;
				}
				result = target.Thing.TakeDamage(dinfo);
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <DamageInfosToApply>c__Iterator0 : IEnumerable, IEnumerable<DamageInfo>, IEnumerator, IDisposable, IEnumerator<DamageInfo>
		{
			internal float <damAmount>__0;

			internal DamageDef <damDef>__0;

			internal BodyPartGroupDef <bodyPartGroupDef>__0;

			internal HediffDef <hediffDef>__0;

			internal ThingDef <source>__0;

			internal LocalTargetInfo target;

			internal Vector3 <direction>__0;

			internal DamageInfo <mainDinfo>__0;

			internal IEnumerable<ExtraMeleeDamage> <extraDamages>__1;

			internal IEnumerator<ExtraMeleeDamage> $locvar0;

			internal ExtraMeleeDamage <extraDamage>__2;

			internal int <amount>__3;

			internal DamageInfo <extraDinfo>__3;

			internal Verb_MeleeAttackDamage $this;

			internal DamageInfo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <DamageInfosToApply>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
				{
					damAmount = this.verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn, this.ownerEquipment);
					damDef = this.verbProps.meleeDamageDef;
					bodyPartGroupDef = null;
					hediffDef = null;
					damAmount = Rand.Range(damAmount * 0.8f, damAmount * 1.2f);
					if (base.CasterIsPawn)
					{
						bodyPartGroupDef = base.LinkedBodyPartsGroup;
						if (damAmount >= 1f)
						{
							if (this.ownerHediffComp != null)
							{
								hediffDef = this.ownerHediffComp.Def;
							}
						}
						else
						{
							damAmount = 1f;
							damDef = DamageDefOf.Blunt;
						}
					}
					if (this.ownerEquipment != null)
					{
						source = this.ownerEquipment.def;
					}
					else
					{
						source = base.CasterPawn.def;
					}
					direction = (target.Thing.Position - base.CasterPawn.Position).ToVector3();
					DamageDef def = damDef;
					float amount2 = (float)GenMath.RoundRandom(damAmount);
					Thing caster = this.caster;
					mainDinfo = new DamageInfo(def, amount2, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null);
					mainDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					mainDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
					mainDinfo.SetWeaponHediff(hediffDef);
					mainDinfo.SetAngle(direction);
					this.$current = mainDinfo;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					if (!this.surpriseAttack || ((this.verbProps.surpriseAttack == null || this.verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraMeleeDamage>()) && this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraMeleeDamage>()))
					{
						goto IL_4BA;
					}
					extraDamages = Enumerable.Empty<ExtraMeleeDamage>();
					if (this.verbProps.surpriseAttack != null && this.verbProps.surpriseAttack.extraMeleeDamages != null)
					{
						extraDamages = extraDamages.Concat(this.verbProps.surpriseAttack.extraMeleeDamages);
					}
					if (this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraMeleeDamage>())
					{
						extraDamages = extraDamages.Concat(this.tool.surpriseAttack.extraMeleeDamages);
					}
					enumerator = extraDamages.GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						extraDamage = enumerator.Current;
						amount = GenMath.RoundRandom((float)extraDamage.amount * base.GetDamageFactorFor(base.CasterPawn));
						DamageDef def = extraDamage.def;
						float amount2 = (float)amount;
						Thing caster = this.caster;
						extraDinfo = new DamageInfo(def, amount2, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null);
						extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
						extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
						extraDinfo.SetWeaponHediff(hediffDef);
						extraDinfo.SetAngle(direction);
						this.$current = extraDinfo;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IL_4BA:
				this.$PC = -1;
				return false;
			}

			DamageInfo IEnumerator<DamageInfo>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.DamageInfo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DamageInfo> IEnumerable<DamageInfo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Verb_MeleeAttackDamage.<DamageInfosToApply>c__Iterator0 <DamageInfosToApply>c__Iterator = new Verb_MeleeAttackDamage.<DamageInfosToApply>c__Iterator0();
				<DamageInfosToApply>c__Iterator.$this = this;
				<DamageInfosToApply>c__Iterator.target = target;
				return <DamageInfosToApply>c__Iterator;
			}
		}
	}
}
