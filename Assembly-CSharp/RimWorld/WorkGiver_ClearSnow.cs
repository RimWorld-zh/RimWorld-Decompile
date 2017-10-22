using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ClearSnow : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.ActiveCells;
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.TrueCount == 0;
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c)
		{
			if (pawn.Map.snowGrid.GetDepth(c) < 0.20000000298023224)
			{
				return false;
			}
			if (!pawn.CanReserveAndReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				return false;
			}
			return true;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c)
		{
			return new Job(JobDefOf.ClearSnow, c);
		}
	}
}
