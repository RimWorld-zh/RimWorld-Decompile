using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078D RID: 1933
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x06002ADF RID: 10975 RVA: 0x0016A17C File Offset: 0x0016857C
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x0016A198 File Offset: 0x00168598
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x0016A1B4 File Offset: 0x001685B4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskExtreme.Concat(BreakRiskAlertUtility.PawnsAtRiskMajor));
		}
	}
}
