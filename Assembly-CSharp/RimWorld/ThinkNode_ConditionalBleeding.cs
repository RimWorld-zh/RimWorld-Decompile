using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D2 RID: 466
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x000564A0 File Offset: 0x000548A0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
