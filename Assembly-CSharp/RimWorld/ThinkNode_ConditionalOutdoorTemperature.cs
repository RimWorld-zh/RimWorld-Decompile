using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D4 RID: 468
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x00056504 File Offset: 0x00054904
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
