using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001FE RID: 510
	public class ThoughtWorker_HardWorkerVsLazy : ThoughtWorker
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x00057FFC File Offset: 0x000563FC
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
				if (num > 0)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}
}
