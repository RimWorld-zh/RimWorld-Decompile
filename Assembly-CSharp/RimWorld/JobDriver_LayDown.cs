using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000098 RID: 152
	public class JobDriver_LayDown : JobDriver
	{
		// Token: 0x04000261 RID: 609
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0002C6BC File Offset: 0x0002AABC
		public Building_Bed Bed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0002C6EC File Offset: 0x0002AAEC
		public override bool TryMakePreToilReservations()
		{
			bool hasThing = this.job.GetTarget(TargetIndex.A).HasThing;
			if (hasThing)
			{
				if (!this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0002C754 File Offset: 0x0002AB54
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0002C780 File Offset: 0x0002AB80
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.job.GetTarget(TargetIndex.A).HasThing;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			}
			yield return Toils_LayDown.LayDown(TargetIndex.A, hasBed, true, true, true);
			yield break;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0002C7AC File Offset: 0x0002ABAC
		public override string GetReport()
		{
			string result;
			if (this.asleep)
			{
				result = "ReportSleeping".Translate();
			}
			else
			{
				result = "ReportResting".Translate();
			}
			return result;
		}
	}
}
