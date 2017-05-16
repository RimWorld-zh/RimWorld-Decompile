using System;
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
				Alert_ColonistNeedsRescuing.<>c__Iterator186 <>c__Iterator = new Alert_ColonistNeedsRescuing.<>c__Iterator186();
				Alert_ColonistNeedsRescuing.<>c__Iterator186 expr_07 = <>c__Iterator;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static bool NeedsRescue(Pawn p)
		{
			return p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker) && (p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p));
		}

		public override string GetLabel()
		{
			if (this.ColonistsNeedingRescue.Count<Pawn>() == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn current in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("    " + current.NameStringShort);
			}
			return string.Format("ColonistsNeedRescueDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs(this.ColonistsNeedingRescue.FirstOrDefault<Pawn>());
		}
	}
}
