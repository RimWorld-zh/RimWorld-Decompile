using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000056 RID: 86
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		// Token: 0x040001F4 RID: 500
		private const int StoneThrowInterval = 400;

		// Token: 0x06000297 RID: 663 RVA: 0x0001C4B0 File Offset: 0x0001A8B0
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}
	}
}
