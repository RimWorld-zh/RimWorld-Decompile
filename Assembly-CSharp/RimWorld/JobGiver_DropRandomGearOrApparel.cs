using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		public JobGiver_DropRandomGearOrApparel()
		{
		}

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
