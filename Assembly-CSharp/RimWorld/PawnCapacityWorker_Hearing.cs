using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000473 RID: 1139
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x06001408 RID: 5128 RVA: 0x000AEB00 File Offset: 0x000ACF00
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef hearingSource = BodyPartTagDefOf.HearingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, hearingSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000AEB38 File Offset: 0x000ACF38
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
