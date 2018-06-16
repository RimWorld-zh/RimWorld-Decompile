using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000099 RID: 153
	public static class JobInBedUtility
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x0002C9B8 File Offset: 0x0002ADB8
		public static void KeepLyingDown(this JobDriver driver, TargetIndex bedIndex)
		{
			driver.AddFinishAction(delegate
			{
				Pawn pawn = driver.pawn;
				if (!pawn.Drafted)
				{
					pawn.jobs.jobQueue.EnqueueFirst(new Job(JobDefOf.LayDown, pawn.CurJob.GetTarget(bedIndex)), null);
				}
			});
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0002C9F4 File Offset: 0x0002ADF4
		public static bool InBedOrRestSpotNow(Pawn pawn, LocalTargetInfo bedOrRestSpot)
		{
			bool result;
			if (!bedOrRestSpot.IsValid || !pawn.Spawned)
			{
				result = false;
			}
			else if (bedOrRestSpot.HasThing)
			{
				result = (bedOrRestSpot.Thing.Map == pawn.Map && RestUtility.GetBedSleepingSlotPosFor(pawn, (Building_Bed)bedOrRestSpot.Thing) == pawn.Position);
			}
			else
			{
				result = (bedOrRestSpot.Cell == pawn.Position);
			}
			return result;
		}
	}
}
