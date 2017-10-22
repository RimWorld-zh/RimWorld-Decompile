using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (user.IsColonistPlayerControlled && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				Job job = new Job(JobDefOf.Resurrect, target, (Thing)base.parent);
				job.count = 1;
				user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}
		}
	}
}
