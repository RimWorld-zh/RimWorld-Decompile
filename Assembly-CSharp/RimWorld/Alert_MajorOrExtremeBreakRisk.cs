using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078B RID: 1931
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x0016A708 File Offset: 0x00168B08
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x0016A724 File Offset: 0x00168B24
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0016A740 File Offset: 0x00168B40
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskExtreme.Concat(BreakRiskAlertUtility.PawnsAtRiskMajor));
		}
	}
}
