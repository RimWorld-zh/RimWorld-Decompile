using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000790 RID: 1936
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06002AF6 RID: 10998 RVA: 0x0016B2CE File Offset: 0x001696CE
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x0016B2E0 File Offset: 0x001696E0
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x0016B2FC File Offset: 0x001696FC
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x0016B318 File Offset: 0x00169718
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
