using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A83 RID: 2691
	public class MentalState_BedroomTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x001F8AF8 File Offset: 0x001F6EF8
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			outThings.Clear();
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed != null)
			{
				if (ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors)
				{
					TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_Passable), this.pawn, outThings, this.GetCustomValidator(), 0);
				}
				else
				{
					TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, ownedBed.Position, outThings, this.GetCustomValidator(), 0, 8);
				}
			}
		}
	}
}
