using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Pretty : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				result = false;
			}
			else if (RelationsUtility.IsDisfigured(other))
			{
				result = false;
			}
			else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				result = false;
			}
			else
			{
				switch (other.story.traits.DegreeOfTrait(TraitDefOf.Beauty))
				{
				case 1:
				{
					result = ThoughtState.ActiveAtStage(0);
					break;
				}
				case 2:
				{
					result = ThoughtState.ActiveAtStage(1);
					break;
				}
				default:
				{
					result = false;
					break;
				}
				}
			}
			return result;
		}
	}
}
