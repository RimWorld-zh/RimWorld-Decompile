using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A67 RID: 2663
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
	{
		// Token: 0x0400255B RID: 9563
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B3E RID: 15166 RVA: 0x001F6AFC File Offset: 0x001F4EFC
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
