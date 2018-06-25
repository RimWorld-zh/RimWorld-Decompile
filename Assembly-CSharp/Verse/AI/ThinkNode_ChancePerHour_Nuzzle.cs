using System;

namespace Verse.AI
{
	// Token: 0x02000AAE RID: 2734
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D10 RID: 15632 RVA: 0x00204F60 File Offset: 0x00203360
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
