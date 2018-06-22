using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000789 RID: 1929
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x06002AD8 RID: 10968 RVA: 0x0016A354 File Offset: 0x00168754
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x0016A370 File Offset: 0x00168770
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x0016A38C File Offset: 0x0016878C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskExtreme.Concat(BreakRiskAlertUtility.PawnsAtRiskMajor));
		}
	}
}
