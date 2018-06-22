using System;

namespace Verse.AI
{
	// Token: 0x02000AB2 RID: 2738
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06003D1F RID: 15647 RVA: 0x00204D24 File Offset: 0x00203124
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
