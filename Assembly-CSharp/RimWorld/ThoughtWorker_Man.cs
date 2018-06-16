using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020A RID: 522
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x0005878C File Offset: 0x00056B8C
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
