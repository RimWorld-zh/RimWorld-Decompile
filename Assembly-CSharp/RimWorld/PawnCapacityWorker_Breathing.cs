using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000470 RID: 1136
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x060013FF RID: 5119 RVA: 0x000AE894 File Offset: 0x000ACC94
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef tag = BodyPartTagDefOf.BreathingSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.BreathingPathway;
			float maximum = 1f;
			float num2 = num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors);
			tag = BodyPartTagDefOf.BreathingSourceCage;
			maximum = 1f;
			return num2 * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, maximum, default(FloatRange), impactors);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000AE918 File Offset: 0x000ACD18
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
