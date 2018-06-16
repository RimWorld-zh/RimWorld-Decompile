using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000794 RID: 1940
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06002AFB RID: 11003 RVA: 0x0016B062 File Offset: 0x00169462
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x0016B074 File Offset: 0x00169474
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x0016B090 File Offset: 0x00169490
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x0016B0AC File Offset: 0x001694AC
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
