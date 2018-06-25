using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;

namespace Verse.AI
{
	public class JobDriver_Goto : JobDriver
	{
		public JobDriver_Goto()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			gotoCell.AddPreTickAction(delegate
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.TryExitMap();
				}
			});
			gotoCell.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			yield return gotoCell;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == base.TargetA.Cell)
					{
						this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
					}
					if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
					{
						this.TryExitMap();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		private void TryExitMap()
		{
			if (!this.job.failIfCantJoinOrCreateCaravan || CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoCell>__1;

			internal Toil <arrive>__2;

			internal JobDriver_Goto $this;

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
					gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
					gotoCell.AddPreTickAction(delegate
					{
						if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
						{
							base.TryExitMap();
						}
					});
					gotoCell.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
					this.$current = gotoCell;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil arrive = new Toil();
					arrive.initAction = delegate()
					{
						if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == base.TargetA.Cell)
						{
							this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
						}
						if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
						{
							base.TryExitMap();
						}
					};
					arrive.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = arrive;
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
				JobDriver_Goto.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Goto.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					base.TryExitMap();
				}
			}

			internal bool <>m__1()
			{
				return this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn);
			}

			internal void <>m__2()
			{
				if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == base.TargetA.Cell)
				{
					this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				}
				if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
				{
					base.TryExitMap();
				}
			}
		}
	}
}
