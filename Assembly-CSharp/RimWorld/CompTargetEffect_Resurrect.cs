using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075B RID: 1883
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x06002997 RID: 10647 RVA: 0x00161488 File Offset: 0x0015F888
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
