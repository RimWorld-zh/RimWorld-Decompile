using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public abstract class JobDriver_PlantWork : JobDriver
	{
		private float workDone = 0f;

		protected float xpPerTick = 0f;

		protected const TargetIndex PlantInd = TargetIndex.A;

		protected JobDriver_PlantWork()
		{
		}

		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (target.IsValid)
			{
				if (!this.pawn.Reserve(target, this.job, 1, -1, null))
				{
					return false;
				}
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (this.RequiredDesignation == null) ? null : new Func<Thing, bool>((Thing t) => this.Map.designationManager.DesignationOn(t, this.RequiredDesignation) != null));
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil gotoThing = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			if (this.RequiredDesignation != null)
			{
				gotoThing.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			yield return gotoThing;
			Toil cut = new Toil();
			cut.tickAction = delegate()
			{
				Pawn actor = cut.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Plants, this.xpPerTick, false);
				}
				float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				float num = statValue;
				Plant plant = this.Plant;
				num *= Mathf.Lerp(3.3f, 1f, plant.Growth);
				this.workDone += num;
				if (this.workDone >= plant.def.plant.harvestWork)
				{
					if (plant.def.plant.harvestedThingDef != null)
					{
						if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
						{
							Vector3 loc = (this.pawn.DrawPos + plant.DrawPos) / 2f;
							MoteMaker.ThrowText(loc, this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
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
								GenPlace.TryPlaceThing(thing, actor.Position, this.Map, ThingPlaceMode.Near, null, null);
								actor.records.Increment(RecordDefOf.PlantsHarvested);
							}
						}
					}
					plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
					plant.PlantCollected();
					this.workDone = 0f;
					this.ReadyForNextToil();
				}
			};
			cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			if (this.RequiredDesignation != null)
			{
				cut.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			cut.defaultCompleteMode = ToilCompleteMode.Never;
			cut.WithEffect(EffecterDefOf.Harvest, TargetIndex.A);
			cut.WithProgressBar(TargetIndex.A, () => this.workDone / this.Plant.def.plant.harvestWork, true, -0.5f);
			cut.PlaySustainerOrSound(() => this.Plant.def.plant.soundHarvesting);
			cut.activeSkill = (() => SkillDefOf.Plants);
			yield return cut;
			Toil plantWorkDoneToil = this.PlantWorkDoneToil();
			if (plantWorkDoneToil != null)
			{
				yield return plantWorkDoneToil;
			}
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		protected virtual void Init()
		{
		}

		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <initExtractTargetFromQueue>__0;

			internal Toil <gotoThing>__1;

			internal Toil <plantWorkDoneToil>__0;

			internal JobDriver_PlantWork $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_PlantWork.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private static Func<SkillDef> <>f__am$cache0;

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
					this.Init();
					this.$current = Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (this.RequiredDesignation == null) ? null : new Func<Thing, bool>((Thing t) => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.designationManager.DesignationOn(t, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.RequiredDesignation) != null));
					this.$current = initExtractTargetFromQueue;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					gotoThing = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
					if (this.RequiredDesignation != null)
					{
						gotoThing.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
					}
					this.$current = gotoThing;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					<MakeNewToils>c__AnonStorey.cut = new Toil();
					<MakeNewToils>c__AnonStorey.cut.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.cut.actor;
						if (actor.skills != null)
						{
							actor.skills.Learn(SkillDefOf.Plants, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.xpPerTick, false);
						}
						float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
						float num2 = statValue;
						Plant plant = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Plant;
						num2 *= Mathf.Lerp(3.3f, 1f, plant.Growth);
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone += num2;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone >= plant.def.plant.harvestWork)
						{
							if (plant.def.plant.harvestedThingDef != null)
							{
								if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
								{
									Vector3 loc = (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.DrawPos + plant.DrawPos) / 2f;
									MoteMaker.ThrowText(loc, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
								}
								else
								{
									int num3 = plant.YieldNow();
									if (num3 > 0)
									{
										Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef, null);
										thing.stackCount = num3;
										if (actor.Faction != Faction.OfPlayer)
										{
											thing.SetForbidden(true, true);
										}
										GenPlace.TryPlaceThing(thing, actor.Position, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map, ThingPlaceMode.Near, null, null);
										actor.records.Increment(RecordDefOf.PlantsHarvested);
									}
								}
							}
							plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
							plant.PlantCollected();
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone = 0f;
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
						}
					};
					<MakeNewToils>c__AnonStorey.cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					if (this.RequiredDesignation != null)
					{
						<MakeNewToils>c__AnonStorey.cut.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
					}
					<MakeNewToils>c__AnonStorey.cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					<MakeNewToils>c__AnonStorey.cut.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.cut.WithEffect(EffecterDefOf.Harvest, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.cut.WithProgressBar(TargetIndex.A, () => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone / <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Plant.def.plant.harvestWork, true, -0.5f);
					<MakeNewToils>c__AnonStorey.cut.PlaySustainerOrSound(() => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Plant.def.plant.soundHarvesting);
					<MakeNewToils>c__AnonStorey.cut.activeSkill = (() => SkillDefOf.Plants);
					this.$current = <MakeNewToils>c__AnonStorey.cut;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					plantWorkDoneToil = this.PlantWorkDoneToil();
					if (plantWorkDoneToil != null)
					{
						this.$current = plantWorkDoneToil;
						if (!this.$disposing)
						{
							this.$PC = 7;
						}
						return true;
					}
					break;
				case 7u:
					break;
				case 8u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				this.$current = Toils_Jump.Jump(initExtractTargetFromQueue);
				if (!this.$disposing)
				{
					this.$PC = 8;
				}
				return true;
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
				JobDriver_PlantWork.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_PlantWork.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Plants;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil cut;

				internal JobDriver_PlantWork.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Thing t)
				{
					return this.<>f__ref$0.$this.Map.designationManager.DesignationOn(t, this.<>f__ref$0.$this.RequiredDesignation) != null;
				}

				internal void <>m__1()
				{
					Pawn actor = this.cut.actor;
					if (actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Plants, this.<>f__ref$0.$this.xpPerTick, false);
					}
					float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
					float num = statValue;
					Plant plant = this.<>f__ref$0.$this.Plant;
					num *= Mathf.Lerp(3.3f, 1f, plant.Growth);
					this.<>f__ref$0.$this.workDone += num;
					if (this.<>f__ref$0.$this.workDone >= plant.def.plant.harvestWork)
					{
						if (plant.def.plant.harvestedThingDef != null)
						{
							if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
							{
								Vector3 loc = (this.<>f__ref$0.$this.pawn.DrawPos + plant.DrawPos) / 2f;
								MoteMaker.ThrowText(loc, this.<>f__ref$0.$this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
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
									GenPlace.TryPlaceThing(thing, actor.Position, this.<>f__ref$0.$this.Map, ThingPlaceMode.Near, null, null);
									actor.records.Increment(RecordDefOf.PlantsHarvested);
								}
							}
						}
						plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
						plant.PlantCollected();
						this.<>f__ref$0.$this.workDone = 0f;
						this.<>f__ref$0.$this.ReadyForNextToil();
					}
				}

				internal float <>m__2()
				{
					return this.<>f__ref$0.$this.workDone / this.<>f__ref$0.$this.Plant.def.plant.harvestWork;
				}

				internal SoundDef <>m__3()
				{
					return this.<>f__ref$0.$this.Plant.def.plant.soundHarvesting;
				}
			}
		}
	}
}
