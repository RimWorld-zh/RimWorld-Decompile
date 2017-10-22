using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_Party : LordToil
	{
		private const int TicksPerPartyPulse = 600;

		private IntVec3 spot;

		private LordToilData_Party Data
		{
			get
			{
				return (LordToilData_Party)base.data;
			}
		}

		public LordToil_Party(IntVec3 spot)
		{
			this.spot = spot;
			base.data = new LordToilData_Party();
			this.Data.ticksToNextPulse = 600;
		}

		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return DutyDefOf.Party.hook;
		}

		public override void UpdateAllDuties()
		{
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				base.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Party, this.spot, -1f);
			}
		}

		public override void LordToilTick()
		{
			if (--this.Data.ticksToNextPulse <= 0)
			{
				this.Data.ticksToNextPulse = 600;
				List<Pawn> ownedPawns = base.lord.ownedPawns;
				for (int i = 0; i < ownedPawns.Count; i++)
				{
					if (PartyUtility.InPartyArea(ownedPawns[i].Position, this.spot, base.Map))
					{
						ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedParty, null);
					}
				}
			}
		}
	}
}
