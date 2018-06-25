using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020C RID: 524
	public class ThoughtWorker_TranshumanistAppreciation : ThoughtWorker
	{
		// Token: 0x060009E3 RID: 2531 RVA: 0x00058934 File Offset: 0x00056D34
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
