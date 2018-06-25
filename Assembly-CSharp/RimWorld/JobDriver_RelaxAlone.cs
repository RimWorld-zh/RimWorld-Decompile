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
	public class JobDriver_RelaxAlone : JobDriver
	{
		private Rot4 faceDir = Rot4.Invalid;

		private const TargetIndex SpotOrBedInd = TargetIndex.A;

		public JobDriver_RelaxAlone()
		{
		}

		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).HasThing;
			}
		}

		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		public override bool TryMakePreToilReservations()
		{
			if (this.FromBed)
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, ((Building_Bed)this.job.GetTarget(TargetIndex.A).Thing).SleepingSlotsCount, 0, null))
				{
					return false;
				}
			}
			else if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null))
			{
				return false;
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil relax;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.A);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
				relax = Toils_LayDown.LayDown(TargetIndex.A, true, false, true, true);
				relax.AddFailCondition(() => !this.pawn.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				relax = new Toil();
				relax.initAction = delegate()
				{
					this.faceDir = ((!this.job.def.faceDir.IsValid) ? Rot4.Random : this.job.def.faceDir);
				};
				relax.handlingFacing = true;
			}
			relax.defaultCompleteMode = ToilCompleteMode.Delay;
			relax.defaultDuration = this.job.def.joyDuration;
			relax.AddPreTickAction(delegate
			{
				if (this.faceDir.IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir.FacingCell);
				}
				this.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			});
			yield return relax;
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <relax>__1;

			internal JobDriver_RelaxAlone $this;

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
					if (base.FromBed)
					{
						this.KeepLyingDown(TargetIndex.A);
						this.$current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 1u:
					this.$current = Toils_Bed.GotoBed(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					relax = Toils_LayDown.LayDown(TargetIndex.A, true, false, true, true);
					relax.AddFailCondition(() => !this.pawn.Awake());
					break;
				case 3u:
					relax = new Toil();
					relax.initAction = delegate()
					{
						this.faceDir = ((!this.job.def.faceDir.IsValid) ? Rot4.Random : this.job.def.faceDir);
					};
					relax.handlingFacing = true;
					break;
				case 4u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				relax.defaultCompleteMode = ToilCompleteMode.Delay;
				relax.defaultDuration = this.job.def.joyDuration;
				relax.AddPreTickAction(delegate
				{
					if (this.faceDir.IsValid)
					{
						this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir.FacingCell);
					}
					this.pawn.GainComfortFromCellIfPossible();
					JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
				});
				this.$current = relax;
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
				JobDriver_RelaxAlone.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_RelaxAlone.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !this.pawn.Awake();
			}

			internal void <>m__1()
			{
				this.faceDir = ((!this.job.def.faceDir.IsValid) ? Rot4.Random : this.job.def.faceDir);
			}

			internal void <>m__2()
			{
				if (this.faceDir.IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir.FacingCell);
				}
				this.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			}
		}
	}
}
