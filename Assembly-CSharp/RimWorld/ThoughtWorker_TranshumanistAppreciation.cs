using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020C RID: 524
	public class ThoughtWorker_TranshumanistAppreciation : ThoughtWorker
	{
		// Token: 0x060009E4 RID: 2532 RVA: 0x00058938 File Offset: 0x00056D38
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.story.traits.HasTrait(TraitDefOf.Transhumanist))
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
