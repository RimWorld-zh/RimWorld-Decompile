using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000477 RID: 1143
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x06001412 RID: 5138 RVA: 0x000AED24 File Offset: 0x000AD124
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef metabolismSource = BodyPartTagDefOf.MetabolismSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, metabolismSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x000AED5C File Offset: 0x000AD15C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
