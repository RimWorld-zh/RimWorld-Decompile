using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanTicksPerMoveUtility
	{
		private const int DownedPawnMoveTicks = 450;

		public const float CellToTilesConversionRatio = 190f;

		public const int DefaultTicksPerMove = 3100;

		public static int GetTicksPerMove(Caravan caravan)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravan.PawnsListForReading);
		}

		public static int GetTicksPerMove(List<Pawn> pawns)
		{
			if (pawns.Any<Pawn>())
			{
				float num = 0f;
				for (int i = 0; i < pawns.Count; i++)
				{
					int num2 = (!pawns[i].Downed) ? pawns[i].TicksPerMoveCardinal : 450;
					num += (float)num2 / (float)pawns.Count;
				}
				num *= 190f;
				return Mathf.Max(Mathf.RoundToInt(num), 1);
			}
			return 3100;
		}
	}
}
