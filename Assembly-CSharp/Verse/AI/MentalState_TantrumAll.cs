using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A83 RID: 2691
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06003BBF RID: 15295 RVA: 0x001F880C File Offset: 0x001F6C0C
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
