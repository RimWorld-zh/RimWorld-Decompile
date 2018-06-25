using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E5 RID: 229
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x0003702C File Offset: 0x0003542C
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
