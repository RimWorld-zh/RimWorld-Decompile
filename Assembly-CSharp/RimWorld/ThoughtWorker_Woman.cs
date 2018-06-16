using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020B RID: 523
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		// Token: 0x060009E4 RID: 2532 RVA: 0x00058840 File Offset: 0x00056C40
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.story.traits.HasTrait(TraitDefOf.DislikesWomen))
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
			else if (other.gender != Gender.Female)
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
