using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class PawnNameColorUtility
	{
		private static readonly List<Color> ColorsNeutral;

		private static readonly List<Color> ColorsHostile;

		private static readonly List<Color> ColorsPrisoner;

		private static readonly Color ColorBaseNeutral;

		private static readonly Color ColorBaseHostile;

		private static readonly Color ColorBasePrisoner;

		private static readonly Color ColorColony;

		private static readonly Color ColorWildMan;

		private const int ColorShiftCount = 10;

		private static readonly List<Color> ColorShifts;

		static PawnNameColorUtility()
		{
			PawnNameColorUtility.ColorsNeutral = new List<Color>();
			PawnNameColorUtility.ColorsHostile = new List<Color>();
			PawnNameColorUtility.ColorsPrisoner = new List<Color>();
			PawnNameColorUtility.ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);
			PawnNameColorUtility.ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);
			PawnNameColorUtility.ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);
			PawnNameColorUtility.ColorColony = new Color(0.9f, 0.9f, 0.9f);
			PawnNameColorUtility.ColorWildMan = new Color(1f, 0.8f, 1f);
			PawnNameColorUtility.ColorShifts = new List<Color>
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
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		private static Color RandomShiftOf(Color color, int i)
		{
			float r = color.r;
			Color color2 = PawnNameColorUtility.ColorShifts[i];
			float r2 = Mathf.Clamp01(r * color2.r);
			float g = color.g;
			Color color3 = PawnNameColorUtility.ColorShifts[i];
			float g2 = Mathf.Clamp01(g * color3.g);
			float b = color.b;
			Color color4 = PawnNameColorUtility.ColorShifts[i];
			return new Color(r2, g2, Mathf.Clamp01(b * color4.b), color.a);
		}

		public static Color PawnNameColorOf(Pawn pawn)
		{
			if (pawn.MentalStateDef != null)
			{
				return pawn.MentalStateDef.nameColor;
			}
			int index = (pawn.Faction != null) ? (pawn.Faction.randomKey % 10) : 0;
			if (pawn.IsPrisoner)
			{
				return PawnNameColorUtility.ColorsPrisoner[index];
			}
			if (pawn.IsWildMan())
			{
				return PawnNameColorUtility.ColorWildMan;
			}
			if (pawn.Faction == null)
			{
				return PawnNameColorUtility.ColorsNeutral[index];
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return PawnNameColorUtility.ColorColony;
			}
			if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return PawnNameColorUtility.ColorsHostile[index];
			}
			return PawnNameColorUtility.ColorsNeutral[index];
		}
	}
}
