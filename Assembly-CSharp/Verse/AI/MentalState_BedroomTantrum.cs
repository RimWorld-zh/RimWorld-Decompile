using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A84 RID: 2692
	public class MentalState_BedroomTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06003BB6 RID: 15286 RVA: 0x001F82B8 File Offset: 0x001F66B8
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
