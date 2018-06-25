using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200013B RID: 315
	public class WorkGiver_ClearSnow : WorkGiver_Scanner
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x00042D40 File Offset: 0x00041140
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.ActiveCells;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00042D6C File Offset: 0x0004116C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00042D84 File Offset: 0x00041184
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowClear.TrueCount == 0;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00042DB4 File Offset: 0x000411B4
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			bool result;
			if (pawn.Map.snowGrid.GetDepth(c) < 0.2f)
			{
				result = false;
			}
			else if (c.IsForbidden(pawn))
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = c;
				result = pawn.CanReserve(target, 1, -1, null, forced);
			}
			return result;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00042E24 File Offset: 0x00041224
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.ClearSnow, c);
		}
	}
}
