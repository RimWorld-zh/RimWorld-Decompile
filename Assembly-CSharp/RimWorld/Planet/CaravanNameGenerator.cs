using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanNameGenerator
	{
		public static string GenerateCaravanName(Caravan caravan)
		{
			Pawn pawn;
			if ((pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan)) == null)
			{
				pawn = (BestCaravanPawnUtility.FindBestDiplomat(caravan) ?? caravan.PawnsListForReading.Find((Pawn x) => caravan.IsOwner(x)));
			}
			Pawn pawn2 = pawn;
			string text = (pawn2 == null) ? caravan.def.label : "CaravanLeaderCaravanName".Translate(new object[]
			{
				pawn2.LabelShort
			}).CapitalizeFirst();
			for (int i = 1; i <= 1000; i++)
			{
				string text2 = text;
				if (i != 1)
				{
					text2 = text2 + " " + i;
				}
				if (!CaravanNameGenerator.CaravanNameInUse(text2))
				{
					return text2;
				}
			}
			Log.Error("Ran out of caravan names.", false);
			return caravan.def.label;
		}

		private static bool CaravanNameInUse(string name)
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}

		[CompilerGenerated]
		private sealed class <GenerateCaravanName>c__AnonStorey0
		{
			internal Caravan caravan;

			public <GenerateCaravanName>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return this.caravan.IsOwner(x);
			}
		}
	}
}
