using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B5A RID: 2906
	public class PawnCapacityWorker
	{
		// Token: 0x06003F7C RID: 16252 RVA: 0x000AE698 File Offset: 0x000ACA98
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x000AE6B4 File Offset: 0x000ACAB4
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x000AE6CC File Offset: 0x000ACACC
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
