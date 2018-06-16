using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B7 RID: 1975
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x06002BB8 RID: 11192 RVA: 0x00172729 File Offset: 0x00170B29
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter, PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x0017273C File Offset: 0x00170B3C
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter).ThenBy(PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x04001784 RID: 6020
		private static Func<Pawn, int> displayOrderGetter = (Pawn x) => (x.playerSettings == null) ? 999999 : x.playerSettings.displayOrder;

		// Token: 0x04001785 RID: 6021
		private static Func<Pawn, int> thingIDNumberGetter = (Pawn x) => x.thingIDNumber;
	}
}
