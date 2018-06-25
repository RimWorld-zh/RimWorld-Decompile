using System;

namespace Verse.AI
{
	// Token: 0x02000AAD RID: 2733
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D10 RID: 15632 RVA: 0x00204C80 File Offset: 0x00203080
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
