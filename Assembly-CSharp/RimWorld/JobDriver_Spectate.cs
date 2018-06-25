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
	public class JobDriver_Spectate : JobDriver
	{
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		private const TargetIndex WatchTargetInd = TargetIndex.B;

		public JobDriver_Spectate()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool haveChair = this.job.GetTarget(TargetIndex.A).HasThing;
			if (haveChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				tickAction = delegate()
				{
					this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
					this.pawn.GainComfortFromCellIfPossible();
					if (this.pawn.IsHashIntervalTick(100))
					{
						this.pawn.jobs.CheckForJobOverride();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal bool <haveChair>__0;

			internal Toil <spectate>__0;

			internal JobDriver_Spectate $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

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
					haveChair = this.job.GetTarget(TargetIndex.A).HasThing;
					if (haveChair)
					{
						this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
					}
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil spectate = new Toil();
					spectate.tickAction = delegate()
					{
						this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
						this.pawn.GainComfortFromCellIfPossible();
						if (this.pawn.IsHashIntervalTick(100))
						{
							this.pawn.jobs.CheckForJobOverride();
						}
					};
					spectate.defaultCompleteMode = ToilCompleteMode.Never;
					spectate.handlingFacing = true;
					this.$current = spectate;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
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
				JobDriver_Spectate.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Spectate.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
				this.pawn.GainComfortFromCellIfPossible();
				if (this.pawn.IsHashIntervalTick(100))
				{
					this.pawn.jobs.CheckForJobOverride();
				}
			}
		}
	}
}
