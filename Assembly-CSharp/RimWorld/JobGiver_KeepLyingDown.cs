using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E5 RID: 229
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x00037024 File Offset: 0x00035424
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.GetPosture().Laying())
			{
				result = pawn.CurJob;
			}
			else
			{
				result = new Job(JobDefOf.LayDown, pawn.Position);
			}
			return result;
		}
	}
}
