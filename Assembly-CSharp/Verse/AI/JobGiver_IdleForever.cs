using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC9 RID: 2761
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06003D55 RID: 15701 RVA: 0x002055D8 File Offset: 0x002039D8
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait_Downed);
		}
	}
}
