using System;

namespace Verse.AI
{
	// Token: 0x02000A6A RID: 2666
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06003B47 RID: 15175 RVA: 0x001F6CDC File Offset: 0x001F50DC
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
