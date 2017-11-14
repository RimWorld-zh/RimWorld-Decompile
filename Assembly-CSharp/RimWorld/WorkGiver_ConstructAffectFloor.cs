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
				using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(this.DesDef).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Designation des = enumerator.Current;
						yield return des.target.Cell;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_00f2:
			/*Error near IL_00f3: Unexpected return in MoveNext()*/;
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c)
		{
			if (!c.IsForbidden(pawn) && pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null)
			{
				LocalTargetInfo target = c;
				ReservationLayerDef floor = ReservationLayerDefOf.Floor;
				if (!pawn.CanReserve(target, 1, -1, floor, false))
					goto IL_0047;
				return true;
			}
			goto IL_0047;
			IL_0047:
			return false;
		}
	}
}
