using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class QuadrumUtility
	{
		public static Quadrum FirstQuadrum
		{
			get
			{
				return Quadrum.Aprimay;
			}
		}

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

		public static float GetMiddleYearPct(this Quadrum quadrum)
		{
			return quadrum.GetMiddleTwelfth().GetMiddleYearPct();
		}

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

		public static Season GetSeason(this Quadrum q, float latitude)
		{
			float middleYearPct = q.GetMiddleYearPct();
			return SeasonUtility.GetReportedSeason(middleYearPct, latitude);
		}

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
