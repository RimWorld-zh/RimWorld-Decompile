using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000473 RID: 1139
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x06001405 RID: 5125 RVA: 0x000AE81C File Offset: 0x000ACC1C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef bloodPumpingSource = BodyPartTagDefOf.BloodPumpingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, bloodPumpingSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000AE854 File Offset: 0x000ACC54
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
