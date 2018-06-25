using System;

namespace Verse.AI
{
	// Token: 0x02000AAE RID: 2734
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D12 RID: 15634 RVA: 0x00204CA8 File Offset: 0x002030A8
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
