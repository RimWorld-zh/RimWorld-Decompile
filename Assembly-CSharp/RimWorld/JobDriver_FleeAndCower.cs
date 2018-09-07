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
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		private const int CowerTicks = 1200;

		private const int CheckFleeAgainIntervalTicks = 35;

		public JobDriver_FleeAndCower()
		{
		}

		public override string GetReport()
		{
			if (this.pawn.CurJob != this.job || this.pawn.Position != this.job.GetTarget(TargetIndex.A).Cell)
			{
				return base.GetReport();
			}
			return "ReportCowering".Translate();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 1200,
				tickAction = delegate()
				{
					if (this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
					{
						base.EndJobWith(JobCondition.InterruptForced);
					}
				}
			};
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Toil> <MakeNewToils>__BaseCallProxy0()
		{
			return base.MakeNewToils();
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal IEnumerator<Toil> $locvar0;

			internal Toil <toil>__1;

			internal Toil <cower>__2;

			internal JobDriver_FleeAndCower $this;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<MakeNewToils>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						toil = enumerator.Current;
						this.$current = toil;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				Toil cower = new Toil();
				cower.defaultCompleteMode = ToilCompleteMode.Delay;
				cower.defaultDuration = 1200;
				cower.tickAction = delegate()
				{
					if (this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
					{
						base.EndJobWith(JobCondition.InterruptForced);
					}
				};
				this.$current = cower;
				if (!this.$disposing)
				{
					this.$PC = 2;
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
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
				JobDriver_FleeAndCower.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_FleeAndCower.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				if (this.pawn.IsHashIntervalTick(35) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
				{
					base.EndJobWith(JobCondition.InterruptForced);
				}
			}
		}
	}
}
