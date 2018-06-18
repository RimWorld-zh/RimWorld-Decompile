using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000476 RID: 1142
	public class PawnCapacityWorker_Eating : PawnCapacityWorker
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x000AEA54 File Offset: 0x000ACE54
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.EatingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.EatingPathway;
			float maximum = 1f;
			return num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x000AEAC0 File Offset: 0x000ACEC0
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.EatingSource);
		}
	}
}
