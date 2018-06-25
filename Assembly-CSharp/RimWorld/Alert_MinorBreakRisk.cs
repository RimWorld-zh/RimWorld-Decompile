using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06002AF9 RID: 11001 RVA: 0x0016B682 File Offset: 0x00169A82
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x0016B694 File Offset: 0x00169A94
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0016B6B0 File Offset: 0x00169AB0
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x0016B6CC File Offset: 0x00169ACC
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
