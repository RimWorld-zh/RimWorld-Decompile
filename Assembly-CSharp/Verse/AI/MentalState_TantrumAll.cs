using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A87 RID: 2695
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06003BC4 RID: 15300 RVA: 0x001F84F8 File Offset: 0x001F68F8
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
