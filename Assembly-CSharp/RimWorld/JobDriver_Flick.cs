using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Flick : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)delegate
			{
				Designation designation2 = ((_003CMakeNewToils_003Ec__Iterator2D)/*Error near IL_0033: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(((_003CMakeNewToils_003Ec__Iterator2D)/*Error near IL_0033: stateMachine*/)._003C_003Ef__this.TargetThingA, DesignationDefOf.Flick);
				if (designation2 != null)
				{
					return false;
				}
				return true;
			});
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(15).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator2D)/*Error near IL_00aa: stateMachine*/)._003Cfinalize_003E__0.actor;
					ThingWithComps thingWithComps = (ThingWithComps)actor.CurJob.targetA.Thing;
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
						if (compFlickable != null && compFlickable.WantsFlick())
						{
							compFlickable.DoFlick();
						}
					}
					actor.records.Increment(RecordDefOf.SwitchesFlicked);
					Designation designation = ((_003CMakeNewToils_003Ec__Iterator2D)/*Error near IL_00aa: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick);
					if (designation != null)
					{
						designation.Delete();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
