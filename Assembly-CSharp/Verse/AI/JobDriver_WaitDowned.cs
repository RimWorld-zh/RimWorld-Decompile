using System;

namespace Verse.AI
{
	// Token: 0x02000A36 RID: 2614
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06003A0E RID: 14862 RVA: 0x001EB704 File Offset: 0x001E9B04
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
