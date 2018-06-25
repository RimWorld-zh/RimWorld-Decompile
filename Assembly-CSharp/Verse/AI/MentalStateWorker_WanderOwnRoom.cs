using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A64 RID: 2660
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06003B34 RID: 15156 RVA: 0x001F6C84 File Offset: 0x001F5084
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
