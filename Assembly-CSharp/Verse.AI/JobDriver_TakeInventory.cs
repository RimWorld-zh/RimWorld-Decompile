using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_TakeInventory : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			Toil gotoThing = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1B3)/*Error near IL_0055: stateMachine*/)._003C_003Ef__this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator1B3)/*Error near IL_0055: stateMachine*/)._003C_003Ef__this.TargetThingA, PathEndMode.ClosestTouch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_Haul.TakeToInventory(TargetIndex.A, base.CurJob.count);
		}
	}
}
