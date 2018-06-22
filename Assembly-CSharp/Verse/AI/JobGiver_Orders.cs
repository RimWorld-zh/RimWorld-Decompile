using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC8 RID: 2760
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x06003D59 RID: 15705 RVA: 0x00205B64 File Offset: 0x00203F64
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
