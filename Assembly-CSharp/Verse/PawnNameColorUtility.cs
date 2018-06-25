using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class PawnNameColorUtility
	{
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		private static readonly List<Color> ColorsHostile = new List<Color>();

		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		private const int ColorShiftCount = 10;

		private static readonly List<Color> ColorShifts = new List<Color>
		{
			new Color(1f, 1f, 1f),
			new Color(0.8f, 1f, 1f),
			new Color(0.8f, 0.8f, 1f),
			new Color(0.8f, 0.8f, 0.8f),
			new Color(1.2f, 1f, 1f),
			new Color(0.8f, 1.2f, 1f),
			new Color(0.8f, 1.2f, 1.2f),
			new Color(1.2f, 1.2f, 1.2f),
			new Color(1f, 1.2f, 1f),
			new Color(1.2f, 1f, 0.8f)
		};

		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		public static Color PawnNameColorOf(Pawn pawn)
		{
			Color result;
			if (pawn.MentalStateDef != null)
			{
				result = pawn.MentalStateDef.nameColor;
			}
			else
			{
				int index;
				if (pawn.Faction == null)
				{
					index = 0;
				}
				else
				{
					index = pawn.Faction.randomKey % 10;
				}
				if (pawn.IsPrisoner)
				{
					result = PawnNameColorUtility.ColorsPrisoner[index];
				}
				else if (pawn.IsWildMan())
				{
					result = PawnNameColorUtility.ColorWildMan;
				}
				else if (pawn.Faction == null)
				{
					result = PawnNameColorUtility.ColorsNeutral[index];
				}
				else if (pawn.Faction == Faction.OfPlayer)
				{
					result = PawnNameColorUtility.ColorColony;
				}
				else if (pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					result = PawnNameColorUtility.ColorsHostile[index];
				}
				else
				{
					result = PawnNameColorUtility.ColorsNeutral[index];
				}
			}
			return result;
		}
	}
}
