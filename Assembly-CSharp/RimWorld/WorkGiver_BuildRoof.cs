using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000124 RID: 292
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0004030C File Offset: 0x0003E70C
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00040338 File Offset: 0x0003E738
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00040350 File Offset: 0x0003E750
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00040368 File Offset: 0x0003E768
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			bool result;
			if (!pawn.Map.areaManager.BuildRoof[c])
			{
				result = false;
			}
			else if (c.Roofed(pawn.Map))
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
				if (!pawn.CanReserve(target, 1, -1, ceiling, forced))
				{
					result = false;
				}
				else if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn) && this.BuildingToTouchToBeAbleToBuildRoof(c, pawn) == null)
				{
					result = false;
				}
				else if (!RoofCollapseUtility.WithinRangeOfRoofHolder(c, pawn.Map, false))
				{
					result = false;
				}
				else if (!RoofCollapseUtility.ConnectedToRoofHolder(c, pawn.Map, true))
				{
					result = false;
				}
				else
				{
					Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
					result = (thing == null || RoofUtility.CanHandleBlockingThing(thing, pawn, forced));
				}
			}
			return result;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0004047C File Offset: 0x0003E87C
		private Building BuildingToTouchToBeAbleToBuildRoof(IntVec3 c, Pawn pawn)
		{
			Building result;
			if (c.Standable(pawn.Map))
			{
				result = null;
			}
			else
			{
				Building edifice = c.GetEdifice(pawn.Map);
				if (edifice == null)
				{
					result = null;
				}
				else if (!pawn.CanReach(edifice, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
				{
					result = null;
				}
				else
				{
					result = edifice;
				}
			}
			return result;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000404E8 File Offset: 0x0003E8E8
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			LocalTargetInfo targetB = c;
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			Job result;
			if (thing != null)
			{
				result = RoofUtility.HandleBlockingThingJob(thing, pawn, forced);
			}
			else
			{
				if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
				{
					targetB = this.BuildingToTouchToBeAbleToBuildRoof(c, pawn);
				}
				result = new Job(JobDefOf.BuildRoof, c, targetB);
			}
			return result;
		}
	}
}
