using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000794 RID: 1940
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06002AFD RID: 11005 RVA: 0x0016B0F6 File Offset: 0x001694F6
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x0016B108 File Offset: 0x00169508
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x0016B124 File Offset: 0x00169524
		public override string GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0016B140 File Offset: 0x00169540
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
