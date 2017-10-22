using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		private IEnumerable<Pawn> ColonistsNeedingRescue
		{
			get
			{
				foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(item))
					{
						yield return item;
					}
				}
			}
		}

		public static bool NeedsRescue(Pawn p)
		{
			if (p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker))
			{
				if (p.jobs.jobQueue != null && p.jobs.jobQueue.Count > 0 && p.jobs.jobQueue.Peek().job.CanBeginNow(p))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public override string GetLabel()
		{
			if (this.ColonistsNeedingRescue.Count() == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn item in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("    " + item.NameStringShort);
			}
			return string.Format("ColonistsNeedRescueDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs((Thing)this.ColonistsNeedingRescue.FirstOrDefault());
		}
	}
}
