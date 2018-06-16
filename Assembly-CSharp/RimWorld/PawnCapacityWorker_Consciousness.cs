using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000475 RID: 1141
	public class PawnCapacityWorker_Consciousness : PawnCapacityWorker
	{
		// Token: 0x0600140B RID: 5131 RVA: 0x000AE91C File Offset: 0x000ACD1C
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

		// Token: 0x0600140C RID: 5132 RVA: 0x000AEA20 File Offset: 0x000ACE20
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ConsciousnessSource);
		}
	}
}
