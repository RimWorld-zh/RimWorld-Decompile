using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BB RID: 1211
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x04000CC2 RID: 3266
		private const float BaseSelectionWeight = 0.02f;

		// Token: 0x06001597 RID: 5527 RVA: 0x000C0494 File Offset: 0x000BE894
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}
	}
}
