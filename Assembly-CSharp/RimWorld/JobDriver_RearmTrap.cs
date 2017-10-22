using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RearmTrap : JobDriver
	{
		private const int RearmTicks = 1125;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.RearmTrap);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			Toil gotoThing = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator38)/*Error near IL_006b: stateMachine*/)._003C_003Ef__this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator38)/*Error near IL_006b: stateMachine*/)._003C_003Ef__this.TargetThingA, PathEndMode.Touch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_General.Wait(1125).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Thing thing = ((_003CMakeNewToils_003Ec__Iterator38)/*Error near IL_00e6: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing;
					Designation designation = ((_003CMakeNewToils_003Ec__Iterator38)/*Error near IL_00e6: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(thing, DesignationDefOf.RearmTrap);
					if (designation != null)
					{
						designation.Delete();
					}
					Building_TrapRearmable building_TrapRearmable = thing as Building_TrapRearmable;
					building_TrapRearmable.Rearm();
					((_003CMakeNewToils_003Ec__Iterator38)/*Error near IL_00e6: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.TrapsRearmed);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
