using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		public MentalStateWorker_WanderOwnRoom()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			return ownedBed != null && ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors;
		}
	}
}
