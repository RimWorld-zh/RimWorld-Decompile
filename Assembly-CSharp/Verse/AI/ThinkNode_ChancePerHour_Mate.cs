using System;

namespace Verse.AI
{
	// Token: 0x02000AAC RID: 2732
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D0E RID: 15630 RVA: 0x00204B7C File Offset: 0x00202F7C
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
