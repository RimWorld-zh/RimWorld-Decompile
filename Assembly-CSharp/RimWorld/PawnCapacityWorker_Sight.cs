using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000477 RID: 1143
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x06001414 RID: 5140 RVA: 0x000AED50 File Offset: 0x000AD150
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000AED88 File Offset: 0x000AD188
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
