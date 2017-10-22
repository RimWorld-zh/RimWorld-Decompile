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
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (!p.SafeTemperatureRange().Includes(p.AmbientTemperature))
							{
								Hediff hypo = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
								if (hypo != null && hypo.CurStageIndex >= 3)
									break;
							}
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_0119:
				/*Error near IL_011a: Unexpected return in MoveNext()*/;
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
			return (pawn != null) ? AlertReport.CulpritIs((Thing)pawn) : false;
		}
	}
}
