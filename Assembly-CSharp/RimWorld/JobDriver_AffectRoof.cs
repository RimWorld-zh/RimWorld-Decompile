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
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		private float workLeft;

		private const TargetIndex CellInd = TargetIndex.A;

		private const TargetIndex GotoTargetInd = TargetIndex.B;

		private const float BaseWorkAmount = 65f;

		protected JobDriver_AffectRoof()
		{
		}

		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		protected abstract PathEndMode PathEndMode { get; }

		protected abstract void DoEffect();

		protected abstract bool DoWorkFailOn();

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		public override bool TryMakePreToilReservations()
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Cell;
			Job job = this.job;
			ReservationLayerDef ceiling = ReservationLayerDefOf.Ceiling;
			return pawn.Reserve(target, job, 1, -1, ceiling);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = 65f;
			};
			doWork.tickAction = delegate()
			{
				float statValue = doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				this.workLeft -= statValue;
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					this.ReadyForNextToil();
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
			doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
			doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / 65f, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			yield return doWork;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_AffectRoof $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_AffectRoof.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.FailOnDespawnedOrNull(TargetIndex.B);
					this.$current = Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.doWork = new Toil();
					<MakeNewToils>c__AnonStorey.doWork.initAction = delegate()
					{
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft = 65f;
					};
					<MakeNewToils>c__AnonStorey.doWork.tickAction = delegate()
					{
						float statValue = <MakeNewToils>c__AnonStorey.doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft -= statValue;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft <= 0f)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.DoEffect();
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
						}
					};
					<MakeNewToils>c__AnonStorey.doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
					<MakeNewToils>c__AnonStorey.doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
					<MakeNewToils>c__AnonStorey.doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
					<MakeNewToils>c__AnonStorey.doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A);
					<MakeNewToils>c__AnonStorey.doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
					<MakeNewToils>c__AnonStorey.doWork.WithProgressBar(TargetIndex.A, () => 1f - <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.workLeft / 65f, false, -0.5f);
					<MakeNewToils>c__AnonStorey.doWork.defaultCompleteMode = ToilCompleteMode.Never;
					this.$current = <MakeNewToils>c__AnonStorey.doWork;
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
				JobDriver_AffectRoof.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_AffectRoof.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil doWork;

				internal JobDriver_AffectRoof.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.workLeft = 65f;
				}

				internal void <>m__1()
				{
					float statValue = this.doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					this.<>f__ref$0.$this.workLeft -= statValue;
					if (this.<>f__ref$0.$this.workLeft <= 0f)
					{
						this.<>f__ref$0.$this.DoEffect();
						this.<>f__ref$0.$this.ReadyForNextToil();
					}
				}

				internal float <>m__2()
				{
					return 1f - this.<>f__ref$0.$this.workLeft / 65f;
				}
			}
		}
	}
}
