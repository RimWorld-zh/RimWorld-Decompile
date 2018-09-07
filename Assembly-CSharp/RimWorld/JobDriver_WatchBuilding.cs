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
	public class JobDriver_WatchBuilding : JobDriver
	{
		public JobDriver_WatchBuilding()
		{
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.job.targetA;
			Job job = this.job;
			int num = this.job.def.joyMaxParticipants;
			int num2 = 0;
			if (!pawn.Reserve(target, job, num, num2, null, errorOnFailed))
			{
				return false;
			}
			pawn = this.pawn;
			target = this.job.targetB;
			job = this.job;
			if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (base.TargetC.HasThing)
			{
				if (base.TargetC.Thing is Building_Bed)
				{
					pawn = this.pawn;
					LocalTargetInfo targetC = this.job.targetC;
					job = this.job;
					num2 = ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount;
					num = 0;
					if (!pawn.Reserve(targetC, job, num2, num, null, errorOnFailed))
					{
						return false;
					}
				}
				else
				{
					pawn = this.pawn;
					LocalTargetInfo targetC = this.job.targetC;
					job = this.job;
					if (!pawn.Reserve(targetC, job, 1, -1, null, errorOnFailed))
					{
						return false;
					}
				}
			}
			return true;
		}

		public override bool CanBeginNowWhileLyingDown()
		{
			return base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, base.TargetC);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			bool hasBed = base.TargetC.HasThing && base.TargetC.Thing is Building_Bed;
			Toil watch;
			if (hasBed)
			{
				this.KeepLyingDown(TargetIndex.C);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.C);
				watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true);
				watch.AddFailCondition(() => !watch.actor.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				watch = new Toil();
			}
			watch.AddPreTickAction(delegate
			{
				this.WatchTickAction();
			});
			watch.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			watch.defaultCompleteMode = ToilCompleteMode.Delay;
			watch.defaultDuration = this.job.def.joyDuration;
			watch.handlingFacing = true;
			yield return watch;
			yield break;
		}

		protected virtual void WatchTickAction()
		{
			this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			this.pawn.GainComfortFromCellIfPossible();
			Pawn pawn = this.pawn;
			Building joySource = (Building)base.TargetThingA;
			JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
		}

		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal bool <hasBed>__0;

			internal JobDriver_WatchBuilding $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_WatchBuilding.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
					hasBed = (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed);
					if (hasBed)
					{
						this.KeepLyingDown(TargetIndex.C);
						this.$current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					this.$current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 1u:
					this.$current = Toils_Bed.GotoBed(TargetIndex.C);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					<MakeNewToils>c__AnonStorey.watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true);
					<MakeNewToils>c__AnonStorey.watch.AddFailCondition(() => !<MakeNewToils>c__AnonStorey.watch.actor.Awake());
					break;
				case 3u:
					<MakeNewToils>c__AnonStorey.watch = new Toil();
					break;
				case 4u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				<MakeNewToils>c__AnonStorey.watch.AddPreTickAction(delegate
				{
					<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.WatchTickAction();
				});
				<MakeNewToils>c__AnonStorey.watch.AddFinishAction(delegate
				{
					JoyUtility.TryGainRecRoomThought(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn);
				});
				<MakeNewToils>c__AnonStorey.watch.defaultCompleteMode = ToilCompleteMode.Delay;
				<MakeNewToils>c__AnonStorey.watch.defaultDuration = this.job.def.joyDuration;
				<MakeNewToils>c__AnonStorey.watch.handlingFacing = true;
				this.$current = <MakeNewToils>c__AnonStorey.watch;
				if (!this.$disposing)
				{
					this.$PC = 4;
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
				JobDriver_WatchBuilding.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_WatchBuilding.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil watch;

				internal JobDriver_WatchBuilding.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					return !this.watch.actor.Awake();
				}

				internal void <>m__1()
				{
					this.<>f__ref$0.$this.WatchTickAction();
				}

				internal void <>m__2()
				{
					JoyUtility.TryGainRecRoomThought(this.<>f__ref$0.$this.pawn);
				}
			}
		}
	}
}
