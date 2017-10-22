using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			return (pawn.equipment == null || !pawn.equipment.HasAnything()) ? ((pawn.apparel == null || !pawn.apparel.WornApparel.Any()) ? null : new Job(JobDefOf.RemoveApparel, (Thing)pawn.apparel.WornApparel.RandomElement())) : new Job(JobDefOf.DropEquipment, (Thing)pawn.equipment.AllEquipmentListForReading.RandomElement());
		}
	}
}
