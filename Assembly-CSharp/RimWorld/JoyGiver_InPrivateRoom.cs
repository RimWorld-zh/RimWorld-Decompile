using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		public JoyGiver_InPrivateRoom()
		{
		}

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

		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return new Job(this.def.jobDef, pawn.CurrentBed());
		}

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return c.Standable(this.pawn.Map) && !c.IsForbidden(this.pawn) && this.pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false);
			}
		}
	}
}
