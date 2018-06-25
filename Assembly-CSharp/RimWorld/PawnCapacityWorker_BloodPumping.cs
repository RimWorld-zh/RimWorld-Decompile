using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000471 RID: 1137
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x060013FF RID: 5119 RVA: 0x000AEB84 File Offset: 0x000ACF84
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef bloodPumpingSource = BodyPartTagDefOf.BloodPumpingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, bloodPumpingSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000AEBBC File Offset: 0x000ACFBC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
