using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000472 RID: 1138
	public class PawnCapacityWorker_Eating : PawnCapacityWorker
	{
		// Token: 0x06001405 RID: 5125 RVA: 0x000AEA6C File Offset: 0x000ACE6C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.EatingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.EatingPathway;
			float maximum = 1f;
			return num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000AEAD8 File Offset: 0x000ACED8
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.EatingSource);
		}
	}
}
