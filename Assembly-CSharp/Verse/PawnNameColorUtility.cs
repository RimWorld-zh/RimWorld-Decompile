using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE5 RID: 3301
	public static class PawnNameColorUtility
	{
		// Token: 0x060048A3 RID: 18595 RVA: 0x00261430 File Offset: 0x0025F830
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x00261640 File Offset: 0x0025FA40
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x002616C4 File Offset: 0x0025FAC4
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

		// Token: 0x04003125 RID: 12581
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x04003126 RID: 12582
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04003127 RID: 12583
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04003128 RID: 12584
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04003129 RID: 12585
		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x0400312A RID: 12586
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x0400312B RID: 12587
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x0400312C RID: 12588
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x0400312D RID: 12589
		private const int ColorShiftCount = 10;

		// Token: 0x0400312E RID: 12590
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
	}
}
