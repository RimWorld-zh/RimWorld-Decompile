using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		private static List<Thing> tmpThings = new List<Thing>();

		public MentalStateWorker_TantrumAll()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
			bool result = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MentalStateWorker_TantrumAll()
		{
		}
	}
}
