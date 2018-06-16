using System;

namespace Verse.AI
{
	// Token: 0x02000AB6 RID: 2742
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06003D22 RID: 15650 RVA: 0x0020492C File Offset: 0x00202D2C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
