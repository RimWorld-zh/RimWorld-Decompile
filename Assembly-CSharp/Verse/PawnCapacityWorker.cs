using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B5A RID: 2906
	public class PawnCapacityWorker
	{
		// Token: 0x06003F7A RID: 16250 RVA: 0x000AE68C File Offset: 0x000ACA8C
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x000AE6A8 File Offset: 0x000ACAA8
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x000AE6C0 File Offset: 0x000ACAC0
		protected float CalculateCapacityAndRecord(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			float level = diffSet.pawn.health.capacities.GetLevel(capacity);
			if (impactors != null && level != 1f)
			{
				impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
				{
					capacity = capacity
				});
			}
			return level;
		}
	}
}
