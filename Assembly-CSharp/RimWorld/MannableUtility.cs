using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public static class MannableUtility
	{
		// Token: 0x06002835 RID: 10293 RVA: 0x00157B84 File Offset: 0x00155F84
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
