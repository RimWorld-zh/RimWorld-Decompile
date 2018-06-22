using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B0 RID: 1200
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x06001574 RID: 5492 RVA: 0x000BEBB4 File Offset: 0x000BCFB4
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
