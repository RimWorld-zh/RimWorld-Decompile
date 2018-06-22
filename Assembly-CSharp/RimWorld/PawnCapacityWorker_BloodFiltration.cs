using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046E RID: 1134
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		// Token: 0x060013F9 RID: 5113 RVA: 0x000AE740 File Offset: 0x000ACB40
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float result;
			if (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney))
			{
				BodyPartTagDef tag = BodyPartTagDefOf.BloodFiltrationKidney;
				float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
				tag = BodyPartTagDefOf.BloodFiltrationLiver;
				result = num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			}
			else
			{
				BodyPartTagDef tag = BodyPartTagDefOf.BloodFiltrationSource;
				result = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			}
			return result;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x000AE7E8 File Offset: 0x000ACBE8
		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
