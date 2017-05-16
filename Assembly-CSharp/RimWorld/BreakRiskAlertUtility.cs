using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public static class BreakRiskAlertUtility
	{
		public static IEnumerable<Pawn> PawnsAtRiskExtreme
		{
			get
			{
				BreakRiskAlertUtility.<>c__Iterator18C <>c__Iterator18C = new BreakRiskAlertUtility.<>c__Iterator18C();
				BreakRiskAlertUtility.<>c__Iterator18C expr_07 = <>c__Iterator18C;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> PawnsAtRiskMajor
		{
			get
			{
				BreakRiskAlertUtility.<>c__Iterator18D <>c__Iterator18D = new BreakRiskAlertUtility.<>c__Iterator18D();
				BreakRiskAlertUtility.<>c__Iterator18D expr_07 = <>c__Iterator18D;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> PawnsAtRiskMinor
		{
			get
			{
				BreakRiskAlertUtility.<>c__Iterator18E <>c__Iterator18E = new BreakRiskAlertUtility.<>c__Iterator18E();
				BreakRiskAlertUtility.<>c__Iterator18E expr_07 = <>c__Iterator18E;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static string AlertLabel
		{
			get
			{
				int num = BreakRiskAlertUtility.PawnsAtRiskExtreme.Count<Pawn>();
				string text;
				if (num > 0)
				{
					text = "BreakRiskExtreme".Translate();
				}
				else
				{
					num = BreakRiskAlertUtility.PawnsAtRiskMajor.Count<Pawn>();
					if (num > 0)
					{
						text = "BreakRiskMajor".Translate();
					}
					else
					{
						num = BreakRiskAlertUtility.PawnsAtRiskMinor.Count<Pawn>();
						text = "BreakRiskMinor".Translate();
					}
				}
				if (num > 1)
				{
					text = text + " x" + num.ToStringCached();
				}
				return text;
			}
		}

		public static string AlertExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>())
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn current in BreakRiskAlertUtility.PawnsAtRiskExtreme)
					{
						stringBuilder2.AppendLine("    " + current.NameStringShort);
					}
					stringBuilder.Append("BreakRiskExtremeDesc".Translate(new object[]
					{
						stringBuilder2
					}));
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					foreach (Pawn current2 in BreakRiskAlertUtility.PawnsAtRiskMajor)
					{
						stringBuilder3.AppendLine("    " + current2.NameStringShort);
					}
					stringBuilder.Append("BreakRiskMajorDesc".Translate(new object[]
					{
						stringBuilder3
					}));
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMinor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (Pawn current3 in BreakRiskAlertUtility.PawnsAtRiskMinor)
					{
						stringBuilder4.AppendLine("    " + current3.NameStringShort);
					}
					stringBuilder.Append("BreakRiskMinorDesc".Translate(new object[]
					{
						stringBuilder4
					}));
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("BreakRiskDescEnding".Translate());
				return stringBuilder.ToString();
			}
		}
	}
}
