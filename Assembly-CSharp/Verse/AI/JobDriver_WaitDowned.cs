using System;

namespace Verse.AI
{
	// Token: 0x02000A3A RID: 2618
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x001EB3F0 File Offset: 0x001E97F0
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
