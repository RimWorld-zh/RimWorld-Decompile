using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000CF8 RID: 3320
	public class DamageWorker_Scratch : DamageWorker_AddInjury
	{
		// Token: 0x0600492C RID: 18732 RVA: 0x00267578 File Offset: 0x00265978
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x002675AC File Offset: 0x002659AC
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				List<BodyPartRecord> list = new List<BodyPartRecord>();
				for (BodyPartRecord bodyPartRecord = dinfo.HitPart; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
				{
					list.Add(bodyPartRecord);
					if (bodyPartRecord.depth == BodyPartDepth.Outside)
					{
						break;
					}
				}
				float num = (float)list.Count;
				for (int i = 0; i < list.Count; i++)
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetHitPart(list[i]);
					base.FinalizeAndAddInjury(pawn, totalDamage / num, dinfo2, result);
				}
			}
			else
			{
				IEnumerable<BodyPartRecord> enumerable = dinfo.HitPart.GetDirectChildParts();
				if (dinfo.HitPart.parent != null)
				{
					enumerable = enumerable.Concat(dinfo.HitPart.parent);
					if (dinfo.HitPart.parent.parent != null)
					{
						enumerable = enumerable.Concat(dinfo.HitPart.parent.GetDirectChildParts());
					}
				}
				enumerable = from target in enumerable
				where target != dinfo.HitPart && !target.def.conceptual && target.depth == BodyPartDepth.Outside && !pawn.health.hediffSet.PartIsMissing(target)
				select target;
				BodyPartRecord bodyPartRecord2 = enumerable.RandomElementWithFallback(null);
				if (bodyPartRecord2 == null)
				{
					base.FinalizeAndAddInjury(pawn, base.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn), dinfo, result);
				}
				else
				{
					base.FinalizeAndAddInjury(pawn, base.ReduceDamageToPreserveOutsideParts(totalDamage * this.def.scratchSplitPercentage, dinfo, pawn), dinfo, result);
					DamageInfo dinfo3 = dinfo;
					dinfo3.SetHitPart(bodyPartRecord2);
					base.FinalizeAndAddInjury(pawn, base.ReduceDamageToPreserveOutsideParts(totalDamage * this.def.scratchSplitPercentage, dinfo3, pawn), dinfo3, result);
				}
			}
		}
	}
}
