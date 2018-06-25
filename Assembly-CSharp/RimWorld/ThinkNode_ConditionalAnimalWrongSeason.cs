using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CD RID: 461
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x00056384 File Offset: 0x00054784
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Animal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def);
		}
	}
}
