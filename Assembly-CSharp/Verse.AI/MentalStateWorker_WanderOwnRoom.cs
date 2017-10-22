using RimWorld;

namespace Verse.AI
{
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
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
				result = ((byte)((ownedBed != null && ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors) ? 1 : 0) != 0);
			}
			return result;
		}
	}
}
