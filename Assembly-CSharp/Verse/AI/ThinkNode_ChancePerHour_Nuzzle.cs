using System;

namespace Verse.AI
{
	// Token: 0x02000AAF RID: 2735
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D11 RID: 15633 RVA: 0x00204830 File Offset: 0x00202C30
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
