using System.Collections.Generic;

namespace Verse
{
	public class DamageWorker_Stab : DamageWorker_AddInjury
	{
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, (Rand.Value < base.def.stabChanceOfForcedInternal) ? BodyPartDepth.Inside : dinfo.Depth);
		}

		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, ref DamageResult result)
		{
			totalDamage = base.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			List<BodyPartRecord> list = new List<BodyPartRecord>();
			BodyPartRecord bodyPartRecord = dinfo.HitPart;
			while (bodyPartRecord != null)
			{
				list.Add(bodyPartRecord);
				if (bodyPartRecord.depth != BodyPartDepth.Outside)
				{
					bodyPartRecord = bodyPartRecord.parent;
					continue;
				}
				break;
			}
			float totalDamage2 = (float)(totalDamage * (1.0 + base.def.stabPierceBonus) / ((float)list.Count + base.def.stabPierceBonus));
			for (int i = 0; i < list.Count; i++)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(list[i]);
				base.FinalizeAndAddInjury(pawn, totalDamage2, dinfo2, ref result);
			}
		}
	}
}
