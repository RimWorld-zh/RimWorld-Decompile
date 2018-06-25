using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06002AFA RID: 11002 RVA: 0x0016B41E File Offset: 0x0016981E
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0016B430 File Offset: 0x00169830
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x0016B44C File Offset: 0x0016984C
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x0016B468 File Offset: 0x00169868
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
