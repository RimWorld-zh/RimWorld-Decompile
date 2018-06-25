using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000203 RID: 515
	public class ThoughtWorker_ChemicalInterestVsTeetotaler : ThoughtWorker
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x00058324 File Offset: 0x00056724
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
