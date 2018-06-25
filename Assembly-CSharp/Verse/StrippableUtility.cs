using System;

namespace Verse
{
	// Token: 0x02000D53 RID: 3411
	public static class StrippableUtility
	{
		// Token: 0x06004C11 RID: 19473 RVA: 0x0027B58C File Offset: 0x0027998C
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
