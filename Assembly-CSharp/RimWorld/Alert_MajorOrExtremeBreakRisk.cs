using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078B RID: 1931
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x06002ADC RID: 10972 RVA: 0x0016A4A4 File Offset: 0x001688A4
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0016A4C0 File Offset: 0x001688C0
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x0016A4DC File Offset: 0x001688DC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskExtreme.Concat(BreakRiskAlertUtility.PawnsAtRiskMajor));
		}
	}
}
