using System;

namespace Verse.AI
{
	// Token: 0x02000A68 RID: 2664
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06003B43 RID: 15171 RVA: 0x001F6BB0 File Offset: 0x001F4FB0
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
