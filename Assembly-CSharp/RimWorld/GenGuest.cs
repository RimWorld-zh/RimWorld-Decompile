using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004EB RID: 1259
	public static class GenGuest
	{
		// Token: 0x06001684 RID: 5764 RVA: 0x000C7E28 File Offset: 0x000C6228
		public static void PrisonerRelease(Pawn p)
		{
			if (p.ownership != null)
			{
				p.ownership.UnclaimAll();
			}
			if (p.Faction == Faction.OfPlayer)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasImprisoned, null);
				p.guest.SetGuestStatus(null, false);
			}
			else
			{
				p.guest.Released = true;
				IntVec3 c;
				if (RCellFinder.TryFindBestExitSpot(p, out c, TraverseMode.ByPawn))
				{
					Job job = new Job(JobDefOf.Goto, c);
					job.exitMapOnArrival = true;
					p.jobs.StartJob(job, JobCondition.None, null, false, true, null, null, false);
				}
			}
		}
	}
}
