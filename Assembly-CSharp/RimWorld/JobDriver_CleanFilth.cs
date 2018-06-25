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
	public class JobDriver_CleanFilth : JobDriver
	{
		private float cleaningWorkDone = 0f;

		private float totalCleaningWorkDone = 0f;

		private float totalCleaningWorkRequired = 0f;

		private const TargetIndex FilthInd = TargetIndex.A;

		public JobDriver_CleanFilth()
		{
		}

		private Filth Filth
		{
			get
			{
				return (Filth)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue).JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			Toil clean = new Toil();
			clean.initAction = delegate()
			{
				this.cleaningWorkDone = 0f;
				this.totalCleaningWorkDone = 0f;
				this.totalCleaningWorkRequired = this.Filth.def.filth.cleaningWorkToReduceThickness * (float)this.Filth.thickness;
			};
			clean.tickAction = delegate()
			{
				Filth filth = this.Filth;
				this.cleaningWorkDone += 1f;
				this.totalCleaningWorkDone += 1f;
				if (this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
				{
					filth.ThinFilth();
					this.cleaningWorkDone = 0f;
					if (filth.Destroyed)
					{
						clean.actor.records.Increment(RecordDefOf.MessesCleaned);
						this.ReadyForNextToil();
					}
				}
			};
			clean.defaultCompleteMode = ToilCompleteMode.Never;
			clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
			clean.WithProgressBar(TargetIndex.A, () => this.totalCleaningWorkDone / this.totalCleaningWorkRequired, true, -0.5f);
			clean.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
			clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			yield return clean;
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <initExtractTargetFromQueue>__0;

			internal JobDriver_CleanFilth $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_CleanFilth.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private static Func<SoundDef> <>f__am$cache0;

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
					initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
					this.$current = initExtractTargetFromQueue;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue).JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					<MakeNewToils>c__AnonStorey.clean = new Toil();
					<MakeNewToils>c__AnonStorey.clean.initAction = delegate()
					{
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.cleaningWorkDone = 0f;
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.totalCleaningWorkDone = 0f;
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.totalCleaningWorkRequired = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Filth.def.filth.cleaningWorkToReduceThickness * (float)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Filth.thickness;
					};
					<MakeNewToils>c__AnonStorey.clean.tickAction = delegate()
					{
						Filth filth = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Filth;
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.cleaningWorkDone += 1f;
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.totalCleaningWorkDone += 1f;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
						{
							filth.ThinFilth();
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.cleaningWorkDone = 0f;
							if (filth.Destroyed)
							{
								<MakeNewToils>c__AnonStorey.clean.actor.records.Increment(RecordDefOf.MessesCleaned);
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
							}
						}
					};
					<MakeNewToils>c__AnonStorey.clean.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.clean.WithProgressBar(TargetIndex.A, () => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.totalCleaningWorkDone / <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.totalCleaningWorkRequired, true, -0.5f);
					<MakeNewToils>c__AnonStorey.clean.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
					<MakeNewToils>c__AnonStorey.clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
					<MakeNewToils>c__AnonStorey.clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
					this.$current = <MakeNewToils>c__AnonStorey.clean;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Jump.Jump(initExtractTargetFromQueue);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
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
				JobDriver_CleanFilth.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_CleanFilth.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SoundDef <>m__0()
			{
				return SoundDefOf.Interact_CleanFilth;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil clean;

				internal JobDriver_CleanFilth.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.cleaningWorkDone = 0f;
					this.<>f__ref$0.$this.totalCleaningWorkDone = 0f;
					this.<>f__ref$0.$this.totalCleaningWorkRequired = this.<>f__ref$0.$this.Filth.def.filth.cleaningWorkToReduceThickness * (float)this.<>f__ref$0.$this.Filth.thickness;
				}

				internal void <>m__1()
				{
					Filth filth = this.<>f__ref$0.$this.Filth;
					this.<>f__ref$0.$this.cleaningWorkDone += 1f;
					this.<>f__ref$0.$this.totalCleaningWorkDone += 1f;
					if (this.<>f__ref$0.$this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
					{
						filth.ThinFilth();
						this.<>f__ref$0.$this.cleaningWorkDone = 0f;
						if (filth.Destroyed)
						{
							this.clean.actor.records.Increment(RecordDefOf.MessesCleaned);
							this.<>f__ref$0.$this.ReadyForNextToil();
						}
					}
				}

				internal float <>m__2()
				{
					return this.<>f__ref$0.$this.totalCleaningWorkDone / this.<>f__ref$0.$this.totalCleaningWorkRequired;
				}
			}
		}
	}
}
