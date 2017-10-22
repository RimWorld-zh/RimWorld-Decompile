using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class DamageWorker_Blunt : DamageWorker_AddInjury
	{
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}

		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, ref DamageResult result)
		{
			bool flag = Rand.Chance(base.def.bluntInnerHitFrequency);
			float num = (float)((!flag) ? 0.0 : base.def.bluntInnerHitConverted.RandomInRange);
			float num2 = (float)(totalDamage * (1.0 - num));
			while (true)
			{
				num2 -= base.FinalizeAndAddInjury(pawn, num2, dinfo, ref result);
				if (pawn.health.hediffSet.PartIsMissing(dinfo.HitPart) && !(num2 <= 1.0))
				{
					BodyPartRecord parent = dinfo.HitPart.parent;
					if (parent != null)
					{
						dinfo.SetHitPart(parent);
						continue;
					}
				}
				break;
			}
			if (flag && !dinfo.HitPart.def.IsSolid(dinfo.HitPart, pawn.health.hediffSet.hediffs) && dinfo.HitPart.depth == BodyPartDepth.Outside)
			{
				IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where x.parent == dinfo.HitPart && x.def.IsSolid(x, pawn.health.hediffSet.hediffs) && x.depth == BodyPartDepth.Inside
				select x;
				BodyPartRecord hitPart = default(BodyPartRecord);
				if (source.TryRandomElementByWeight<BodyPartRecord>((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.coverageAbs), out hitPart))
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetHitPart(hitPart);
					base.FinalizeAndAddInjury(pawn, totalDamage * num + totalDamage * base.def.bluntInnerHitAdded.RandomInRange, dinfo2, ref result);
				}
			}
		}
	}
}
