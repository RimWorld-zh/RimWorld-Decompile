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
	public class JobDriver_BeatFire : JobDriver
	{
		public JobDriver_BeatFire()
		{
		}

		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil beat = new Toil();
			Toil approach = new Toil();
			approach.initAction = delegate()
			{
				if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetFire, 1, -1, null, false))
				{
					this.pawn.Reserve(this.TargetFire, this.job, 1, -1, null, true);
				}
				this.pawn.pather.StartPath(this.TargetFire, PathEndMode.Touch);
			};
			approach.tickAction = delegate()
			{
				if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.pather.nextCell, beat);
				}
				if (this.pawn.Position != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.Position, beat);
				}
			};
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = delegate()
			{
				if (!this.pawn.CanReachImmediate(this.TargetFire, PathEndMode.Touch))
				{
					this.JumpToToil(approach);
				}
				else
				{
					if (this.pawn.Position != this.TargetFire.Position && this.StartBeatingFireIfAnyAt(this.pawn.Position, beat))
					{
						return;
					}
					this.pawn.natives.TryBeatFire(this.TargetFire);
					if (this.TargetFire.Destroyed)
					{
						this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						return;
					}
				}
			};
			beat.FailOnDespawnedOrNull(TargetIndex.A);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
			yield break;
		}

		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					this.job.targetA = fire;
					this.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_BeatFire $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_BeatFire.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					Toil beat = new Toil();
					Toil approach = new Toil();
					approach.initAction = delegate()
					{
						if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetFire, 1, -1, null, false))
						{
							this.pawn.Reserve(this.TargetFire, this.job, 1, -1, null, true);
						}
						this.pawn.pather.StartPath(this.TargetFire, PathEndMode.Touch);
					};
					approach.tickAction = delegate()
					{
						if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetFire.Position)
						{
							this.StartBeatingFireIfAnyAt(this.pawn.pather.nextCell, beat);
						}
						if (this.pawn.Position != this.TargetFire.Position)
						{
							this.StartBeatingFireIfAnyAt(this.pawn.Position, beat);
						}
					};
					approach.FailOnDespawnedOrNull(TargetIndex.A);
					approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
					approach.atomicWithPrevious = true;
					this.$current = approach;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					<MakeNewToils>c__AnonStorey.beat.tickAction = delegate()
					{
						if (!<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.CanReachImmediate(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetFire, PathEndMode.Touch))
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.JumpToToil(<MakeNewToils>c__AnonStorey.approach);
						}
						else
						{
							if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Position != <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetFire.Position && <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.StartBeatingFireIfAnyAt(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Position, <MakeNewToils>c__AnonStorey.beat))
							{
								return;
							}
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.natives.TryBeatFire(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetFire);
							if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetFire.Destroyed)
							{
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
								return;
							}
						}
					};
					<MakeNewToils>c__AnonStorey.beat.FailOnDespawnedOrNull(TargetIndex.A);
					<MakeNewToils>c__AnonStorey.beat.defaultCompleteMode = ToilCompleteMode.Never;
					this.$current = <MakeNewToils>c__AnonStorey.beat;
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
				JobDriver_BeatFire.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_BeatFire.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil beat;

				internal Toil approach;

				internal JobDriver_BeatFire.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					if (this.<>f__ref$0.$this.Map.reservationManager.CanReserve(this.<>f__ref$0.$this.pawn, this.<>f__ref$0.$this.TargetFire, 1, -1, null, false))
					{
						this.<>f__ref$0.$this.pawn.Reserve(this.<>f__ref$0.$this.TargetFire, this.<>f__ref$0.$this.job, 1, -1, null, true);
					}
					this.<>f__ref$0.$this.pawn.pather.StartPath(this.<>f__ref$0.$this.TargetFire, PathEndMode.Touch);
				}

				internal void <>m__1()
				{
					if (this.<>f__ref$0.$this.pawn.pather.Moving && this.<>f__ref$0.$this.pawn.pather.nextCell != this.<>f__ref$0.$this.TargetFire.Position)
					{
						this.<>f__ref$0.$this.StartBeatingFireIfAnyAt(this.<>f__ref$0.$this.pawn.pather.nextCell, this.beat);
					}
					if (this.<>f__ref$0.$this.pawn.Position != this.<>f__ref$0.$this.TargetFire.Position)
					{
						this.<>f__ref$0.$this.StartBeatingFireIfAnyAt(this.<>f__ref$0.$this.pawn.Position, this.beat);
					}
				}

				internal void <>m__2()
				{
					if (!this.<>f__ref$0.$this.pawn.CanReachImmediate(this.<>f__ref$0.$this.TargetFire, PathEndMode.Touch))
					{
						this.<>f__ref$0.$this.JumpToToil(this.approach);
					}
					else
					{
						if (this.<>f__ref$0.$this.pawn.Position != this.<>f__ref$0.$this.TargetFire.Position && this.<>f__ref$0.$this.StartBeatingFireIfAnyAt(this.<>f__ref$0.$this.pawn.Position, this.beat))
						{
							return;
						}
						this.<>f__ref$0.$this.pawn.natives.TryBeatFire(this.<>f__ref$0.$this.TargetFire);
						if (this.<>f__ref$0.$this.TargetFire.Destroyed)
						{
							this.<>f__ref$0.$this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
							this.<>f__ref$0.$this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
							return;
						}
					}
				}
			}
		}
	}
}
