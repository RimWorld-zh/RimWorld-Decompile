using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Open : JobDriver
	{
		private const int OpenTicks = 300;

		private IOpenable Openable
		{
			get
			{
				return (IOpenable)base.CurJob.targetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (!((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_0042: stateMachine*/)._003C_003Ef__this.Openable.CanOpen)
					{
						Designation designation2 = ((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_0042: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_0042: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing, DesignationDefOf.Open);
						if (designation2 != null)
						{
							designation2.Delete();
						}
					}
				}
			}.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Open).FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(300).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDestroyedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Thing thing = ((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing;
					Designation designation = ((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
					if (designation != null)
					{
						designation.Delete();
					}
					if (((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.Openable.CanOpen)
					{
						((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.Openable.Open();
						((_003CMakeNewToils_003Ec__Iterator36)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.ContainersOpened);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
