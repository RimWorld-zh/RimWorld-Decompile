using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046F RID: 1135
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x060013FC RID: 5116 RVA: 0x000AE834 File Offset: 0x000ACC34
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef bloodPumpingSource = BodyPartTagDefOf.BloodPumpingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, bloodPumpingSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000AE86C File Offset: 0x000ACC6C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
