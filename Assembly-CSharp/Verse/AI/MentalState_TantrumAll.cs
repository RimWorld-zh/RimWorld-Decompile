using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A85 RID: 2693
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06003BC3 RID: 15299 RVA: 0x001F8938 File Offset: 0x001F6D38
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
