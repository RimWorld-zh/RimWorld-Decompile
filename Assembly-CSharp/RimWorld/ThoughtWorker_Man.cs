using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020A RID: 522
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x000587D0 File Offset: 0x00056BD0
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
