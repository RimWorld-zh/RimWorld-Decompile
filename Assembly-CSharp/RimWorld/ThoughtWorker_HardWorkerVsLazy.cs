using Verse;

namespace RimWorld
{
	public class ThoughtWorker_HardWorkerVsLazy : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (p.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) <= 0)
			{
				result = false;
			}
			else if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				result = false;
			}
			else
			{
				int num = other.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
				result = ((num <= 0) ? true : false);
			}
			return result;
		}
	}
}
