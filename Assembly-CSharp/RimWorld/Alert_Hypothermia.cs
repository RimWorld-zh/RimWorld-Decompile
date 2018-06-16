using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078E RID: 1934
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x06002AE0 RID: 10976 RVA: 0x0016A149 File Offset: 0x00168549
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x0016A164 File Offset: 0x00168564
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

		// Token: 0x06002AE2 RID: 10978 RVA: 0x0016A188 File Offset: 0x00168588
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

		// Token: 0x06002AE3 RID: 10979 RVA: 0x0016A21C File Offset: 0x0016861C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}
	}
}
