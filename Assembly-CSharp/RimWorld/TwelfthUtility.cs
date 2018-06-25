using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class TwelfthUtility
	{
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

		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 16f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

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
