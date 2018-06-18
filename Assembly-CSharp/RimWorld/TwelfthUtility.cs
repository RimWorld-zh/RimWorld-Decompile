using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000913 RID: 2323
	public static class TwelfthUtility
	{
		// Token: 0x06003612 RID: 13842 RVA: 0x001D01F0 File Offset: 0x001CE5F0
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

		// Token: 0x06003613 RID: 13843 RVA: 0x001D0298 File Offset: 0x001CE698
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

		// Token: 0x06003614 RID: 13844 RVA: 0x001D02D0 File Offset: 0x001CE6D0
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

		// Token: 0x06003615 RID: 13845 RVA: 0x001D02FC File Offset: 0x001CE6FC
		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x001D0320 File Offset: 0x001CE720
		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x001D0340 File Offset: 0x001CE740
		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 16f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x001D038C File Offset: 0x001CE78C
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

		// Token: 0x06003619 RID: 13849 RVA: 0x001D03D8 File Offset: 0x001CE7D8
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

		// Token: 0x0600361A RID: 13850 RVA: 0x001D0428 File Offset: 0x001CE828
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

		// Token: 0x0600361B RID: 13851 RVA: 0x001D0450 File Offset: 0x001CE850
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
