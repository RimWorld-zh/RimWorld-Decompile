using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000471 RID: 1137
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x000AE984 File Offset: 0x000ACD84
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef bloodPumpingSource = BodyPartTagDefOf.BloodPumpingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, bloodPumpingSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000AE9BC File Offset: 0x000ACDBC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
