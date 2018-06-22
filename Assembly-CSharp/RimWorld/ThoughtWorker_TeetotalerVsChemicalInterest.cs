using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000200 RID: 512
	public class ThoughtWorker_TeetotalerVsChemicalInterest : ThoughtWorker
	{
		// Token: 0x060009CC RID: 2508 RVA: 0x00058138 File Offset: 0x00056538
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.IsTeetotaler())
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
				int num = other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
				if (num <= 0)
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
