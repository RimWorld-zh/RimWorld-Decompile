using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_Hypothermia : Alert_Critical
	{
		private IEnumerable<Pawn> HypothermiaDangerColonists
		{
			get
			{
				foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (!item.SafeTemperatureRange().Includes(item.AmbientTemperature))
					{
						Hediff hypo = item.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (hypo != null && hypo.CurStageIndex >= 3)
						{
							yield return item;
						}
					}
				}
			}
		}

		public Alert_Hypothermia()
		{
			base.defaultLabel = "AlertHypothermia".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn hypothermiaDangerColonist in this.HypothermiaDangerColonists)
			{
				stringBuilder.AppendLine("    " + hypothermiaDangerColonist.NameStringShort);
			}
			return "AlertHypothermiaDesc".Translate(stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.HypothermiaDangerColonists.FirstOrDefault();
			if (pawn == null)
			{
				return false;
			}
			return AlertReport.CulpritIs((Thing)pawn);
		}
	}
}
