using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
		public WorkGiver_BuildRoof()
		{
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

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
