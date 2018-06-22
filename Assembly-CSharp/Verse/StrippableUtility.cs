using System;

namespace Verse
{
	// Token: 0x02000D51 RID: 3409
	public static class StrippableUtility
	{
		// Token: 0x06004C0D RID: 19469 RVA: 0x0027B460 File Offset: 0x00279860
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
