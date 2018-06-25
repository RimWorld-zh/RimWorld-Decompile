using System;

namespace Verse.AI
{
	// Token: 0x02000A6D RID: 2669
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x06003B4D RID: 15181 RVA: 0x001F6DB4 File Offset: 0x001F51B4
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
