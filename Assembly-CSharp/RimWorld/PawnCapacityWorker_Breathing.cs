using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000474 RID: 1140
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x06001408 RID: 5128 RVA: 0x000AE87C File Offset: 0x000ACC7C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.BreathingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.BreathingPathway;
			float maximum = 1f;
			float num2 = num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.BreathingSourceCage;
			maximum = 1f;
			return num2 * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000AE900 File Offset: 0x000ACD00
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
