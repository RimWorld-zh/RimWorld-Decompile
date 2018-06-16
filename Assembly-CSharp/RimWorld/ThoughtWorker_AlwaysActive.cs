using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000229 RID: 553
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x00059958 File Offset: 0x00057D58
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00059974 File Offset: 0x00057D74
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
