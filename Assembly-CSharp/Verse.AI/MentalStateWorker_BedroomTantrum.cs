using RimWorld;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
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
				Building_Bed ownedBed = pawn.ownership.OwnedBed;
				if (ownedBed == null || ownedBed.GetRoom(RegionType.Set_Passable) == null || ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors)
				{
					result = false;
				}
				else
				{
					MentalStateWorker_BedroomTantrum.tmpThings.Clear();
					TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_Passable), pawn, MentalStateWorker_BedroomTantrum.tmpThings, null, 0);
					bool flag = MentalStateWorker_BedroomTantrum.tmpThings.Any();
					MentalStateWorker_BedroomTantrum.tmpThings.Clear();
					result = flag;
				}
			}
			return result;
		}
	}
}
