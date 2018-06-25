using System;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		public JobDriver_WaitDowned()
		{
		}

		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}

		[CompilerGenerated]
		private bool <DecorateWaitToil>m__0()
		{
			return !this.pawn.Downed;
		}
	}
}
