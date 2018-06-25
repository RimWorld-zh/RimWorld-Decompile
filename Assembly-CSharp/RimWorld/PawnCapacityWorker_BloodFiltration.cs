using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000470 RID: 1136
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		// Token: 0x060013FC RID: 5116 RVA: 0x000AEA90 File Offset: 0x000ACE90
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

		// Token: 0x060013FD RID: 5117 RVA: 0x000AEB38 File Offset: 0x000ACF38
		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
