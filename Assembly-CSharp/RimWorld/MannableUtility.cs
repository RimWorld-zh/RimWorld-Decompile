using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000722 RID: 1826
	public static class MannableUtility
	{
		// Token: 0x06002837 RID: 10295 RVA: 0x00157800 File Offset: 0x00155C00
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
