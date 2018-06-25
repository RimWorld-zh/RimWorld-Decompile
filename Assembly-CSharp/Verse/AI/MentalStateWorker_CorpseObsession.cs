using System;

namespace Verse.AI
{
	// Token: 0x02000A6B RID: 2667
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06003B48 RID: 15176 RVA: 0x001F7008 File Offset: 0x001F5408
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
