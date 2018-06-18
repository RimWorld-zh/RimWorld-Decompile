using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC9 RID: 2761
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06003D57 RID: 15703 RVA: 0x002056AC File Offset: 0x00203AAC
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait_Downed);
		}
	}
}
