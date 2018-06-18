using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047C RID: 1148
	public class PawnCapacityWorker_Talking : PawnCapacityWorker
	{
		// Token: 0x06001420 RID: 5152 RVA: 0x000AED98 File Offset: 0x000AD198
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.TalkingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.TalkingPathway;
			float maximum = 1f;
			return num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x000AEE04 File Offset: 0x000AD204
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.TalkingSource);
		}
	}
}
