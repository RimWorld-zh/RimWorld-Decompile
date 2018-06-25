using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B2 RID: 1202
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x06001578 RID: 5496 RVA: 0x000BED04 File Offset: 0x000BD104
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
