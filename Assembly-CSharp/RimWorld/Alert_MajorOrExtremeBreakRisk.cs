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
			if (pawn != null)
			{
				return (Thing)pawn;
			}
			pawn = BreakRiskAlertUtility.PawnsAtRiskMajor.FirstOrDefault();
			if (pawn != null)
			{
				return (Thing)pawn;
			}
			return false;
		}
	}
}
