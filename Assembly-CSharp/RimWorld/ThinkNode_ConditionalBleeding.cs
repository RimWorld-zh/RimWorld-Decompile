using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D2 RID: 466
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x0600095A RID: 2394 RVA: 0x0005649C File Offset: 0x0005489C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
