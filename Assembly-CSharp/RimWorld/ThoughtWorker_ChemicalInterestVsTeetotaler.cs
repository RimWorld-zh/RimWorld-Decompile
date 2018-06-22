using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000203 RID: 515
	public class ThoughtWorker_ChemicalInterestVsTeetotaler : ThoughtWorker
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x00058328 File Offset: 0x00056728
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (p.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
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
				if (num >= 0)
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
