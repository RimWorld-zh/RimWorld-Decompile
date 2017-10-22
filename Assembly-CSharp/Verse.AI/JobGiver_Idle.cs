using RimWorld;

namespace Verse.AI
{
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		public int ticks = 50;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = new Job(JobDefOf.Wait);
			job.expiryInterval = this.ticks;
			return job;
		}
	}
}
