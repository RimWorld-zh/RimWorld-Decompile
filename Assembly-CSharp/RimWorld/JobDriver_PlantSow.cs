using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantSow : JobDriver
	{
		private float sowWorkDone;

		private Plant Plant
		{
			get
			{
				return (Plant)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn((Func<bool>)(() => GenPlant.AdjacentSowBlocker(((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.CurJob.plantDefToSow, ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.TargetA.Cell, ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.Map) != null)).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0061: stateMachine*/)._003C_003Ef__this.CurJob.plantDefToSow.CanEverPlantAt(((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0061: stateMachine*/)._003C_003Ef__this.TargetLocA, ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0061: stateMachine*/)._003C_003Ef__this.Map)));
			Toil sowToil = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.TargetThingA = GenSpawn.Spawn(((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.CurJob.plantDefToSow, ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.TargetLocA, ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.Map);
					((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.pawn.Reserve(((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.TargetThingA, 1, -1, null);
					Plant plant3 = (Plant)((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0094: stateMachine*/)._003C_003Ef__this.TargetThingA;
					plant3.Growth = 0f;
					plant3.sown = true;
				},
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003CsowToil_003E__0.actor;
					if (actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Growing, 0.11f, false);
					}
					float statValue;
					float num = statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
					Plant plant2 = ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.Plant;
					if (plant2.LifeStage != 0)
					{
						Log.Error(((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this + " getting sowing work while not in Sowing life stage.");
					}
					((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.sowWorkDone += statValue;
					if (((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.sowWorkDone >= plant2.def.plant.sowWork)
					{
						plant2.Growth = 0.05f;
						((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.Map.mapDrawer.MapMeshDirty(plant2.Position, MapMeshFlag.Things);
						actor.records.Increment(RecordDefOf.PlantsSown);
						((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00ab: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			sowToil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			sowToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			sowToil.WithEffect(EffecterDefOf.Sow, TargetIndex.A);
			sowToil.WithProgressBar(TargetIndex.A, (Func<float>)(() => ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.sowWorkDone / ((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_00fc: stateMachine*/)._003C_003Ef__this.Plant.def.plant.sowWork), true, -0.5f);
			sowToil.PlaySustainerOrSound((Func<SoundDef>)(() => SoundDefOf.Interact_Sow));
			sowToil.AddFinishAction((Action)delegate
			{
				if (((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0143: stateMachine*/)._003C_003Ef__this.TargetThingA != null)
				{
					Plant plant = (Plant)((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0143: stateMachine*/)._003CsowToil_003E__0.actor.CurJob.GetTarget(TargetIndex.A).Thing;
					if (((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0143: stateMachine*/)._003C_003Ef__this.sowWorkDone < plant.def.plant.sowWork && !((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0143: stateMachine*/)._003C_003Ef__this.TargetThingA.Destroyed)
					{
						((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_0143: stateMachine*/)._003C_003Ef__this.TargetThingA.Destroy(DestroyMode.Vanish);
					}
				}
			});
			yield return sowToil;
			if (base.pawn.story.traits.HasTrait(TraitDefOf.GreenThumb))
			{
				yield return new Toil
				{
					initAction = (Action)delegate
					{
						((_003CMakeNewToils_003Ec__Iterator45)/*Error near IL_01a1: stateMachine*/)._003C_003Ef__this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GreenThumbHappy, null);
					}
				};
			}
		}
	}
}
