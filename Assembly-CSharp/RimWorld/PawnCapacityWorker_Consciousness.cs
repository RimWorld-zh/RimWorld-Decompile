using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000473 RID: 1139
	public class PawnCapacityWorker_Consciousness : PawnCapacityWorker
	{
		// Token: 0x06001406 RID: 5126 RVA: 0x000AEA90 File Offset: 0x000ACE90
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef consciousnessSource = BodyPartTagDefOf.ConsciousnessSource;
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, consciousnessSource, float.MaxValue, default(FloatRange), impactors);
			float num2 = Mathf.Clamp(GenMath.LerpDouble(0.1f, 1f, 0f, 0.4f, diffSet.PainTotal), 0f, 0.4f);
			if ((double)num2 >= 0.01)
			{
				num -= num2;
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorPain());
				}
			}
			num = Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodPumping, impactors), 1f), 0.2f);
			num = Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Breathing, impactors), 1f), 0.2f);
			return Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodFiltration, impactors), 1f), 0.1f);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000AEB94 File Offset: 0x000ACF94
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ConsciousnessSource);
		}
	}
}
