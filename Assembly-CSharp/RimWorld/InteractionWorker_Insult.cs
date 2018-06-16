using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B7 RID: 1207
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x06001583 RID: 5507 RVA: 0x000BECCC File Offset: 0x000BD0CC
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04000CB1 RID: 3249
		private const float BaseSelectionWeight = 0.007f;
	}
}
