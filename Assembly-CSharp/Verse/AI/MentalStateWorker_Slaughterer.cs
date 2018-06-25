using System;

namespace Verse.AI
{
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		public MentalStateWorker_Slaughterer()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
