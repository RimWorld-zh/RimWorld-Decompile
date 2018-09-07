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
	public class JobDriver_ClearSnow : JobDriver
	{
		private float workDone;

		private const float ClearWorkPerSnowDepth = 50f;

		public JobDriver_ClearSnow()
		{
		}

		private float TotalNeededWork
		{
			get
			{
				return 50f * base.Map.snowGrid.GetDepth(base.TargetLocA);
			}
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
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil clearToil = new Toil();
			clearToil.tickAction = delegate()
			{
				Pawn actor = clearToil.actor;
				float statValue = actor.GetStatValue(StatDefOf.UnskilledLaborSpeed, true);
				float num = statValue;
				this.workDone += num;
				if (this.workDone >= this.TotalNeededWork)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
					this.ReadyForNextToil();
					return;
				}
			};
			clearToil.defaultCompleteMode = ToilCompleteMode.Never;
			clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A);
			clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
			clearToil.WithProgressBar(TargetIndex.A, () => this.workDone / this.TotalNeededWork, true, -0.5f);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_ClearSnow $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_ClearSnow.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.clearToil = new Toil();
					<MakeNewToils>c__AnonStorey.clearToil.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.clearToil.actor;
						float statValue = actor.GetStatValue(StatDefOf.UnskilledLaborSpeed, true);
						float num2 = statValue;
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone += num2;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone >= <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TotalNeededWork)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.snowGrid.SetDepth(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TargetLocA, 0f);
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
							return;
						}
					};
					<MakeNewToils>c__AnonStorey.clearToil.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
					<MakeNewToils>c__AnonStorey.clearToil.WithProgressBar(TargetIndex.A, () => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workDone / <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.TotalNeededWork, true, -0.5f);
					<MakeNewToils>c__AnonStorey.clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					this.$current = <MakeNewToils>c__AnonStorey.clearToil;
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
				JobDriver_ClearSnow.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_ClearSnow.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SoundDef <>m__0()
			{
				return SoundDefOf.Interact_CleanFilth;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil clearToil;

				internal JobDriver_ClearSnow.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.clearToil.actor;
					float statValue = actor.GetStatValue(StatDefOf.UnskilledLaborSpeed, true);
					float num = statValue;
					this.<>f__ref$0.$this.workDone += num;
					if (this.<>f__ref$0.$this.workDone >= this.<>f__ref$0.$this.TotalNeededWork)
					{
						this.<>f__ref$0.$this.Map.snowGrid.SetDepth(this.<>f__ref$0.$this.TargetLocA, 0f);
						this.<>f__ref$0.$this.ReadyForNextToil();
						return;
					}
				}

				internal float <>m__1()
				{
					return this.<>f__ref$0.$this.workDone / this.<>f__ref$0.$this.TotalNeededWork;
				}
			}
		}
	}
}
