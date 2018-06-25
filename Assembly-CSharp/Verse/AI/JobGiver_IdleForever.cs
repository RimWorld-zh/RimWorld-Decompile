using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC7 RID: 2759
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06003D56 RID: 15702 RVA: 0x00205AFC File Offset: 0x00203EFC
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait_Downed);
		}
	}
}
