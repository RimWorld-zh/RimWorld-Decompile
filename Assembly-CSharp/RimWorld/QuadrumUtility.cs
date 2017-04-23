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
				return Quadrum.Decembary;
			}
		}

		public static Twelfth GetFirstTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Decembary:
				return Twelfth.First;
			case Quadrum.Aprimay:
				return Twelfth.Fourth;
			case Quadrum.Jugust:
				return Twelfth.Seventh;
			case Quadrum.Septober:
				return Twelfth.Tenth;
			default:
				return Twelfth.Undefined;
			}
		}

		public static string Label(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Decembary:
				return "QuadrumDecembary".Translate();
			case Quadrum.Aprimay:
				return "QuadrumAprimay".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		public static string QuadrumsRangeLabel(List<Twelfth> twelfths)
		{
			if (twelfths.Count == 0)
			{
				return string.Empty;
			}
			if (twelfths.Count == 12)
			{
				return "WholeYear".Translate();
			}
			string text = string.Empty;
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
			return text;
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
					}));
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.QuadrumDateStringAt(leftMostTwelfth) + " - " + GenDate.QuadrumDateStringAt(rightMostTwelfth);
		}
	}
}
