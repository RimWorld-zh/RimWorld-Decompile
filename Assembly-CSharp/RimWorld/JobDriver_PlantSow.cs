using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200008B RID: 139
	public class JobDriver_PlantSow : JobDriver
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000281B4 File Offset: 0x000265B4
		private Plant Plant
		{
			get
			{
				return (Plant)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000281E2 File Offset: 0x000265E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00028204 File Offset: 0x00026604
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00028238 File Offset: 0x00026638
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn(() => GenPlant.AdjacentSowBlocker(this.job.plantDefToSow, this.TargetA.Cell, this.Map) != null).FailOn(() => !this.job.plantDefToSow.CanEverPlantAt(this.TargetLocA, this.Map));
			Toil sowToil = new Toil();
			sowToil.initAction = delegate()
			{
				this.TargetThingA = GenSpawn.Spawn(this.job.plantDefToSow, this.TargetLocA, this.Map, WipeMode.Vanish);
				this.pawn.Reserve(this.TargetThingA, sowToil.actor.CurJob, 1, -1, null);
				Plant plant = (Plant)this.TargetThingA;
				plant.Growth = 0f;
				plant.sown = true;
			};
			sowToil.tickAction = delegate()
			{
				Pawn actor = sowToil.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Growing, 0.0935f, false);
				}
				float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				float num = statValue;
				Plant plant = this.Plant;
				if (plant.LifeStage != PlantLifeStage.Sowing)
				{
					Log.Error(this.$this + " getting sowing work while not in Sowing life stage.", false);
				}
				this.sowWorkDone += num;
				if (this.sowWorkDone >= plant.def.plant.sowWork)
				{
					plant.Growth = 0.05f;
					this.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlag.Things);
					actor.records.Increment(RecordDefOf.PlantsSown);
					this.ReadyForNextToil();
				}
			};
			sowToil.defaultCompleteMode = ToilCompleteMode.Never;
			sowToil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			sowToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			sowToil.WithEffect(EffecterDefOf.Sow, TargetIndex.A);
			sowToil.WithProgressBar(TargetIndex.A, () => this.sowWorkDone / this.Plant.def.plant.sowWork, true, -0.5f);
			sowToil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow);
			sowToil.AddFinishAction(delegate
			{
				if (this.TargetThingA != null)
				{
					Plant plant = (Plant)sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
					if (this.sowWorkDone < plant.def.plant.sowWork && !this.TargetThingA.Destroyed)
					{
						this.TargetThingA.Destroy(DestroyMode.Vanish);
					}
				}
			});
			sowToil.activeSkill = (() => SkillDefOf.Growing);
			yield return sowToil;
			yield break;
		}

		// Token: 0x0400024A RID: 586
		private float sowWorkDone = 0f;
	}
}
