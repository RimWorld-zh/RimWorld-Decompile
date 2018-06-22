using System;

namespace RimWorld
{
	// Token: 0x02000202 RID: 514
	public class Thought_TeetotalerVsChemicalInterest : Thought_SituationalSocial
	{
		// Token: 0x060009D0 RID: 2512 RVA: 0x000582C4 File Offset: 0x000566C4
		public override float OpinionOffset()
		{
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			float result;
			if (num <= 0)
			{
				result = 0f;
			}
			else if (num == 1)
			{
				result = -20f;
			}
			else
			{
				result = -30f;
			}
			return result;
		}
	}
}
