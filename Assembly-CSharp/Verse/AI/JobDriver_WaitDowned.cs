using System;

namespace Verse.AI
{
	// Token: 0x02000A3A RID: 2618
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06003A14 RID: 14868 RVA: 0x001EB4C4 File Offset: 0x001E98C4
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
