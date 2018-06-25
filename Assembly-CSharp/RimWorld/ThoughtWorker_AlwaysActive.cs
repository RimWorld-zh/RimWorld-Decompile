using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022B RID: 555
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x06000A23 RID: 2595 RVA: 0x00059AEC File Offset: 0x00057EEC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00059B08 File Offset: 0x00057F08
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
