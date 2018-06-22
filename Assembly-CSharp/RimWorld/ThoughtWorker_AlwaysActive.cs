using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000229 RID: 553
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x06000A1F RID: 2591 RVA: 0x0005999C File Offset: 0x00057D9C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000599B8 File Offset: 0x00057DB8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
