using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000059 RID: 89
	public class JobDriver_RelaxAlone : JobDriver
	{
		// Token: 0x040001F7 RID: 503
		private Rot4 faceDir = Rot4.Invalid;

		// Token: 0x040001F8 RID: 504
		private const TargetIndex SpotOrBedInd = TargetIndex.A;

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0001C910 File Offset: 0x0001AD10
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).HasThing;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0001C93C File Offset: 0x0001AD3C
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0001C978 File Offset: 0x0001AD78
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

		// Token: 0x060002A2 RID: 674 RVA: 0x0001CA18 File Offset: 0x0001AE18
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

		// Token: 0x060002A3 RID: 675 RVA: 0x0001CA44 File Offset: 0x0001AE44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}
	}
}
