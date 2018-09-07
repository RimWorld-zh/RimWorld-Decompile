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
	public class JobDriver_Execute : JobDriver
	{
		public JobDriver_Execute()
		{
		}

		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Victim;
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !this.Victim.IsPrisonerOfColony || !this.Victim.guest.PrisonerIsSecure);
			Toil execute = new Toil();
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, PawnExecutionKind.GenericBrutal);
				TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
				{
					this.pawn,
					this.Victim
				});
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return execute;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Execute $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Execute.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnAggroMentalState(TargetIndex.A);
					this.$current = Toils_Interpersonal.GotoPrisoner(this.pawn, base.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !this.Victim.IsPrisonerOfColony || !this.Victim.guest.PrisonerIsSecure);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.execute = new Toil();
					<MakeNewToils>c__AnonStorey.execute.initAction = delegate()
					{
						ExecutionUtility.DoExecutionByCut(<MakeNewToils>c__AnonStorey.execute.actor, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Victim);
						ThoughtUtility.GiveThoughtsForPawnExecuted(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Victim, PawnExecutionKind.GenericBrutal);
						TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn,
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Victim
						});
					};
					<MakeNewToils>c__AnonStorey.execute.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = <MakeNewToils>c__AnonStorey.execute;
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
				JobDriver_Execute.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Execute.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil execute;

				internal JobDriver_Execute.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					return !this.<>f__ref$0.$this.Victim.IsPrisonerOfColony || !this.<>f__ref$0.$this.Victim.guest.PrisonerIsSecure;
				}

				internal void <>m__1()
				{
					ExecutionUtility.DoExecutionByCut(this.execute.actor, this.<>f__ref$0.$this.Victim);
					ThoughtUtility.GiveThoughtsForPawnExecuted(this.<>f__ref$0.$this.Victim, PawnExecutionKind.GenericBrutal);
					TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
					{
						this.<>f__ref$0.$this.pawn,
						this.<>f__ref$0.$this.Victim
					});
				}
			}
		}
	}
}
