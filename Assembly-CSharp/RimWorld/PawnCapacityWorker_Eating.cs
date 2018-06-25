using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000474 RID: 1140
	public class PawnCapacityWorker_Eating : PawnCapacityWorker
	{
		// Token: 0x06001408 RID: 5128 RVA: 0x000AEDBC File Offset: 0x000AD1BC
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.EatingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.EatingPathway;
			float maximum = 1f;
			return num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000AEE28 File Offset: 0x000AD228
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.EatingSource);
		}
	}
}
