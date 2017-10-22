using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
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
				MentalStateWorker_TargetedTantrum.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TargetedTantrum.tmpThings, null, 300, 40);
				bool flag = MentalStateWorker_TargetedTantrum.tmpThings.Any();
				MentalStateWorker_TargetedTantrum.tmpThings.Clear();
				result = flag;
			}
			return result;
		}
	}
}
