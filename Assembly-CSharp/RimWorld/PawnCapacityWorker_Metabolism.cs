using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000475 RID: 1141
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x000AEBD4 File Offset: 0x000ACFD4
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef metabolismSource = BodyPartTagDefOf.MetabolismSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, metabolismSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x000AEC0C File Offset: 0x000AD00C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
