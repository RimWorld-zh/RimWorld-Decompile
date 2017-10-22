using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		public IntRange ticksRange = new IntRange(300, 600);

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			return jobGiver_StandAndBeSociallyActive;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = new Job(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}
	}
}
