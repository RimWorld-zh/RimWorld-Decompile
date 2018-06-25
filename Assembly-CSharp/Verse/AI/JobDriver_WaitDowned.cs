using System;

namespace Verse.AI
{
	// Token: 0x02000A39 RID: 2617
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06003A13 RID: 14867 RVA: 0x001EBB5C File Offset: 0x001E9F5C
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
