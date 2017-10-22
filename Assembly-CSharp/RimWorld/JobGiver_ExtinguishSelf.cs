using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		private const float ActivateChance = 0.1f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.10000000149011612)
			{
				Fire fire = (Fire)pawn.GetAttachment(ThingDefOf.Fire);
				if (fire != null)
				{
					return new Job(JobDefOf.ExtinguishSelf, (Thing)fire);
				}
			}
			return null;
		}
	}
}
