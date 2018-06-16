using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000CD RID: 205
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x060004A8 RID: 1192 RVA: 0x00034E8C File Offset: 0x0003328C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			return jobGiver_StandAndBeSociallyActive;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00034EBC File Offset: 0x000332BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.StandAndBeSociallyActive)
			{
				expiryInterval = this.ticksRange.RandomInRange
			};
		}

		// Token: 0x0400029C RID: 668
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
