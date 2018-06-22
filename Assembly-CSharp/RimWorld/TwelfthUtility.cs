using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090F RID: 2319
	public static class TwelfthUtility
	{
		// Token: 0x0600360B RID: 13835 RVA: 0x001D03D8 File Offset: 0x001CE7D8
		public static Quadrum GetQuadrum(this Twelfth twelfth)
		{
			Quadrum result;
			switch (twelfth)
			{
			case Twelfth.First:
				result = Quadrum.Aprimay;
				break;
			case Twelfth.Second:
				result = Quadrum.Aprimay;
				break;
			case Twelfth.Third:
				result = Quadrum.Aprimay;
				break;
			case Twelfth.Fourth:
				result = Quadrum.Jugust;
				break;
			case Twelfth.Fifth:
				result = Quadrum.Jugust;
				break;
			case Twelfth.Sixth:
				result = Quadrum.Jugust;
				break;
			case Twelfth.Seventh:
				result = Quadrum.Septober;
				break;
			case Twelfth.Eighth:
				result = Quadrum.Septober;
				break;
			case Twelfth.Ninth:
				result = Quadrum.Septober;
				break;
			case Twelfth.Tenth:
				result = Quadrum.Decembary;
				break;
			case Twelfth.Eleventh:
				result = Quadrum.Decembary;
				break;
			case Twelfth.Twelfth:
				result = Quadrum.Decembary;
				break;
			default:
				result = Quadrum.Undefined;
				break;
			}
			return result;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x001D0480 File Offset: 0x001CE880
		public static Twelfth PreviousTwelfth(this Twelfth twelfth)
		{
			Twelfth result;
			if (twelfth == Twelfth.Undefined)
			{
				result = Twelfth.Undefined;
			}
			else
			{
				int num = (int)(twelfth - Twelfth.Second);
				if (num == -1)
				{
					num = 11;
				}
				result = (Twelfth)num;
			}
			return result;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x001D04B8 File Offset: 0x001CE8B8
		public static Twelfth NextTwelfth(this Twelfth twelfth)
		{
			Twelfth result;
			if (twelfth == Twelfth.Undefined)
			{
				result = Twelfth.Undefined;
			}
			else
			{
				result = (twelfth + 1) % Twelfth.Undefined;
			}
			return result;
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x001D04E4 File Offset: 0x001CE8E4
		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x001D0508 File Offset: 0x001CE908
		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x001D0528 File Offset: 0x001CE928
		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 16f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x001D0574 File Offset: 0x001CE974
		public static Twelfth GetLeftMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			Twelfth result;
			if (twelfths.Count >= 12)
			{
				result = Twelfth.Undefined;
			}
			else
			{
				Twelfth twelfth;
				do
				{
					twelfth = rootTwelfth;
					rootTwelfth = TwelfthUtility.TwelfthBefore(rootTwelfth);
				}
				while (twelfths.Contains(rootTwelfth));
				result = twelfth;
			}
			return result;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x001D05C0 File Offset: 0x001CE9C0
		public static Twelfth GetRightMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			Twelfth result;
			if (twelfths.Count >= 12)
			{
				result = Twelfth.Undefined;
			}
			else
			{
				Twelfth m;
				do
				{
					m = rootTwelfth;
					rootTwelfth = TwelfthUtility.TwelfthAfter(rootTwelfth);
				}
				while (twelfths.Contains(rootTwelfth));
				result = TwelfthUtility.TwelfthAfter(m);
			}
			return result;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x001D0610 File Offset: 0x001CEA10
		public static Twelfth TwelfthBefore(Twelfth m)
		{
			Twelfth result;
			if (m == Twelfth.First)
			{
				result = Twelfth.Twelfth;
			}
			else
			{
				result = (Twelfth)(m - Twelfth.Second);
			}
			return result;
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x001D0638 File Offset: 0x001CEA38
		public static Twelfth TwelfthAfter(Twelfth m)
		{
			Twelfth result;
			if (m == Twelfth.Twelfth)
			{
				result = Twelfth.First;
			}
			else
			{
				result = m + 1;
			}
			return result;
		}
	}
}
