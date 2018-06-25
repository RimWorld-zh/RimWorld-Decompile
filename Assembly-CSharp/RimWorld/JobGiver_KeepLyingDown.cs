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
			Job result;
			if (pawn.GetPosture().Laying())
			{
				result = pawn.CurJob;
			}
			else
			{
				result = new Job(JobDefOf.LayDown, pawn.Position);
			}
			return result;
		}
	}
}
