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
				result = ((lastMannedThing != null && lastMannedThing.TryGetComp<CompMannable>().ManningPawn == pawn) ? lastMannedThing : null);
			}
			return result;
		}
	}
}
