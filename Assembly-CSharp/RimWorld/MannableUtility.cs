using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public static class MannableUtility
	{
		// Token: 0x06002834 RID: 10292 RVA: 0x00157DE4 File Offset: 0x001561E4
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
