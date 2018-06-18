using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BD RID: 1213
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x0600159C RID: 5532 RVA: 0x000C0358 File Offset: 0x000BE758
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04000CC5 RID: 3269
		private const float BaseSelectionWeight = 0.02f;
	}
}
