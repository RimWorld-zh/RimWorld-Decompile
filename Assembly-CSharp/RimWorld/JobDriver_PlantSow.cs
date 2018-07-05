using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantSow : JobDriver
	{
		private float sowWorkDone = 0f;

		public JobDriver_PlantSow()
		{
		}

		private Plant Plant
		{
			get
			{
				return (Plant)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

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
					actor.skills.Learn(SkillDefOf.Plants, 0.0935f, false);
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
			sowToil.activeSkill = (() => SkillDefOf.Plants);
			yield return sowToil;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_PlantSow $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_PlantSow.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private static Func<SoundDef> <>f__am$cache0;

			private static Func<SkillDef> <>f__am$cache1;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn(() => GenPlant.AdjacentSowBlocker(this.job.plantDefToSow, this.TargetA.Cell, this.Map) != null).FailOn(() => !this.job.plantDefToSow.CanEverPlantAt(this.TargetLocA, this.Map));
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.sowToil = new Toil();
					<MakeNewToils>c__AnonStorey.sowToil.initAction = delegate()
					{
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA = GenSpawn.Spawn(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.plantDefToSow, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetLocA, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map, WipeMode.Vanish);
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Reserve(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA, <MakeNewToils>c__AnonStorey.sowToil.actor.CurJob, 1, -1, null);
						Plant plant = (Plant)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA;
						plant.Growth = 0f;
						plant.sown = true;
					};
					<MakeNewToils>c__AnonStorey.sowToil.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.sowToil.actor;
						if (actor.skills != null)
						{
							actor.skills.Learn(SkillDefOf.Plants, 0.0935f, false);
						}
						float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
						float num2 = statValue;
						Plant plant = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Plant;
						if (plant.LifeStage != PlantLifeStage.Sowing)
						{
							Log.Error(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this + " getting sowing work while not in Sowing life stage.", false);
						}
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.sowWorkDone += num2;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.sowWorkDone >= plant.def.plant.sowWork)
						{
							plant.Growth = 0.05f;
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlag.Things);
							actor.records.Increment(RecordDefOf.PlantsSown);
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
						}
					};
					<MakeNewToils>c__AnonStorey.sowToil.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.sowToil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					<MakeNewToils>c__AnonStorey.sowToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					<MakeNewToils>c__AnonStorey.sowToil.WithEffect(EffecterDefOf.Sow, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.sowToil.WithProgressBar(TargetIndex.A, () => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.sowWorkDone / <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Plant.def.plant.sowWork, true, -0.5f);
					<MakeNewToils>c__AnonStorey.sowToil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow);
					<MakeNewToils>c__AnonStorey.sowToil.AddFinishAction(delegate
					{
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA != null)
						{
							Plant plant = (Plant)<MakeNewToils>c__AnonStorey.sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
							if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.sowWorkDone < plant.def.plant.sowWork && !<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.Destroyed)
							{
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetThingA.Destroy(DestroyMode.Vanish);
							}
						}
					});
					<MakeNewToils>c__AnonStorey.sowToil.activeSkill = (() => SkillDefOf.Plants);
					this.$current = <MakeNewToils>c__AnonStorey.sowToil;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_PlantSow.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_PlantSow.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SoundDef <>m__0()
			{
				return SoundDefOf.Interact_Sow;
			}

			private static SkillDef <>m__1()
			{
				return SkillDefOf.Plants;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil sowToil;

				internal JobDriver_PlantSow.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					return GenPlant.AdjacentSowBlocker(this.<>f__ref$0.$this.job.plantDefToSow, this.<>f__ref$0.$this.TargetA.Cell, this.<>f__ref$0.$this.Map) != null;
				}

				internal bool <>m__1()
				{
					return !this.<>f__ref$0.$this.job.plantDefToSow.CanEverPlantAt(this.<>f__ref$0.$this.TargetLocA, this.<>f__ref$0.$this.Map);
				}

				internal void <>m__2()
				{
					this.<>f__ref$0.$this.TargetThingA = GenSpawn.Spawn(this.<>f__ref$0.$this.job.plantDefToSow, this.<>f__ref$0.$this.TargetLocA, this.<>f__ref$0.$this.Map, WipeMode.Vanish);
					this.<>f__ref$0.$this.pawn.Reserve(this.<>f__ref$0.$this.TargetThingA, this.sowToil.actor.CurJob, 1, -1, null);
					Plant plant = (Plant)this.<>f__ref$0.$this.TargetThingA;
					plant.Growth = 0f;
					plant.sown = true;
				}

				internal void <>m__3()
				{
					Pawn actor = this.sowToil.actor;
					if (actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Plants, 0.0935f, false);
					}
					float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
					float num = statValue;
					Plant plant = this.<>f__ref$0.$this.Plant;
					if (plant.LifeStage != PlantLifeStage.Sowing)
					{
						Log.Error(this.<>f__ref$0.$this + " getting sowing work while not in Sowing life stage.", false);
					}
					this.<>f__ref$0.$this.sowWorkDone += num;
					if (this.<>f__ref$0.$this.sowWorkDone >= plant.def.plant.sowWork)
					{
						plant.Growth = 0.05f;
						this.<>f__ref$0.$this.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlag.Things);
						actor.records.Increment(RecordDefOf.PlantsSown);
						this.<>f__ref$0.$this.ReadyForNextToil();
					}
				}

				internal float <>m__4()
				{
					return this.<>f__ref$0.$this.sowWorkDone / this.<>f__ref$0.$this.Plant.def.plant.sowWork;
				}

				internal void <>m__5()
				{
					if (this.<>f__ref$0.$this.TargetThingA != null)
					{
						Plant plant = (Plant)this.sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
						if (this.<>f__ref$0.$this.sowWorkDone < plant.def.plant.sowWork && !this.<>f__ref$0.$this.TargetThingA.Destroyed)
						{
							this.<>f__ref$0.$this.TargetThingA.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}
	}
}
