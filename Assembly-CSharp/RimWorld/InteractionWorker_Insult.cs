using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B3 RID: 1203
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x0600157A RID: 5498 RVA: 0x000BECE8 File Offset: 0x000BD0E8
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04000CAE RID: 3246
		private const float BaseSelectionWeight = 0.007f;
	}
}
