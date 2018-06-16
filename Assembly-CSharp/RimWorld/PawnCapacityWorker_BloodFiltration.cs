using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000472 RID: 1138
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		// Token: 0x06001402 RID: 5122 RVA: 0x000AE71C File Offset: 0x000ACB1C
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

		// Token: 0x06001403 RID: 5123 RVA: 0x000AE7C4 File Offset: 0x000ACBC4
		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
