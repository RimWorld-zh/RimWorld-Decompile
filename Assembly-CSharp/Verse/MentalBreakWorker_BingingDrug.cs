using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public class MentalBreakWorker_BingingDrug : MentalBreakWorker
	{
		public override float CommonalityFor(Pawn pawn)
		{
			int num = this.BingeableAddictionsCount(pawn);
			float num2 = (float)((num != 0) ? (base.def.baseCommonality * 1.3999999761581421 * (float)num) : (base.def.baseCommonality * 1.0));
			if (pawn.story != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				if (trait != null)
				{
					if (trait.Degree == 1)
					{
						num2 = (float)(num2 * 2.5);
					}
					else if (trait.Degree == 2)
					{
						num2 = (float)(num2 * 5.0);
					}
				}
			}
			return num2;
		}

		private int BingeableAddictionsCount(Pawn pawn)
		{
			int num = 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					num++;
				}
			}
			return num;
		}
	}
}
