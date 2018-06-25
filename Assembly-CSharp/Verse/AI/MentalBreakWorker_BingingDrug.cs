using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5D RID: 2653
	public class MentalBreakWorker_BingingDrug : MentalBreakWorker
	{
		// Token: 0x06003B05 RID: 15109 RVA: 0x001F529C File Offset: 0x001F369C
		public override float CommonalityFor(Pawn pawn)
		{
			float num = base.CommonalityFor(pawn);
			int num2 = this.BingeableAddictionsCount(pawn);
			if (num2 > 0)
			{
				num *= 1.4f * (float)num2;
			}
			if (pawn.story != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				if (trait != null)
				{
					if (trait.Degree == 1)
					{
						num *= 2.5f;
					}
					else if (trait.Degree == 2)
					{
						num *= 5f;
					}
				}
			}
			return num;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x001F532C File Offset: 0x001F372C
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
