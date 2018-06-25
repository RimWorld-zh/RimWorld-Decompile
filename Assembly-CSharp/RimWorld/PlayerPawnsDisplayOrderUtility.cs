using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B5 RID: 1973
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x04001786 RID: 6022
		private static Func<Pawn, int> displayOrderGetter = (Pawn x) => (x.playerSettings == null) ? 999999 : x.playerSettings.displayOrder;

		// Token: 0x04001787 RID: 6023
		private static Func<Pawn, int> thingIDNumberGetter = (Pawn x) => x.thingIDNumber;

		// Token: 0x06002BB6 RID: 11190 RVA: 0x00172D49 File Offset: 0x00171149
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter, PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x00172D5C File Offset: 0x0017115C
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter).ThenBy(PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}
	}
}
