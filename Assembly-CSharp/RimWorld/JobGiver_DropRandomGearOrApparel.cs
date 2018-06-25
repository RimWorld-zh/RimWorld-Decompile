using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200010B RID: 267
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0003C154 File Offset: 0x0003A554
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.equipment != null && pawn.equipment.HasAnything())
			{
				result = new Job(JobDefOf.DropEquipment, pawn.equipment.AllEquipmentListForReading.RandomElement<ThingWithComps>());
			}
			else if (pawn.apparel != null && pawn.apparel.WornApparel.Any<Apparel>())
			{
				result = new Job(JobDefOf.RemoveApparel, pawn.apparel.WornApparel.RandomElement<Apparel>());
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
