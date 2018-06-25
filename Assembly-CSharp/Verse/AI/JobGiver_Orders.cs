using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ACA RID: 2762
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x06003D5D RID: 15709 RVA: 0x00205C90 File Offset: 0x00204090
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
