using System;

namespace Verse.AI
{
	// Token: 0x02000A38 RID: 2616
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x001EB830 File Offset: 0x001E9C30
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
