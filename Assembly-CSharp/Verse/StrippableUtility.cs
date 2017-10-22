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
				result = ((byte)((pawn == null) ? 1 : (pawn.Downed ? 1 : ((pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure) ? 1 : 0))) != 0);
			}
			return result;
		}
	}
}
