using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078F RID: 1935
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x0016A460 File Offset: 0x00168860
		private IEnumerable<Pawn> ColonistsNeedingRescue
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(p))
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x0016A484 File Offset: 0x00168884
		public static bool NeedsRescue(Pawn p)
		{
			return p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker) && (p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false));
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x0016A518 File Offset: 0x00168918
		public override string GetLabel()
		{
			string result;
			if (this.ColonistsNeedingRescue.Count<Pawn>() == 1)
			{
				result = "ColonistNeedsRescue".Translate();
			}
			else
			{
				result = "ColonistsNeedRescue".Translate();
			}
			return result;
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x0016A558 File Offset: 0x00168958
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistsNeedRescueDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x0016A5E8 File Offset: 0x001689E8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}
	}
}
