using RimWorld;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_BingingDrug : MentalStateWorker
	{
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
					if (AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], base.def.drugCategory))
						goto IL_0051;
					if (base.def.drugCategory == DrugCategory.Hard && AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], DrugCategory.Social))
						goto IL_007d;
				}
				result = false;
			}
			goto IL_009d;
			IL_0051:
			result = true;
			goto IL_009d;
			IL_009d:
			return result;
			IL_007d:
			result = true;
			goto IL_009d;
		}
	}
}
