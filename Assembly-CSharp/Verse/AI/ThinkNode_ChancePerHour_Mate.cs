using System;

namespace Verse.AI
{
	// Token: 0x02000AB0 RID: 2736
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D11 RID: 15633 RVA: 0x00204784 File Offset: 0x00202B84
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
