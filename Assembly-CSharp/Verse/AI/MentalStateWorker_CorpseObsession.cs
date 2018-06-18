using System;

namespace Verse.AI
{
	// Token: 0x02000A6C RID: 2668
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06003B48 RID: 15176 RVA: 0x001F68B4 File Offset: 0x001F4CB4
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
