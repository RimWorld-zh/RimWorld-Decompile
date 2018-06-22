using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078A RID: 1930
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x0016A3B5 File Offset: 0x001687B5
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x0016A3D0 File Offset: 0x001687D0
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

		// Token: 0x06002ADD RID: 10973 RVA: 0x0016A3F4 File Offset: 0x001687F4
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

		// Token: 0x06002ADE RID: 10974 RVA: 0x0016A488 File Offset: 0x00168888
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}
	}
}
