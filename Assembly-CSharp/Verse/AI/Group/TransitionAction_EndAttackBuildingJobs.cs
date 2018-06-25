using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A02 RID: 2562
	public class TransitionAction_EndAttackBuildingJobs : TransitionAction
	{
		// Token: 0x0600397C RID: 14716 RVA: 0x001E7B84 File Offset: 0x001E5F84
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null && pawn.jobs.curJob.def == JobDefOf.AttackMelee && pawn.jobs.curJob.targetA.Thing is Building)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			}
		}
	}
}
