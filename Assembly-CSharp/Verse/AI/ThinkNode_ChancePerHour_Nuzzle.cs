using System;

namespace Verse.AI
{
	// Token: 0x02000AAB RID: 2731
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D0C RID: 15628 RVA: 0x00204B54 File Offset: 0x00202F54
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
