using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ConstructFinishFrame : JobDriver
	{
		private const int JobEndInterval = 5000;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil build = new Toil
			{
				initAction = (Action)delegate
				{
					GenClamor.DoClamor(((_003CMakeNewToils_003Ec__IteratorE)/*Error near IL_0074: stateMachine*/)._003Cbuild_003E__0.actor, 15f, ClamorType.Construction);
				},
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__IteratorE)/*Error near IL_008b: stateMachine*/)._003Cbuild_003E__0.actor;
					Frame frame = (Frame)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
					if (frame.resourceContainer.Count > 0)
					{
						actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
					}
					float statValue = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					float workToMake = frame.WorkToMake;
					if (actor.Faction == Faction.OfPlayer)
					{
						float statValue2 = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
						if (Rand.Value < 1.0 - Mathf.Pow(statValue2, statValue / workToMake))
						{
							frame.FailConstruction(actor);
							((_003CMakeNewToils_003Ec__IteratorE)/*Error near IL_008b: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
							return;
						}
					}
					if (frame.def.entityDefToBuild is TerrainDef)
					{
						((_003CMakeNewToils_003Ec__IteratorE)/*Error near IL_008b: stateMachine*/)._003C_003Ef__this.Map.snowGrid.SetDepth(frame.Position, 0f);
					}
					frame.workDone += statValue;
					if (frame.workDone >= workToMake)
					{
						frame.CompleteConstruction(actor);
						((_003CMakeNewToils_003Ec__IteratorE)/*Error near IL_008b: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				}
			};
			build.WithEffect((Func<EffecterDef>)(() => ((Frame)((_003CMakeNewToils_003Ec__IteratorE)/*Error near IL_00a2: stateMachine*/)._003Cbuild_003E__0.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect), TargetIndex.A);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = 5000;
			yield return build;
		}
	}
}
