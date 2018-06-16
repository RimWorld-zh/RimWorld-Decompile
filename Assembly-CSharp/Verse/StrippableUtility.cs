using System;

namespace Verse
{
	// Token: 0x02000D55 RID: 3413
	public static class StrippableUtility
	{
		// Token: 0x06004BFB RID: 19451 RVA: 0x00279EE8 File Offset: 0x002782E8
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
