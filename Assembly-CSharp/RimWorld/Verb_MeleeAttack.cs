using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Verb_MeleeAttack : Verb_MeleeAttackBase
	{
		private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
		{
			float damAmount2 = base.verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn, base.ownerEquipment);
			DamageDef damDef = base.verbProps.meleeDamageDef;
			BodyPartGroupDef bodyPartGroupDef = null;
			HediffDef hediffDef = null;
			damAmount2 = Random.Range((float)(damAmount2 * 0.800000011920929), (float)(damAmount2 * 1.2000000476837158));
			if (base.CasterIsPawn)
			{
				bodyPartGroupDef = base.LinkedBodyPartsGroup;
				if (damAmount2 >= 1.0)
				{
					if (base.ownerHediffComp != null)
					{
						hediffDef = base.ownerHediffComp.Def;
					}
				}
				else
				{
					damAmount2 = 1f;
					damDef = DamageDefOf.Blunt;
				}
			}
			ThingDef source = (base.ownerEquipment == null) ? base.CasterPawn.def : base.ownerEquipment.def;
			Vector3 direction = (target.Thing.Position - base.CasterPawn.Position).ToVector3();
			DamageDef def = damDef;
			int amount = GenMath.RoundRandom(damAmount2);
			Thing caster = base.caster;
			DamageInfo mainDinfo = new DamageInfo(def, amount, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown);
			mainDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			mainDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
			mainDinfo.SetWeaponHediff(hediffDef);
			mainDinfo.SetAngle(direction);
			yield return mainDinfo;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult result = DamageWorker.DamageResult.MakeNew();
			foreach (DamageInfo item in this.DamageInfosToApply(target))
			{
				if (!target.ThingDestroyed)
				{
					result = target.Thing.TakeDamage(item);
					continue;
				}
				break;
			}
			return result;
		}
	}
}
