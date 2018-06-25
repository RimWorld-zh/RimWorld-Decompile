using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B59 RID: 2905
	public class PawnCapacityWorker
	{
		// Token: 0x06003F7F RID: 16255 RVA: 0x000AEA00 File Offset: 0x000ACE00
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x000AEA1C File Offset: 0x000ACE1C
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x000AEA34 File Offset: 0x000ACE34
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
