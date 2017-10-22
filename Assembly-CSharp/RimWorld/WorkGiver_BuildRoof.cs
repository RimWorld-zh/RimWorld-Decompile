using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
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

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c)
		{
			bool result;
			if (!((Area)pawn.Map.areaManager.BuildRoof)[c])
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
				result = ((byte)(pawn.CanReserve(target, 1, -1, ceiling, false) ? ((pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn) || this.BuildingToTouchToBeAbleToBuildRoof(c, pawn) != null) ? (RoofCollapseUtility.WithinRangeOfRoofHolder(c, pawn.Map) ? (RoofCollapseUtility.ConnectedToRoofHolder(c, pawn.Map, true) ? 1 : 0) : 0) : 0) : 0) != 0);
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
				result = ((edifice != null) ? (pawn.CanReach((Thing)edifice, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn) ? edifice : null) : null);
			}
			return result;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c)
		{
			LocalTargetInfo targetB = c;
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
			{
				targetB = (Thing)this.BuildingToTouchToBeAbleToBuildRoof(c, pawn);
			}
			return new Job(JobDefOf.BuildRoof, c, targetB);
		}
	}
}
