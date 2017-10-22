using System.Linq;
using Verse;

namespace RimWorld
{
	public class Alert_MinorBreakRisk : Alert
	{
		public Alert_MinorBreakRisk()
		{
			base.defaultPriority = AlertPriority.High;
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
			if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any() || BreakRiskAlertUtility.PawnsAtRiskMajor.Any())
			{
				result = false;
			}
			else
			{
				Pawn pawn = BreakRiskAlertUtility.PawnsAtRiskMinor.FirstOrDefault();
				result = ((pawn == null) ? false : ((Thing)pawn));
			}
			return result;
		}
	}
}
