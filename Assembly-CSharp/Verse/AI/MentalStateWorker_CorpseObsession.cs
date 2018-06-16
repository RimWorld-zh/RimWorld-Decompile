using System;

namespace Verse.AI
{
	// Token: 0x02000A6C RID: 2668
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06003B46 RID: 15174 RVA: 0x001F67E0 File Offset: 0x001F4BE0
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
