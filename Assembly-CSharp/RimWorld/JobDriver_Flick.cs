using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Flick : JobDriver
	{
		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003a: stateMachine*/;
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(delegate
			{
				Designation designation = _003CMakeNewToils_003Ec__Iterator._0024this.Map.designationManager.DesignationOn(_003CMakeNewToils_003Ec__Iterator._0024this.TargetThingA, DesignationDefOf.Flick);
				if (designation != null)
				{
					return false;
				}
				return true;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
