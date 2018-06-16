using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D3 RID: 467
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		// Token: 0x0600095F RID: 2399 RVA: 0x000564C0 File Offset: 0x000548C0
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
