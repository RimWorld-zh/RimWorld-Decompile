using System;

namespace Verse.AI
{
	// Token: 0x02000A6F RID: 2671
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06003B4C RID: 15180 RVA: 0x001F68B8 File Offset: 0x001F4CB8
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
