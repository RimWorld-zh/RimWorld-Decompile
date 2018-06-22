using System;

namespace Verse.AI
{
	// Token: 0x02000A6B RID: 2667
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06003B49 RID: 15177 RVA: 0x001F6C88 File Offset: 0x001F5088
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
