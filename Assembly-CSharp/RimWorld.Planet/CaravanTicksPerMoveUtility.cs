using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanTicksPerMoveUtility
	{
		private const int DownedPawnMoveTicks = 450;

		public const float CellToTilesConversionRatio = 190f;

		public const int DefaultTicksPerMove = 2500;

		public static int GetTicksPerMove(Caravan caravan)
		{
			return (caravan != null) ? CaravanTicksPerMoveUtility.GetTicksPerMove(caravan.PawnsListForReading) : 2500;
		}

		public static int GetTicksPerMove(List<Pawn> pawns)
		{
			int result;
			if (pawns.Any())
			{
				float num = 0f;
				for (int i = 0; i < pawns.Count; i++)
				{
					int num2 = (!pawns[i].Downed) ? pawns[i].TicksPerMoveCardinal : 450;
					num += (float)num2 / (float)pawns.Count;
				}
				num = (float)(num * 190.0);
				result = Mathf.Max(Mathf.RoundToInt(num), 1);
			}
			else
			{
				result = 2500;
			}
			return result;
		}
	}
}
