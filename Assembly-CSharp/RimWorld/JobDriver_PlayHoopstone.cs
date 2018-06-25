using System;
using Verse;

namespace RimWorld
{
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		private const int StoneThrowInterval = 400;

		public JobDriver_PlayHoopstone()
		{
		}

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
