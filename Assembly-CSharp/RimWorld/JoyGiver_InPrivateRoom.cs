using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		public override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.ownership == null)
			{
				result = null;
			}
			else
			{
				Room ownedRoom = pawn.ownership.OwnedRoom;
				IntVec3 c2 = default(IntVec3);
				result = ((ownedRoom != null) ? ((from c in ownedRoom.Cells
				where c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false)
				select c).TryRandomElement<IntVec3>(out c2) ? new Job(base.def.jobDef, c2) : null) : null);
			}
			return result;
		}
	}
}
