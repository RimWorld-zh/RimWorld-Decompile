using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000790 RID: 1936
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x0016A7DC File Offset: 0x00168BDC
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

		// Token: 0x06002AEC RID: 10988 RVA: 0x0016A800 File Offset: 0x00168C00
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x0016A820 File Offset: 0x00168C20
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

		// Token: 0x06002AEE RID: 10990 RVA: 0x0016A978 File Offset: 0x00168D78
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}
	}
}
