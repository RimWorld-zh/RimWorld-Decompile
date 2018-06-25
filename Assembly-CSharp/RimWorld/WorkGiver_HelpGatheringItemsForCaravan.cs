using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200014B RID: 331
	public class WorkGiver_HelpGatheringItemsForCaravan : WorkGiver
	{
		// Token: 0x060006D5 RID: 1749 RVA: 0x000461E0 File Offset: 0x000445E0
		public override Job NonScanJob(Pawn pawn)
		{
			List<Lord> lords = pawn.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
				{
					Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lords[i]);
					if (thing != null)
					{
						if (this.AnyReachableCarrierOrColonist(pawn, lords[i]))
						{
							return new Job(JobDefOf.PrepareCaravan_GatherItems, thing)
							{
								lord = lords[i]
							};
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000462A0 File Offset: 0x000446A0
		private bool AnyReachableCarrierOrColonist(Pawn forPawn, Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(lord.ownedPawns[i], forPawn, false))
				{
					if (!lord.ownedPawns[i].IsForbidden(forPawn))
					{
						if (forPawn.CanReach(lord.ownedPawns[i], PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
