using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D1 RID: 209
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x000351D4 File Offset: 0x000335D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.LayDown, pawn.Position)
			{
				forceSleep = true
			};
		}
	}
}
