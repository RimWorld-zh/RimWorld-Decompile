using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071E RID: 1822
	public static class MannableUtility
	{
		// Token: 0x06002831 RID: 10289 RVA: 0x00157A34 File Offset: 0x00155E34
		public static Thing MannedThing(this Pawn pawn)
		{
			Thing result;
			if (pawn.Dead)
			{
				result = null;
			}
			else
			{
				Thing lastMannedThing = pawn.mindState.lastMannedThing;
				if (lastMannedThing == null || lastMannedThing.TryGetComp<CompMannable>().ManningPawn != pawn)
				{
					result = null;
				}
				else
				{
					result = lastMannedThing;
				}
			}
			return result;
		}
	}
}
