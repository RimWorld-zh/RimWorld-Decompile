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
	public class JobDriver_VisitSickPawn : JobDriver
	{
		private const TargetIndex PatientInd = TargetIndex.A;

		private const TargetIndex ChairInd = TargetIndex.B;

		public JobDriver_VisitSickPawn()
		{
		}

		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Patient;
			Job job = this.job;
			if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (this.Chair != null)
			{
				pawn = this.pawn;
				target = this.Chair;
				job = this.job;
				if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.Patient.InBed() || !this.Patient.Awake());
			if (this.Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			if (this.Chair != null)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			}
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return new Toil
			{
				tickAction = delegate()
				{
					this.Patient.needs.joy.GainJoy(this.job.def.joyGainRate * 0.000144f, this.job.def.joyKind);
					if (this.pawn.IsHashIntervalTick(320))
					{
						InteractionDef intDef = (Rand.Value >= 0.8f) ? InteractionDefOf.DeepTalk : InteractionDefOf.Chitchat;
						this.pawn.interactions.TryInteractWith(this.Patient, intDef);
					}
					this.pawn.rotationTracker.FaceCell(this.Patient.Position);
					this.pawn.GainComfortFromCellIfPossible();
					JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
					if (this.pawn.needs.joy.CurLevelPercentage > 0.9999f && this.Patient.needs.joy.CurLevelPercentage > 0.9999f)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				},
				handlingFacing = true,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = this.job.def.joyDuration
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <waitAndTalk>__0;

			internal JobDriver_VisitSickPawn $this;

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
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOn(() => !base.Patient.InBed() || !base.Patient.Awake());
					if (base.Chair != null)
					{
						this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
					}
					if (base.Chair != null)
					{
						this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 1u:
					break;
				case 2u:
					break;
				case 3u:
				{
					Toil waitAndTalk = new Toil();
					waitAndTalk.tickAction = delegate()
					{
						base.Patient.needs.joy.GainJoy(this.job.def.joyGainRate * 0.000144f, this.job.def.joyKind);
						if (this.pawn.IsHashIntervalTick(320))
						{
							InteractionDef intDef = (Rand.Value >= 0.8f) ? InteractionDefOf.DeepTalk : InteractionDefOf.Chitchat;
							this.pawn.interactions.TryInteractWith(base.Patient, intDef);
						}
						this.pawn.rotationTracker.FaceCell(base.Patient.Position);
						this.pawn.GainComfortFromCellIfPossible();
						JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
						if (this.pawn.needs.joy.CurLevelPercentage > 0.9999f && base.Patient.needs.joy.CurLevelPercentage > 0.9999f)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					};
					waitAndTalk.handlingFacing = true;
					waitAndTalk.socialMode = RandomSocialMode.Off;
					waitAndTalk.defaultCompleteMode = ToilCompleteMode.Delay;
					waitAndTalk.defaultDuration = this.job.def.joyDuration;
					this.$current = waitAndTalk;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				case 4u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
				if (!this.$disposing)
				{
					this.$PC = 3;
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
				JobDriver_VisitSickPawn.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_VisitSickPawn.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Patient.InBed() || !base.Patient.Awake();
			}

			internal void <>m__1()
			{
				base.Patient.needs.joy.GainJoy(this.job.def.joyGainRate * 0.000144f, this.job.def.joyKind);
				if (this.pawn.IsHashIntervalTick(320))
				{
					InteractionDef intDef = (Rand.Value >= 0.8f) ? InteractionDefOf.DeepTalk : InteractionDefOf.Chitchat;
					this.pawn.interactions.TryInteractWith(base.Patient, intDef);
				}
				this.pawn.rotationTracker.FaceCell(base.Patient.Position);
				this.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
				if (this.pawn.needs.joy.CurLevelPercentage > 0.9999f && base.Patient.needs.joy.CurLevelPercentage > 0.9999f)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
			}
		}
	}
}
