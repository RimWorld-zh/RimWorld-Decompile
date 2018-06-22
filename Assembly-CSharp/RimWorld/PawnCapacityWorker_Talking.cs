using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000478 RID: 1144
	public class PawnCapacityWorker_Talking : PawnCapacityWorker
	{
		// Token: 0x06001417 RID: 5143 RVA: 0x000AEDB0 File Offset: 0x000AD1B0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.TalkingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.TalkingPathway;
			float maximum = 1f;
			return num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000AEE1C File Offset: 0x000AD21C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.TalkingSource);
		}
	}
}
