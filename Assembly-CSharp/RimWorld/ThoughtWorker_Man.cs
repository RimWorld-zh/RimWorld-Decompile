using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Man : ThoughtWorker
	{
		public ThoughtWorker_Man()
		{
		}

		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.story.traits.HasTrait(TraitDefOf.DislikesMen))
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				result = false;
			}
			else if (other.def != p.def)
			{
				result = false;
			}
			else if (other.gender != Gender.Male)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
