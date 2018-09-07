using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		public JobGiver_KeepLyingDown()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetPosture().Laying())
			{
				return pawn.CurJob;
			}
			return new Job(JobDefOf.LayDown, pawn.Position);
		}
	}
}
