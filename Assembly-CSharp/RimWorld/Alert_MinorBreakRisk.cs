using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Alert_MinorBreakRisk : Alert
	{
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		public override AlertReport GetReport()
		{
			AlertReport result;
			if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>() || BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
			{
				result = false;
			}
			else
			{
				result = AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskMinor);
			}
			return result;
		}
	}
}
