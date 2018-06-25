using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D7 RID: 2519
	public class Verb_MeleeAttackDamage : Verb_MeleeAttack
	{
		// Token: 0x06003879 RID: 14457 RVA: 0x001E2A84 File Offset: 0x001E0E84
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

		// Token: 0x0600387A RID: 14458 RVA: 0x001E2AB8 File Offset: 0x001E0EB8
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
	}
}
