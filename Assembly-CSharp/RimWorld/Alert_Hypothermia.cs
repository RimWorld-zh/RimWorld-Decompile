using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078C RID: 1932
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x06002ADF RID: 10975 RVA: 0x0016A505 File Offset: 0x00168905
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x0016A520 File Offset: 0x00168920
		private IEnumerable<Pawn> HypothermiaDangerColonists
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!p.SafeTemperatureRange().Includes(p.AmbientTemperature))
					{
						Hediff hypo = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (hypo != null && hypo.CurStageIndex >= 3)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x0016A544 File Offset: 0x00168944
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HypothermiaDangerColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return "AlertHypothermiaDesc".Translate(new object[]
			{
				stringBuilder.ToString()
			});
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x0016A5D8 File Offset: 0x001689D8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}
	}
}
