using System.Collections.Generic;
using UnityEngine;
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
			{
				result = Quadrum.Aprimay;
				break;
			}
			case Twelfth.Second:
			{
				result = Quadrum.Aprimay;
				break;
			}
			case Twelfth.Third:
			{
				result = Quadrum.Aprimay;
				break;
			}
			case Twelfth.Fourth:
			{
				result = Quadrum.Jugust;
				break;
			}
			case Twelfth.Fifth:
			{
				result = Quadrum.Jugust;
				break;
			}
			case Twelfth.Sixth:
			{
				result = Quadrum.Jugust;
				break;
			}
			case Twelfth.Seventh:
			{
				result = Quadrum.Septober;
				break;
			}
			case Twelfth.Eighth:
			{
				result = Quadrum.Septober;
				break;
			}
			case Twelfth.Ninth:
			{
				result = Quadrum.Septober;
				break;
			}
			case Twelfth.Tenth:
			{
				result = Quadrum.Decembary;
				break;
			}
			case Twelfth.Eleventh:
			{
				result = Quadrum.Decembary;
				break;
			}
			case Twelfth.Twelfth:
			{
				result = Quadrum.Decembary;
				break;
			}
			default:
			{
				result = Quadrum.Undefined;
				break;
			}
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
				int num = (int)(twelfth - 1);
				if (num == -1)
				{
					num = 11;
				}
				result = (Twelfth)(byte)num;
			}
			return result;
		}

		public static Twelfth NextTwelfth(this Twelfth twelfth)
		{
			return (Twelfth)((twelfth != Twelfth.Undefined) ? ((byte)((int)(twelfth + 1) % 12)) : 12);
		}

		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return (float)(((float)(int)twelfth + 0.5) / 12.0);
		}

		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)((float)(int)twelfth / 12.0);
		}

		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 16f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(tile);
				twelfth = Season.Summer.GetFirstTwelfth(vector.y);
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
				while (true)
				{
					twelfth = rootTwelfth;
					rootTwelfth = TwelfthUtility.TwelfthBefore(rootTwelfth);
					if (!twelfths.Contains(rootTwelfth))
						break;
				}
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
				while (true)
				{
					m = rootTwelfth;
					rootTwelfth = TwelfthUtility.TwelfthAfter(rootTwelfth);
					if (!twelfths.Contains(rootTwelfth))
						break;
				}
				result = TwelfthUtility.TwelfthAfter(m);
			}
			return result;
		}

		public static Twelfth TwelfthBefore(Twelfth m)
		{
			return (m != 0) ? (m - 1) : Twelfth.Twelfth;
		}

		public static Twelfth TwelfthAfter(Twelfth m)
		{
			return (m != Twelfth.Twelfth) ? (m + 1) : Twelfth.First;
		}
	}
}
