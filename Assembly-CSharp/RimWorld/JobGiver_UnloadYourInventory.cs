using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F1 RID: 241
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x00038790 File Offset: 0x00036B90
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.inventory.UnloadEverything)
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.UnloadYourInventory);
			}
			return result;
		}
	}
}
