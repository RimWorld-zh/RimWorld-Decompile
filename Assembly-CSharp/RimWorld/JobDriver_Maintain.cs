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
	public class JobDriver_Maintain : JobDriver
	{
		private const int MaintainTicks = 180;

		public JobDriver_Maintain()
		{
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo targetA = this.job.targetA;
			Job job = this.job;
			return pawn.Reserve(targetA, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil prepare = Toils_General.Wait(180, TargetIndex.None);
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return prepare;
			Toil maintain = new Toil();
			maintain.initAction = delegate()
			{
				Pawn actor = maintain.actor;
				CompMaintainable compMaintainable = actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>();
				compMaintainable.Maintained();
			};
			maintain.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return maintain;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <prepare>__0;

			internal JobDriver_Maintain $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Maintain.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDespawnedOrNull(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					prepare = Toils_General.Wait(180, TargetIndex.None);
					prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					this.$current = prepare;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil maintain = new Toil();
					maintain.initAction = delegate()
					{
						Pawn actor = maintain.actor;
						CompMaintainable compMaintainable = actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>();
						compMaintainable.Maintained();
					};
					maintain.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = maintain;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
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
				JobDriver_Maintain.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Maintain.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil maintain;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.maintain.actor;
					CompMaintainable compMaintainable = actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>();
					compMaintainable.Maintained();
				}
			}
		}
	}
}
