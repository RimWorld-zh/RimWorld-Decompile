using System;
using Verse;

namespace RimWorld
{
	public static class MannableUtility
	{
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
