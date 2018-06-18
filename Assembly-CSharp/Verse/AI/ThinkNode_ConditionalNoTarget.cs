using System;

namespace Verse.AI
{
	// Token: 0x02000AB6 RID: 2742
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06003D24 RID: 15652 RVA: 0x00204A00 File Offset: 0x00202E00
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
