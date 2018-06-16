using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200012D RID: 301
	public class WorkGiver_ConstructSmoothWall : WorkGiver_Scanner
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0004174C File Offset: 0x0003FB4C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00041764 File Offset: 0x0003FB64
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.SmoothWall))
			{
				yield return des.target.Cell;
			}
			yield break;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00041790 File Offset: 0x0003FB90
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			bool result;
			if (c.IsForbidden(pawn) || pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) == null)
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(pawn.Map);
				if (edifice == null || !edifice.def.IsSmoothable)
				{
					Log.ErrorOnce("Failed to find valid edifice when trying to smooth a wall", 58988176, false);
					pawn.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
					result = false;
				}
				else
				{
					LocalTargetInfo target = edifice;
					if (pawn.CanReserve(target, 1, -1, null, forced))
					{
						target = c;
						if (pawn.CanReserve(target, 1, -1, null, forced))
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00041868 File Offset: 0x0003FC68
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.SmoothWall, c.GetEdifice(pawn.Map));
		}
	}
}
