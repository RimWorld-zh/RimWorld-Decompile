using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CF7 RID: 3319
	public class DamageWorker_Stab : DamageWorker_AddInjury
	{
		// Token: 0x0600492C RID: 18732 RVA: 0x00267750 File Offset: 0x00265B50
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, (Rand.Value >= this.def.stabChanceOfForcedInternal) ? dinfo.Depth : BodyPartDepth.Inside);
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x002677A8 File Offset: 0x00265BA8
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = base.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			List<BodyPartRecord> list = new List<BodyPartRecord>();
			for (BodyPartRecord bodyPartRecord = dinfo.HitPart; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				list.Add(bodyPartRecord);
				if (bodyPartRecord.depth == BodyPartDepth.Outside)
				{
					break;
				}
			}
			float totalDamage2 = totalDamage * (1f + this.def.stabPierceBonus) / ((float)list.Count + this.def.stabPierceBonus);
			for (int i = 0; i < list.Count; i++)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(list[i]);
				base.FinalizeAndAddInjury(pawn, totalDamage2, dinfo2, result);
			}
		}
	}
}
