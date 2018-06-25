using System;

namespace Verse.AI
{
	// Token: 0x02000AAF RID: 2735
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D12 RID: 15634 RVA: 0x00204F88 File Offset: 0x00203388
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
