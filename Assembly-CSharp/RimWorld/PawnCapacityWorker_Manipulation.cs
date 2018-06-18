using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000478 RID: 1144
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x06001414 RID: 5140 RVA: 0x000AEB48 File Offset: 0x000ACF48
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			float num2 = PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors);
			return num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000AEB94 File Offset: 0x000ACF94
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
