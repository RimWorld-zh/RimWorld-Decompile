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
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		public JobDriver_ExtinguishSelf()
		{
		}

		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		public override string GetReport()
		{
			string result;
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				result = "ReportExtinguishingFireOn".Translate(new object[]
				{
					this.TargetFire.parent.LabelCap
				});
			}
			else
			{
				result = "ReportExtinguishingFire".Translate();
			}
			return result;
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 150
			};
			Toil killFire = new Toil();
			killFire.initAction = delegate()
			{
				this.TargetFire.Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			};
			killFire.FailOnDestroyedOrNull(TargetIndex.A);
			killFire.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return killFire;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <extinguishDelay>__0;

			internal Toil <killFire>__0;

			internal JobDriver_ExtinguishSelf $this;

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
					Toil extinguishDelay = new Toil();
					extinguishDelay.defaultCompleteMode = ToilCompleteMode.Delay;
					extinguishDelay.defaultDuration = 150;
					this.$current = extinguishDelay;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					killFire = new Toil();
					killFire.initAction = delegate()
					{
						base.TargetFire.Destroy(DestroyMode.Vanish);
						this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
					};
					killFire.FailOnDestroyedOrNull(TargetIndex.A);
					killFire.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = killFire;
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
				JobDriver_ExtinguishSelf.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ExtinguishSelf.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				base.TargetFire.Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			}
		}
	}
}
