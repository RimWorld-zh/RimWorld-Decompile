using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B2 RID: 1202
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x06001577 RID: 5495 RVA: 0x000BEF04 File Offset: 0x000BD304
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
