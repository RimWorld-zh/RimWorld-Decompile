using System;

namespace Verse.AI
{
	// Token: 0x02000AB4 RID: 2740
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06003D23 RID: 15651 RVA: 0x00204E50 File Offset: 0x00203250
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
