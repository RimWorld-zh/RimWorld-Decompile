using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A63 RID: 2659
	public class MentalStateWorker_BingingDrug : MentalStateWorker
	{
		// Token: 0x06003B2E RID: 15150 RVA: 0x001F634C File Offset: 0x001F474C
		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else if (!pawn.Spawned)
			{
				result = false;
			}
			else
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], this.def.drugCategory))
					{
						return true;
					}
					if (this.def.drugCategory == DrugCategory.Hard)
					{
						if (AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], DrugCategory.Social))
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
