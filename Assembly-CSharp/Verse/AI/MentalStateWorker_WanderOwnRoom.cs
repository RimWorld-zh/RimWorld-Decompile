using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A61 RID: 2657
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06003B2F RID: 15151 RVA: 0x001F682C File Offset: 0x001F4C2C
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
