using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BB RID: 1211
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x04000CC5 RID: 3269
		private const float BaseSelectionWeight = 0.02f;

		// Token: 0x06001596 RID: 5526 RVA: 0x000C0694 File Offset: 0x000BEA94
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}
	}
}
