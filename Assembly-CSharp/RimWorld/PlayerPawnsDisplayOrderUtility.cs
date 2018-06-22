using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B3 RID: 1971
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x06002BB3 RID: 11187 RVA: 0x00172995 File Offset: 0x00170D95
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter, PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x001729A8 File Offset: 0x00170DA8
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter).ThenBy(PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x04001782 RID: 6018
		private static Func<Pawn, int> displayOrderGetter = (Pawn x) => (x.playerSettings == null) ? 999999 : x.playerSettings.displayOrder;

		// Token: 0x04001783 RID: 6019
		private static Func<Pawn, int> thingIDNumberGetter = (Pawn x) => x.thingIDNumber;
	}
}
