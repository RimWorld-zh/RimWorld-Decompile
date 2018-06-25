using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_BodyPuristDisgust : ThoughtWorker
	{
		public ThoughtWorker_BodyPuristDisgust()
		{
		}

		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.story.traits.HasTrait(TraitDefOf.BodyPurist))
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
			else
			{
				int num = other.health.hediffSet.CountAddedParts();
				if (num > 0)
				{
					result = ThoughtState.ActiveAtStage(num - 1);
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
