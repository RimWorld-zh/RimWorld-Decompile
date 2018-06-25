using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ACB RID: 2763
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x06003D5D RID: 15709 RVA: 0x00205F70 File Offset: 0x00204370
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
