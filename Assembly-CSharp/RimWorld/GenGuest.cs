using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004ED RID: 1261
	public static class GenGuest
	{
		// Token: 0x06001689 RID: 5769 RVA: 0x000C7A90 File Offset: 0x000C5E90
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
