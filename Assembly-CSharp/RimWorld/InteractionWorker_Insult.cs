using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B5 RID: 1205
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x04000CAE RID: 3246
		private const float BaseSelectionWeight = 0.007f;

		// Token: 0x0600157E RID: 5502 RVA: 0x000BEE38 File Offset: 0x000BD238
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}
	}
}
