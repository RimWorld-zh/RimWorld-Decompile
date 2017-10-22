using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public abstract class JobDriver_PlantWork : JobDriver
	{
		protected const TargetIndex PlantInd = TargetIndex.A;

		private float workDone;

		protected float xpPerTick;

		protected Plant Plant
		{
			get
			{
				return (Plant)base.CurJob.targetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			yield return Toils_Reserve.ReserveQueue(TargetIndex.A, 1, -1, null);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
			Toil checkNextQueuedTarget = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, checkNextQueuedTarget);
			Toil cut = new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003Ccut_003E__2.actor;
					if (actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Growing, ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.xpPerTick, false);
					}
					float statValue;
					float num = statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
					Plant plant = ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.Plant;
					((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.workDone += statValue;
					if (((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.workDone >= plant.def.plant.harvestWork)
					{
						if (plant.def.plant.harvestedThingDef != null)
						{
							if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
							{
								Vector3 loc = (((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.pawn.DrawPos + plant.DrawPos) / 2f;
								MoteMaker.ThrowText(loc, ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
							}
							else
							{
								int num2 = plant.YieldNow();
								if (num2 > 0)
								{
									Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef, null);
									thing.stackCount = num2;
									if (actor.Faction != Faction.OfPlayer)
									{
										thing.SetForbidden(true, true);
									}
									GenPlace.TryPlaceThing(thing, actor.Position, ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.Map, ThingPlaceMode.Near, null);
									actor.records.Increment(RecordDefOf.PlantsHarvested);
								}
							}
						}
						plant.def.plant.soundHarvestFinish.PlayOneShot((Thing)actor);
						plant.PlantCollected();
						((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.workDone = 0f;
						((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_00f9: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				}
			};
			cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			cut.defaultCompleteMode = ToilCompleteMode.Never;
			cut.WithEffect(EffecterDefOf.Harvest, TargetIndex.A);
			cut.WithProgressBar(TargetIndex.A, (Func<float>)(() => ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_014a: stateMachine*/)._003C_003Ef__this.workDone / ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_014a: stateMachine*/)._003C_003Ef__this.Plant.def.plant.harvestWork), true, -0.5f);
			cut.PlaySustainerOrSound((Func<SoundDef>)(() => ((_003CMakeNewToils_003Ec__Iterator46)/*Error near IL_0168: stateMachine*/)._003C_003Ef__this.Plant.def.plant.soundHarvesting));
			yield return cut;
			yield return checkNextQueuedTarget;
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.A, initExtractTargetFromQueue);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		protected virtual void Init()
		{
		}
	}
}
