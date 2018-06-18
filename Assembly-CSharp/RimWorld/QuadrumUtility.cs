using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090F RID: 2319
	public static class QuadrumUtility
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060035FC RID: 13820 RVA: 0x001CF714 File Offset: 0x001CDB14
		public static Quadrum FirstQuadrum
		{
			get
			{
				return Quadrum.Aprimay;
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x001CF72C File Offset: 0x001CDB2C
		public static Twelfth GetFirstTwelfth(this Quadrum quadrum)
		{
			Twelfth result;
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				result = Twelfth.First;
				break;
			case Quadrum.Jugust:
				result = Twelfth.Fourth;
				break;
			case Quadrum.Septober:
				result = Twelfth.Seventh;
				break;
			case Quadrum.Decembary:
				result = Twelfth.Tenth;
				break;
			default:
				result = Twelfth.Undefined;
				break;
			}
			return result;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x001CF77C File Offset: 0x001CDB7C
		public static Twelfth GetMiddleTwelfth(this Quadrum quadrum)
		{
			Twelfth result;
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				result = Twelfth.Second;
				break;
			case Quadrum.Jugust:
				result = Twelfth.Fifth;
				break;
			case Quadrum.Septober:
				result = Twelfth.Eighth;
				break;
			case Quadrum.Decembary:
				result = Twelfth.Eleventh;
				break;
			default:
				result = Twelfth.Undefined;
				break;
			}
			return result;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x001CF7CC File Offset: 0x001CDBCC
		public static float GetMiddleYearPct(this Quadrum quadrum)
		{
			return quadrum.GetMiddleTwelfth().GetMiddleYearPct();
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x001CF7EC File Offset: 0x001CDBEC
		public static string Label(this Quadrum quadrum)
		{
			string result;
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				result = "QuadrumAprimay".Translate();
				break;
			case Quadrum.Jugust:
				result = "QuadrumJugust".Translate();
				break;
			case Quadrum.Septober:
				result = "QuadrumSeptober".Translate();
				break;
			case Quadrum.Decembary:
				result = "QuadrumDecembary".Translate();
				break;
			default:
				result = "Unknown quadrum";
				break;
			}
			return result;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x001CF864 File Offset: 0x001CDC64
		public static Season GetSeason(this Quadrum q, float latitude)
		{
			float middleYearPct = q.GetMiddleYearPct();
			return SeasonUtility.GetReportedSeason(middleYearPct, latitude);
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x001CF888 File Offset: 0x001CDC88
		public static string QuadrumsRangeLabel(List<Twelfth> twelfths)
		{
			string result;
			if (twelfths.Count == 0)
			{
				result = "";
			}
			else if (twelfths.Count == 12)
			{
				result = "WholeYear".Translate();
			}
			else
			{
				string text = "";
				for (int i = 0; i < 12; i++)
				{
					Twelfth twelfth = (Twelfth)i;
					if (twelfths.Contains(twelfth))
					{
						if (!text.NullOrEmpty())
						{
							text += ", ";
						}
						text += QuadrumUtility.QuadrumsContinuousRangeLabel(twelfths, twelfth);
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x001CF924 File Offset: 0x001CDD24
		private static string QuadrumsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			for (Twelfth twelfth = leftMostTwelfth; twelfth != rightMostTwelfth; twelfth = TwelfthUtility.TwelfthAfter(twelfth))
			{
				if (!twelfths.Contains(twelfth))
				{
					Log.Error(string.Concat(new object[]
					{
						"Twelfths doesn't contain ",
						twelfth,
						" (",
						leftMostTwelfth,
						"..",
						rightMostTwelfth,
						")"
					}), false);
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.QuadrumDateStringAt(leftMostTwelfth) + " - " + GenDate.QuadrumDateStringAt(rightMostTwelfth);
		}
	}
}
