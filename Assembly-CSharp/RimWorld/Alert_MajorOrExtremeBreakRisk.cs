using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078D RID: 1933
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x06002ADD RID: 10973 RVA: 0x0016A0E8 File Offset: 0x001684E8
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x0016A104 File Offset: 0x00168504
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x0016A120 File Offset: 0x00168520
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskExtreme.Concat(BreakRiskAlertUtility.PawnsAtRiskMajor));
		}
	}
}
