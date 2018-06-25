using System;

namespace Verse.AI
{
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		public MentalStateWorker_CorpseObsession()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
