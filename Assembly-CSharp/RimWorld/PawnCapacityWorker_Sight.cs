using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000479 RID: 1145
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x06001417 RID: 5143 RVA: 0x000AF0A0 File Offset: 0x000AD4A0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000AF0D8 File Offset: 0x000AD4D8
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
