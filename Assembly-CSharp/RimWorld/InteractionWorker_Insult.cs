using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B5 RID: 1205
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x04000CB1 RID: 3249
		private const float BaseSelectionWeight = 0.007f;

		// Token: 0x0600157D RID: 5501 RVA: 0x000BF038 File Offset: 0x000BD438
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}
	}
}
