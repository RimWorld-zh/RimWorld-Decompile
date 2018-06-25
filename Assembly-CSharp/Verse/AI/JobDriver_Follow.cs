using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse.AI
{
	public class JobDriver_Follow : JobDriver
	{
		private const TargetIndex FolloweeInd = TargetIndex.A;

		private const int Distance = 4;

		public JobDriver_Follow()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
					if (!this.pawn.Position.InHorDistOf(pawn.Position, 4f) || !this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
					{
						if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							base.EndJobWith(JobCondition.Incompletable);
						}
						else if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
						{
							this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <follow>__0;

			internal JobDriver_Follow $this;

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
				{
					this.FailOnDespawnedOrNull(TargetIndex.A);
					Toil follow = new Toil();
					follow.tickAction = delegate()
					{
						Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
						if (!this.pawn.Position.InHorDistOf(pawn.Position, 4f) || !this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
						{
							if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
							else if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
							{
								this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
							}
						}
					};
					follow.defaultCompleteMode = ToilCompleteMode.Never;
					this.$current = follow;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
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
				JobDriver_Follow.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Follow.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				if (!this.pawn.Position.InHorDistOf(pawn.Position, 4f) || !this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
				{
					if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
					else if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
					{
						this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
					}
				}
			}
		}
	}
}
