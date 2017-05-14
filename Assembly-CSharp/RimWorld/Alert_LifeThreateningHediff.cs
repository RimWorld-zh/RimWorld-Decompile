using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		private IEnumerable<Pawn> SickPawns
		{
			get
			{
				Alert_LifeThreateningHediff.<>c__Iterator186 <>c__Iterator = new Alert_LifeThreateningHediff.<>c__Iterator186();
				Alert_LifeThreateningHediff.<>c__Iterator186 expr_07 = <>c__Iterator;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn current in this.SickPawns)
			{
				stringBuilder.AppendLine("    " + current.NameStringShort);
				foreach (Hediff current2 in current.health.hediffSet.hediffs)
				{
					if (current2.CurStage != null && current2.CurStage.lifeThreatening && current2.Part != null && current2.Part != current.RaceProps.body.corePart)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return string.Format("PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(), stringBuilder.ToString());
			}
			return string.Format("PawnsWithLifeThreateningDiseaseDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs(this.SickPawns.FirstOrDefault<Pawn>());
		}
	}
}
