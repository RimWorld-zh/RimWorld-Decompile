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
	public class JobDriver_GoForWalk : JobDriver
	{
		public JobDriver_GoForWalk()
		{
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickAction = delegate()
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			yield return goToil;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = this.job.targetQueueA[0];
						this.job.targetQueueA.RemoveAt(0);
						this.job.targetA = targetA;
						this.JumpToToil(goToil);
						return;
					}
				}
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <getNextDest>__0;

			internal JobDriver_GoForWalk $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_GoForWalk.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
					Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
					goToil.tickAction = delegate()
					{
						if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
						{
							this.EndJobWith(JobCondition.Succeeded);
							return;
						}
						JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
					};
					this.$current = goToil;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
				{
					Toil getNextDest = new Toil();
					getNextDest.initAction = delegate()
					{
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.targetQueueA.Count > 0)
						{
							LocalTargetInfo targetA = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.targetQueueA[0];
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.targetQueueA.RemoveAt(0);
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.targetA = targetA;
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.JumpToToil(<MakeNewToils>c__AnonStorey.goToil);
							return;
						}
					};
					this.$current = getNextDest;
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
				JobDriver_GoForWalk.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_GoForWalk.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil goToil;

				internal JobDriver_GoForWalk.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					return !JoyUtility.EnjoyableOutsideNow(this.<>f__ref$0.$this.pawn, null);
				}

				internal void <>m__1()
				{
					if (Find.TickManager.TicksGame > this.<>f__ref$0.$this.startTick + this.<>f__ref$0.$this.job.def.joyDuration)
					{
						this.<>f__ref$0.$this.EndJobWith(JobCondition.Succeeded);
						return;
					}
					JoyUtility.JoyTickCheckEnd(this.<>f__ref$0.$this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
				}

				internal void <>m__2()
				{
					if (this.<>f__ref$0.$this.job.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = this.<>f__ref$0.$this.job.targetQueueA[0];
						this.<>f__ref$0.$this.job.targetQueueA.RemoveAt(0);
						this.<>f__ref$0.$this.job.targetA = targetA;
						this.<>f__ref$0.$this.JumpToToil(this.goToil);
						return;
					}
				}
			}
		}
	}
}
