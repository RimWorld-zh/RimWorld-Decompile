using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class DamageWorker_Cut : DamageWorker_AddInjury
	{
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, ref DamageResult result)
		{
			if (dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
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
				float num = (float)((float)(list.Count - 1) + 0.5);
				for (int i = 0; i < list.Count; i++)
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetHitPart(list[i]);
					base.FinalizeAndAddInjury(pawn, (float)(totalDamage / num * ((i != 0) ? 1.0 : 0.5)), dinfo2, ref result);
				}
			}
			else
			{
				int num2 = (base.def.cutExtraTargetsCurve != null) ? GenMath.RoundRandom(base.def.cutExtraTargetsCurve.Evaluate(Rand.Value)) : 0;
				List<BodyPartRecord> list2 = null;
				if (num2 != 0)
				{
					IEnumerable<BodyPartRecord> lhs = dinfo.HitPart.GetDirectChildParts();
					if (dinfo.HitPart.parent != null)
					{
						lhs = lhs.Concat(dinfo.HitPart.parent);
						lhs = lhs.Concat(dinfo.HitPart.parent.GetDirectChildParts());
					}
					list2 = lhs.Except(dinfo.HitPart).InRandomOrder(null).Take(num2)
						.ToList();
				}
				else
				{
					list2 = new List<BodyPartRecord>();
				}
				list2.Add(dinfo.HitPart);
				float num3 = (float)(totalDamage * (1.0 + base.def.cutCleaveBonus) / ((float)list2.Count + base.def.cutCleaveBonus));
				if (num2 == 0)
				{
					num3 = base.ReduceDamageToPreserveOutsideParts(num3, dinfo, pawn);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					DamageInfo dinfo3 = dinfo;
					dinfo3.SetHitPart(list2[j]);
					base.FinalizeAndAddInjury(pawn, num3, dinfo3, ref result);
				}
			}
		}
	}
}
