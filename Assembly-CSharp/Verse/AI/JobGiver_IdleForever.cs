using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC5 RID: 2757
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06003D52 RID: 15698 RVA: 0x002059D0 File Offset: 0x00203DD0
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait_Downed);
		}
	}
}
