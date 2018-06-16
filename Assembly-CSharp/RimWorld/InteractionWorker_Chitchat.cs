using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B4 RID: 1204
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x0600157D RID: 5501 RVA: 0x000BEB98 File Offset: 0x000BCF98
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
