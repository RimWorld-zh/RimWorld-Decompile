using System;

namespace Verse.AI
{
	// Token: 0x02000A6F RID: 2671
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06003B4E RID: 15182 RVA: 0x001F698C File Offset: 0x001F4D8C
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
