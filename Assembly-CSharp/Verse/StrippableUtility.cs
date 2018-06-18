using System;

namespace Verse
{
	// Token: 0x02000D54 RID: 3412
	public static class StrippableUtility
	{
		// Token: 0x06004BF9 RID: 19449 RVA: 0x00279EC8 File Offset: 0x002782C8
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
