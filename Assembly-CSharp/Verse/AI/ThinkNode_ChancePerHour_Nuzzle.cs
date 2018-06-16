using System;

namespace Verse.AI
{
	// Token: 0x02000AAF RID: 2735
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D0F RID: 15631 RVA: 0x0020475C File Offset: 0x00202B5C
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
