using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000109 RID: 265
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x0003BEE8 File Offset: 0x0003A2E8
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
				IntVec3 c2;
				if (ownedRoom == null)
				{
					result = null;
				}
				else if (!(from c in ownedRoom.Cells
				where c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false)
				select c).TryRandomElement(out c2))
				{
					result = null;
				}
				else
				{
					result = new Job(this.def.jobDef, c2);
				}
			}
			return result;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0003BF80 File Offset: 0x0003A380
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return new Job(this.def.jobDef, pawn.CurrentBed());
		}
	}
}
