using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D31 RID: 3377
	public static class HediffGiveUtility
	{
		// Token: 0x06004A7A RID: 19066 RVA: 0x0026DAF8 File Offset: 0x0026BEF8
		public static bool TryApply(Pawn pawn, HediffDef hediff, List<BodyPartDef> partsToAffect, bool canAffectAnyLivePart = false, int countToAffect = 1, List<Hediff> outAddedHediffs = null)
		{
			bool result;
			if (canAffectAnyLivePart || partsToAffect != null)
			{
				bool flag = false;
				for (int i = 0; i < countToAffect; i++)
				{
					IEnumerable<BodyPartRecord> source = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null);
					if (partsToAffect != null)
					{
						source = from p in source
						where partsToAffect.Contains(p.def)
						select p;
					}
					if (canAffectAnyLivePart)
					{
						source = from p in source
						where p.def.alive
						select p;
					}
					source = from p in source
					where !pawn.health.hediffSet.HasHediff(hediff, p, false) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)
					select p;
					if (!source.Any<BodyPartRecord>())
					{
						break;
					}
					BodyPartRecord partRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					Hediff hediff2 = HediffMaker.MakeHediff(hediff, pawn, partRecord);
					pawn.health.AddHediff(hediff2, null, null, null);
					if (outAddedHediffs != null)
					{
						outAddedHediffs.Add(hediff2);
					}
					flag = true;
				}
				result = flag;
			}
			else if (!pawn.health.hediffSet.HasHediff(hediff, false))
			{
				Hediff hediff3 = HediffMaker.MakeHediff(hediff, pawn, null);
				pawn.health.AddHediff(hediff3, null, null, null);
				if (outAddedHediffs != null)
				{
					outAddedHediffs.Add(hediff3);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
