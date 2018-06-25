using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078E RID: 1934
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x0016ADFC File Offset: 0x001691FC
		private IEnumerable<Pawn> SickPawns
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
					{
						Hediff diff = p.health.hediffSet.hediffs[i];
						if (diff.CurStage != null && diff.CurStage.lifeThreatening && !diff.FullyImmune())
						{
							yield return p;
							break;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x0016AE20 File Offset: 0x00169220
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x0016AE40 File Offset: 0x00169240
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn pawn in this.SickPawns)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && hediff.Part != null && hediff.Part != pawn.RaceProps.body.corePart)
					{
						flag = true;
						break;
					}
				}
			}
			string result;
			if (flag)
			{
				result = string.Format("PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(), stringBuilder.ToString());
			}
			else
			{
				result = string.Format("PawnsWithLifeThreateningDiseaseDesc".Translate(), stringBuilder.ToString());
			}
			return result;
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x0016AF98 File Offset: 0x00169398
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}
	}
}
