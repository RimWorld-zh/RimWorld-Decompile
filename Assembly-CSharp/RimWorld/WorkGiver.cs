using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200011A RID: 282
	public abstract class WorkGiver
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0003F0E0 File Offset: 0x0003D4E0
		public virtual bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return false;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0003F0F8 File Offset: 0x0003D4F8
		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0003F110 File Offset: 0x0003D510
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04000302 RID: 770
		public WorkGiverDef def;
	}
}
