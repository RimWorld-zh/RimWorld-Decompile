using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000060 RID: 96
	public class JobDriver_VisitSickPawn : JobDriver
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0001D998 File Offset: 0x0001BD98
		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0001D9C8 File Offset: 0x0001BDC8
		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0001D9F4 File Offset: 0x0001BDF4
		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!this.pawn.Reserve(this.Patient, this.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (this.Chair != null)
				{
					if (!this.pawn.Reserve(this.Chair, this.job, 1, -1, null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001DA70 File Offset: 0x0001BE70
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

		// Token: 0x040001FD RID: 509
		private const TargetIndex PatientInd = TargetIndex.A;

		// Token: 0x040001FE RID: 510
		private const TargetIndex ChairInd = TargetIndex.B;
	}
}
