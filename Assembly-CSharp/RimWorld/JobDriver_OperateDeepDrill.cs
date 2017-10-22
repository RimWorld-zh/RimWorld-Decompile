using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_OperateDeepDrill : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn((Func<bool>)delegate
			{
				CompDeepDrill compDeepDrill = ((_003CMakeNewToils_003Ec__Iterator37)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing.TryGetComp<CompDeepDrill>();
				return !compDeepDrill.CanDrillNow();
			});
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil work = new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator37)/*Error near IL_00a0: stateMachine*/)._003Cwork_003E__0.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					CompDeepDrill comp = building.GetComp<CompDeepDrill>();
					comp.DrillWorkDone(actor);
					actor.skills.Learn(SkillDefOf.Mining, 0.0714999959f, false);
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			work.WithEffect(EffecterDefOf.Drill, TargetIndex.A);
			work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return work;
		}
	}
}
