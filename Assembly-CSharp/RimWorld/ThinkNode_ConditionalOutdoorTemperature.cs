using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D4 RID: 468
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x06000961 RID: 2401 RVA: 0x000564F4 File Offset: 0x000548F4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
