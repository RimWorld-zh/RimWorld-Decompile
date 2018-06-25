using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A68 RID: 2664
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
	{
		// Token: 0x0400256B RID: 9579
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B3F RID: 15167 RVA: 0x001F6E28 File Offset: 0x001F5228
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
					bool flag = MentalStateWorker_BedroomTantrum.tmpThings.Any<Thing>();
					MentalStateWorker_BedroomTantrum.tmpThings.Clear();
					result = flag;
				}
			}
			return result;
		}
	}
}
