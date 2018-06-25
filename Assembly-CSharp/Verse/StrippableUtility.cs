using System;

namespace Verse
{
	public static class StrippableUtility
	{
		public static bool CanBeStrippedByColony(Thing th)
		{
			IStrippable strippable = th as IStrippable;
			bool result;
			if (strippable == null)
			{
				result = false;
			}
			else if (!strippable.AnythingToStrip())
			{
				result = false;
			}
			else
			{
				Pawn pawn = th as Pawn;
				result = (pawn == null || pawn.Downed || (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure));
			}
			return result;
		}
	}
}
