using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE3 RID: 3299
	public static class PawnNameColorUtility
	{
		// Token: 0x0400312E RID: 12590
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x0400312F RID: 12591
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04003130 RID: 12592
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04003131 RID: 12593
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04003132 RID: 12594
		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04003133 RID: 12595
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04003134 RID: 12596
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04003135 RID: 12597
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04003136 RID: 12598
		private const int ColorShiftCount = 10;

		// Token: 0x04003137 RID: 12599
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

		// Token: 0x060048B5 RID: 18613 RVA: 0x002628FC File Offset: 0x00260CFC
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x00262B0C File Offset: 0x00260F0C
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00262B90 File Offset: 0x00260F90
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
