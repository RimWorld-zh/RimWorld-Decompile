using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009FF RID: 2559
	public class TransitionAction_EndAllJobs : TransitionAction
	{
		// Token: 0x06003976 RID: 14710 RVA: 0x001E79E8 File Offset: 0x001E5DE8
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			}
		}
	}
}
