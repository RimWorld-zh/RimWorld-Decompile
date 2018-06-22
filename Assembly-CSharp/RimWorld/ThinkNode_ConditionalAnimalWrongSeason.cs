using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CD RID: 461
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x00056388 File Offset: 0x00054788
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Animal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def);
		}
	}
}
