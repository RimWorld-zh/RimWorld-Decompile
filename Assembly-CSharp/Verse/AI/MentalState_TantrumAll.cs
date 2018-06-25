using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A86 RID: 2694
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06003BC4 RID: 15300 RVA: 0x001F8C64 File Offset: 0x001F7064
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
