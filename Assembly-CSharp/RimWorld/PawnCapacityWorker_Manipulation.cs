using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000474 RID: 1140
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x0600140B RID: 5131 RVA: 0x000AEB60 File Offset: 0x000ACF60
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			float num2 = PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors);
			return num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x000AEBAC File Offset: 0x000ACFAC
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
