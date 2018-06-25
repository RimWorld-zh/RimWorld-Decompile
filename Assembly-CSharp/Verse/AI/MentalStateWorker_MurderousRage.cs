using System;

namespace Verse.AI
{
	// Token: 0x02000A6E RID: 2670
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06003B4E RID: 15182 RVA: 0x001F70E0 File Offset: 0x001F54E0
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
