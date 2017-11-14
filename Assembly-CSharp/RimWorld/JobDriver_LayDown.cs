using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_LayDown : JobDriver
	{
		private const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

		public Building_Bed Bed
		{
			get
			{
				return (Building_Bed)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			if (base.job.GetTarget(TargetIndex.A).HasThing && !base.pawn.Reserve(this.Bed, base.job, this.Bed.SleepingSlotsCount, 0, null))
			{
				return false;
			}
			return true;
		}

		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(base.pawn, base.job.GetTarget(TargetIndex.A));
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (base.job.GetTarget(TargetIndex.A).HasThing)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override string GetReport()
		{
			if (base.asleep)
			{
				return "ReportSleeping".Translate();
			}
			return "ReportResting".Translate();
		}
	}
}
