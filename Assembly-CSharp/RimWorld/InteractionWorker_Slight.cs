using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B9 RID: 1209
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x04000CC2 RID: 3266
		private const float BaseSelectionWeight = 0.02f;

		// Token: 0x06001593 RID: 5523 RVA: 0x000C0344 File Offset: 0x000BE744
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}
	}
}
