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
	public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
	{
		private float gatherProgress;

		protected const TargetIndex AnimalInd = TargetIndex.A;

		protected JobDriver_GatherAnimalBodyResources()
		{
		}

		protected abstract float WorkTotal { get; }

		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil wait = new Toil();
			wait.initAction = delegate()
			{
				Pawn actor = wait.actor;
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				actor.pather.StopDead();
				PawnUtility.ForceWait(pawn, 15000, null, true);
			};
			wait.tickAction = delegate()
			{
				Pawn actor = wait.actor;
				actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
				this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
				if (this.gatherProgress >= this.WorkTotal)
				{
					this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).Gathered(this.pawn);
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
			};
			wait.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			});
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.AddEndCondition(delegate
			{
				if (!this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).ActiveAndFull)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f);
			wait.activeSkill = (() => SkillDefOf.Animals);
			yield return wait;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_GatherAnimalBodyResources $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_GatherAnimalBodyResources.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOnDowned(TargetIndex.A);
					this.FailOnNotCasualInterruptible(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.wait = new Toil();
					<MakeNewToils>c__AnonStorey.wait.initAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.wait.actor;
						Pawn pawn = (Pawn)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.GetTarget(TargetIndex.A).Thing;
						actor.pather.StopDead();
						PawnUtility.ForceWait(pawn, 15000, null, true);
					};
					<MakeNewToils>c__AnonStorey.wait.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.wait.actor;
						actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.gatherProgress >= <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.WorkTotal)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.GetComp((Pawn)((Thing)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.GetTarget(TargetIndex.A))).Gathered(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn);
							actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					};
					<MakeNewToils>c__AnonStorey.wait.AddFinishAction(delegate
					{
						Pawn pawn = (Pawn)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.GetTarget(TargetIndex.A).Thing;
						if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
						{
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					});
					<MakeNewToils>c__AnonStorey.wait.FailOnDespawnedOrNull(TargetIndex.A);
					<MakeNewToils>c__AnonStorey.wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					<MakeNewToils>c__AnonStorey.wait.AddEndCondition(delegate
					{
						if (!<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.GetComp((Pawn)((Thing)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.GetTarget(TargetIndex.A))).ActiveAndFull)
						{
							return JobCondition.Incompletable;
						}
						return JobCondition.Ongoing;
					});
					<MakeNewToils>c__AnonStorey.wait.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.wait.WithProgressBar(TargetIndex.A, () => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.gatherProgress / <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.WorkTotal, false, -0.5f);
					<MakeNewToils>c__AnonStorey.wait.activeSkill = (() => SkillDefOf.Animals);
					this.$current = <MakeNewToils>c__AnonStorey.wait;
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
				JobDriver_GatherAnimalBodyResources.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_GatherAnimalBodyResources.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Animals;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil wait;

				internal JobDriver_GatherAnimalBodyResources.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.wait.actor;
					Pawn pawn = (Pawn)this.<>f__ref$0.$this.job.GetTarget(TargetIndex.A).Thing;
					actor.pather.StopDead();
					PawnUtility.ForceWait(pawn, 15000, null, true);
				}

				internal void <>m__1()
				{
					Pawn actor = this.wait.actor;
					actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
					this.<>f__ref$0.$this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
					if (this.<>f__ref$0.$this.gatherProgress >= this.<>f__ref$0.$this.WorkTotal)
					{
						this.<>f__ref$0.$this.GetComp((Pawn)((Thing)this.<>f__ref$0.$this.job.GetTarget(TargetIndex.A))).Gathered(this.<>f__ref$0.$this.pawn);
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}

				internal void <>m__2()
				{
					Pawn pawn = (Pawn)this.<>f__ref$0.$this.job.GetTarget(TargetIndex.A).Thing;
					if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
				}

				internal JobCondition <>m__3()
				{
					if (!this.<>f__ref$0.$this.GetComp((Pawn)((Thing)this.<>f__ref$0.$this.job.GetTarget(TargetIndex.A))).ActiveAndFull)
					{
						return JobCondition.Incompletable;
					}
					return JobCondition.Ongoing;
				}

				internal float <>m__4()
				{
					return this.<>f__ref$0.$this.gatherProgress / this.<>f__ref$0.$this.WorkTotal;
				}
			}
		}
	}
}
