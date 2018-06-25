using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022B RID: 555
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x06000A22 RID: 2594 RVA: 0x00059AE8 File Offset: 0x00057EE8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00059B04 File Offset: 0x00057F04
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
