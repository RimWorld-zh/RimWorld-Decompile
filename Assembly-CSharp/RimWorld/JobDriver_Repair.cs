using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Repair : JobDriver
	{
		private const float WarmupTicks = 80f;

		private const float TicksBetweenRepairs = 20f;

		protected float ticksToNextRepair;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil repair = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_007b: stateMachine*/)._003C_003Ef__this.ticksToNextRepair = 80f;
				},
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003Crepair_003E__0.actor;
					actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
					float statValue = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.ticksToNextRepair -= statValue;
					if (((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.ticksToNextRepair <= 0.0)
					{
						((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.ticksToNextRepair += 20f;
						((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA.HitPoints++;
						((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA.HitPoints = Mathf.Min(((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA.HitPoints, ((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA.MaxHitPoints);
						((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA);
						if (((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA.HitPoints == ((_003CMakeNewToils_003Ec__Iterator13)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.TargetThingA.MaxHitPoints)
						{
							actor.records.Increment(RecordDefOf.ThingsRepaired);
							actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					}
				}
			};
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			yield return repair;
		}
	}
}
