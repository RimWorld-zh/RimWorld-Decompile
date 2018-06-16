using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000130 RID: 304
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00041E1C File Offset: 0x0004021C
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00041E34 File Offset: 0x00040234
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00041E60 File Offset: 0x00040260
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00041E78 File Offset: 0x00040278
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			bool result;
			if (!pawn.Map.areaManager.NoRoof[c])
			{
				result = false;
			}
			else if (!c.Roofed(pawn.Map))
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
				ReservationLayerDef ceiling = ReservationLayerDefOf.Ceiling;
				result = pawn.CanReserve(target, 1, -1, ceiling, forced);
			}
			return result;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00041F08 File Offset: 0x00040308
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.RemoveRoof, c, c);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00041F34 File Offset: 0x00040334
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			IntVec3 cell = t.Cell;
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = cell + GenAdj.AdjacentCells[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.holdsRoof)
					{
						return -60f;
					}
					if (c.Roofed(pawn.Map))
					{
						num++;
					}
				}
			}
			return (float)(-(float)Mathf.Min(num, 3));
		}
	}
}
