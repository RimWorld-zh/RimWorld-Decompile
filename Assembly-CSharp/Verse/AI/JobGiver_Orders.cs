using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ACC RID: 2764
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x06003D5E RID: 15710 RVA: 0x00205840 File Offset: 0x00203C40
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.Drafted)
			{
				result = new Job(JobDefOf.Wait_Combat, pawn.Position);
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
