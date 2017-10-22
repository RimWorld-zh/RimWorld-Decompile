using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		private static List<Thing> tmpThings = new List<Thing>();

		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				MentalStateWorker_TantrumAll.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
				bool flag = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
				MentalStateWorker_TantrumAll.tmpThings.Clear();
				result = flag;
			}
			return result;
		}
	}
}
