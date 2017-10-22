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
			if (!BreakRiskAlertUtility.PawnsAtRiskExtreme.Any() && !BreakRiskAlertUtility.PawnsAtRiskMajor.Any())
			{
				Pawn pawn = BreakRiskAlertUtility.PawnsAtRiskMinor.FirstOrDefault();
				if (pawn != null)
				{
					return (Thing)pawn;
				}
				return false;
			}
			return false;
		}
	}
}
