using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistNeedsTend : Alert
	{
		private IEnumerable<Pawn> NeedingColonists
		{
			get
			{
				foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (item.health.HasHediffsNeedingTendByColony(true))
					{
						Building_Bed curBed = item.CurrentBed();
						if ((curBed == null || !curBed.Medical) && !Alert_ColonistNeedsRescuing.NeedsRescue(item))
						{
							yield return item;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_0114:
				/*Error near IL_0115: Unexpected return in MoveNext()*/;
			}
		}

		public Alert_ColonistNeedsTend()
		{
			base.defaultLabel = "ColonistNeedsTreatment".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn needingColonist in this.NeedingColonists)
			{
				stringBuilder.AppendLine("    " + needingColonist.NameStringShort);
			}
			return string.Format("ColonistNeedsTreatmentDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.NeedingColonists.FirstOrDefault();
			if (pawn == null)
			{
				return false;
			}
			return AlertReport.CulpritIs(pawn);
		}
	}
}
