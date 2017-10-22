using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_ConstructAffectFloor : WorkGiver_Scanner
	{
		protected abstract DesignationDef DesDef
		{
			get;
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.DesDef))
				{
					yield return item.target.Cell;
				}
			}
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c)
		{
			if (pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null)
			{
				ReservationLayerDef floor = ReservationLayerDefOf.Floor;
				if (!pawn.CanReserveAndReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, floor, false))
					goto IL_003e;
				return true;
			}
			goto IL_003e;
			IL_003e:
			return false;
		}
	}
}
