using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Ignite : JobDriver
	{
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnBurningImmobile(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
