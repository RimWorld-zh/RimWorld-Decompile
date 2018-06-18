using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078E RID: 1934
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x06002AE2 RID: 10978 RVA: 0x0016A1DD File Offset: 0x001685DD
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x0016A1F8 File Offset: 0x001685F8
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

		// Token: 0x06002AE4 RID: 10980 RVA: 0x0016A21C File Offset: 0x0016861C
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

		// Token: 0x06002AE5 RID: 10981 RVA: 0x0016A2B0 File Offset: 0x001686B0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}
	}
}
