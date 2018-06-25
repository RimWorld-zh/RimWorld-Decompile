using System;

namespace Verse.AI
{
	// Token: 0x02000AB5 RID: 2741
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06003D23 RID: 15651 RVA: 0x00205130 File Offset: 0x00203530
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
