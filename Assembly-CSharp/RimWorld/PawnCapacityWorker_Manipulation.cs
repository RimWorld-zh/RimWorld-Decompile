using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000476 RID: 1142
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x000AEEB0 File Offset: 0x000AD2B0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			float num2 = PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors);
			return num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x000AEEFC File Offset: 0x000AD2FC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
