using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BD RID: 1213
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x0600159C RID: 5532 RVA: 0x000C033C File Offset: 0x000BE73C
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04000CC5 RID: 3269
		private const float BaseSelectionWeight = 0.02f;
	}
}
