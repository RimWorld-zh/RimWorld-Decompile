using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000DE RID: 222
	public class JobGiver_DoLovin : ThinkNode_JobGiver
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x0003640C File Offset: 0x0003480C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
			{
				result = null;
			}
			else if (pawn.CurrentBed() == null || pawn.CurrentBed().Medical || !pawn.health.capacities.CanBeAwake)
			{
				result = null;
			}
			else
			{
				Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
				if (partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake || Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick)
				{
					result = null;
				}
				else if (!pawn.CanReserve(partnerInMyBed, 1, -1, null, false) || !partnerInMyBed.CanReserve(pawn, 1, -1, null, false))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Lovin, partnerInMyBed, pawn.CurrentBed());
				}
			}
			return result;
		}
	}
}
