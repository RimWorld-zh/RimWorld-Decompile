using System.Linq;
using Verse;

namespace RimWorld
{
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
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
			Pawn pawn = BreakRiskAlertUtility.PawnsAtRiskExtreme.FirstOrDefault();
			AlertReport result;
			if (pawn != null)
			{
				result = (Thing)pawn;
			}
			else
			{
				pawn = BreakRiskAlertUtility.PawnsAtRiskMajor.FirstOrDefault();
				result = ((pawn == null) ? false : ((Thing)pawn));
			}
			return result;
		}
	}
}
