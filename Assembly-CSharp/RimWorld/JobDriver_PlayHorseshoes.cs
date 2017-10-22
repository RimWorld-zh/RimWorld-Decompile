using Verse;

namespace RimWorld
{
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		private const int HorseshoeThrowInterval = 400;

		protected override void WatchTickAction()
		{
			if (base.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowHorseshoe(base.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}
	}
}
