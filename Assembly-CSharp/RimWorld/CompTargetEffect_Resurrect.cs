using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000759 RID: 1881
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x06002995 RID: 10645 RVA: 0x00161AA4 File Offset: 0x0015FEA4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (user.IsColonistPlayerControlled)
			{
				if (user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
				{
					Job job = new Job(JobDefOf.Resurrect, target, this.parent);
					job.count = 1;
					user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}
			}
		}
	}
}
