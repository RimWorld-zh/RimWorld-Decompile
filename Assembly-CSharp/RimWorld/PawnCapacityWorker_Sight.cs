using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000479 RID: 1145
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x06001418 RID: 5144 RVA: 0x000AEEA0 File Offset: 0x000AD2A0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x000AEED8 File Offset: 0x000AD2D8
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
