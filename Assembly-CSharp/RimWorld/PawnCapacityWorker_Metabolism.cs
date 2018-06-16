using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000479 RID: 1145
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x06001417 RID: 5143 RVA: 0x000AEBB0 File Offset: 0x000ACFB0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef metabolismSource = BodyPartTagDefOf.MetabolismSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, metabolismSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000AEBE8 File Offset: 0x000ACFE8
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
