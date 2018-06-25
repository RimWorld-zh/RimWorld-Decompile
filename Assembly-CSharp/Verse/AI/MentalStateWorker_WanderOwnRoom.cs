using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A63 RID: 2659
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06003B33 RID: 15155 RVA: 0x001F6958 File Offset: 0x001F4D58
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
				result = (ownedBed != null && ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors);
			}
			return result;
		}
	}
}
