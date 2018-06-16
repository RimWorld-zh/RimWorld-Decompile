using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047B RID: 1147
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x0600141D RID: 5149 RVA: 0x000AED2C File Offset: 0x000AD12C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000AED64 File Offset: 0x000AD164
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
