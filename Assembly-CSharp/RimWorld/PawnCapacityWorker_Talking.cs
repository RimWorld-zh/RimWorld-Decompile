using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047A RID: 1146
	public class PawnCapacityWorker_Talking : PawnCapacityWorker
	{
		// Token: 0x0600141B RID: 5147 RVA: 0x000AEF00 File Offset: 0x000AD300
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.TalkingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.TalkingPathway;
			float maximum = 1f;
			return num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x000AEF6C File Offset: 0x000AD36C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.TalkingSource);
		}
	}
}
