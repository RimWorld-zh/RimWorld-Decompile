using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A65 RID: 2661
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06003B32 RID: 15154 RVA: 0x001F645C File Offset: 0x001F485C
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
