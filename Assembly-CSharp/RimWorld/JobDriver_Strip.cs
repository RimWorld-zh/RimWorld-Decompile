using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Strip : JobDriver
	{
		private const int StripTicks = 60;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !StrippableUtility.CanBeStrippedByColony(((_003CMakeNewToils_003Ec__Iterator3C)/*Error near IL_0040: stateMachine*/)._003C_003Ef__this.TargetThingA)));
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			Toil gotoThing = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator3C)/*Error near IL_007e: stateMachine*/)._003C_003Ef__this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator3C)/*Error near IL_007e: stateMachine*/)._003C_003Ef__this.TargetThingA, PathEndMode.ClosestTouch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_General.Wait(60).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Thing thing = ((_003CMakeNewToils_003Ec__Iterator3C)/*Error near IL_00f6: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing;
					Designation designation = ((_003CMakeNewToils_003Ec__Iterator3C)/*Error near IL_00f6: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
					if (designation != null)
					{
						designation.Delete();
					}
					IStrippable strippable = thing as IStrippable;
					if (strippable != null)
					{
						strippable.Strip();
					}
					((_003CMakeNewToils_003Ec__Iterator3C)/*Error near IL_00f6: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.BodiesStripped);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
