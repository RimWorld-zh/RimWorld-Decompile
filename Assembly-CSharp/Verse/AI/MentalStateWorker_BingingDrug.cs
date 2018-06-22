using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5F RID: 2655
	public class MentalStateWorker_BingingDrug : MentalStateWorker
	{
		// Token: 0x06003B2B RID: 15147 RVA: 0x001F671C File Offset: 0x001F4B1C
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
