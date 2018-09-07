using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class PlayerPawnsDisplayOrderUtility
	{
		private static Func<Pawn, int> displayOrderGetter = (Pawn x) => (x.playerSettings == null) ? 999999 : x.playerSettings.displayOrder;

		private static Func<Pawn, int> thingIDNumberGetter = (Pawn x) => x.thingIDNumber;

		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter, PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter).ThenBy(PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PlayerPawnsDisplayOrderUtility()
		{
		}

		[CompilerGenerated]
		private static int <displayOrderGetter>m__0(Pawn x)
		{
			return (x.playerSettings == null) ? 999999 : x.playerSettings.displayOrder;
		}

		[CompilerGenerated]
		private static int <thingIDNumberGetter>m__1(Pawn x)
		{
			return x.thingIDNumber;
		}
	}
}
